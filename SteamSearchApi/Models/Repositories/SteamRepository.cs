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


namespace SteamSearchApi.Models.Repositories
{
    public class SteamRepository
    {

        public Player GetUser(string steamId, string ip)
        {
            var mongoRepo = new MongoRepository<Query>();
            if (!Util.isValidSteamId(steamId)) // before we send this SteamID up in the request we check if its even in a valid format
            {
                if (mongoRepo.Exists<Player>(x => x.Personaname.ToLower() == steamId.ToLower()))
                {
                    steamId = mongoRepo.Get<Player>(x => x.Personaname.ToLower() == steamId.ToLower()).FirstOrDefault().Steamid;
                }
                else
                {
                    steamId = GetSteamId(steamId);
                }


            }


            var req = SteamWebAPI.CustomRequest("ISteamUser", "GetPlayerSummaries", "v0002", new { steamids = steamId });


            var response = _HandleResponse<HttpUserResponse>(req);



            var query = new Query()
            {
                SteamId = steamId,
                IpAddress = ip,
                DateCreated = DateTime.Now
            };

            mongoRepo.Insert(query);

            var player = response.Response.Players.First();

            if (!mongoRepo.Exists<Player>(x => x.Steamid == steamId))
                mongoRepo.Insert(player);

            // have we got this player Already


            return player;
        }
        public string GetRecentGames(string steamId)
        {


            return "";
        }
        public string GetOwnedGames(string steamId)
        {


            return "";
        }
        public string GetFriends(string steamId)
        {

            return "";
        }
        public string GetGamesInCommon(string steamId, string query)
        {


            return "";
        }
        public string GetSteamId(string username)
        {

            var req = SteamWebAPI.CustomRequest("ISteamUser", "ResolveVanityURL", "v0001", new { vanityurl = username });

            var result = _HandleResponse<HttpGetVanityUrlResponse>(req);

            if (result == null || result.Response.SteamId == null)
                throw new Exception("Unable to convert to Valid SteamID");


            return result.Response.SteamId;

        }


        private T _HandleResponse<T>(SteamCustomBuilder request)
        {
            var sResponse = request.GetResponseString(RequestFormat.JSON);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sResponse);
        }


    }
}