﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SoccerScoresApi.ResponseModel
{
    public class FixtureUpdateModel
    {
        [JsonProperty(PropertyName = "date", Order = 1)]
        public string date { get; set; }

        [JsonProperty(PropertyName = "homeTeam", Order = 2)]
        public string homeTeam { get; set; }

        [JsonProperty(PropertyName = "awayTeam", Order = 3)]
        public string awayTeam { get; set; }  

        public FixtureUpdateModel() { }

        public FixtureUpdateModel(TableModel.SoccerTable model)
        {
            this.date = model.date;
            this.homeTeam = model.homeTeam;
            this.awayTeam = model.awayTeam;
           
        }
    }
}