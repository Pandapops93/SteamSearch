﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="GetSchemaRequest.cs"
//             company="fluxter.net">
//       Copyright (c) fluxter.net. All rights reserved.
//  </copyright>
//  <author>Marcel Kallen</author>
//  <created>09.08.2018 - 19:42</created>
// ------------------------------------------------------------------------------------------------

namespace Fluxter.SteamWebAPI.Interfaces.Game.Generic.IEconItems.GetSchema
{
    public class GetSchemaRequest : GameRequest
    {
        public GetSchemaRequest(string key, int appID) : base(key, appID)
        {
        }

        protected internal override string EndpointUrl
        {
            get { return string.Format("http://api.steampowered.com/IEconItems_{0}/GetSchema/v0001/", this.AppID); }
        }
    }
}