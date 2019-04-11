﻿using Fluxter.SteamWebAPI;
using Fluxter.SteamWebAPI.Fluent;
using SteamSearchApi.Helpers;
using SteamSearchApi.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using System.Configuration;


namespace SteamSearchApi.Models.Repositories {
    public class SteamRepository {

        public Player GetUser(string steamId, string ip) {
            var repo = new MongoRepository<UserQuery>();

            Player dbPlayer = null;
            if (!Util.isValidSteamId(steamId)) // before we send this SteamID up in the request we check if its even in a valid format
            {

                dbPlayer = repo.Get<Player>(x => x.Personaname.ToLower() == steamId.ToLower()).FirstOrDefault(); // do we have a record of that in our DB

                if (dbPlayer != null)
                    steamId = dbPlayer.Steamid; // Found it.
                else
                    steamId = GetSteamId(steamId); // no Go Request it from Steam



            }


            var req = SteamWebAPI.CustomRequest("ISteamUser", "GetPlayerSummaries", "v0002", new { steamids = steamId });
            var response = _HandleResponse<HttpUserResponse>(req);



            var query = new UserQuery() {
                SteamId = steamId,
                IpAddress = ip,
                DateCreated = DateTime.Now
            };

            repo.Insert(query);

            var player = response.Response.Players.First(); // get the player object from Steam

            if (!repo.Exists<Player>(x => x.Steamid == steamId)) { // do we already have a record in the DB for this user
                repo.Insert(player); // Nope, insert them
            } else {

                dbPlayer = dbPlayer ?? repo.Get<Player>(x => x.Steamid == steamId).FirstOrDefault(); // get the Player from the DB. 
                
                // has the user changed their display name
                if (player.Personaname != dbPlayer.Personaname) {
                    dbPlayer.Personaname = player.Personaname;
                    repo.Update(dbPlayer);
                }

            }



            // have we got this player Already


            return player;
        }
        public string GetRecentGames(string steamId) {


            return "";
        }
        public string GetOwnedGames(string steamId) {


            return "";
        }
        public string GetFriends(string steamId) {

            return "";
        }
        public string GetGamesInCommon(string steamId, string query) {


            return "";
        }
        public string GetSteamId(string username) {

            var req = SteamWebAPI.CustomRequest("ISteamUser", "ResolveVanityURL", "v0001", new { vanityurl = username });

            var result = _HandleResponse<HttpGetVanityUrlResponse>(req);

            if (result == null || result.Response.SteamId == null)
                throw new Exception("Unable to convert to Valid SteamID");


            return result.Response.SteamId;

        }


        private T _HandleResponse<T>(SteamCustomBuilder request) {
            var sResponse = request.GetResponseString(RequestFormat.JSON);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sResponse);
        }


    }
}