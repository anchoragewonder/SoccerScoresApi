using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace SoccerScoresApi.ResponseModel
{
    public class ScoreUpdateModel
    {

        [JsonProperty(PropertyName = "date", Order = 1)]
        public string date { get; set; }

        [JsonProperty(PropertyName = "homeTeam", Order = 2)]
        public string homeTeam { get; set; }

        [JsonProperty(PropertyName = "awayTeam", Order = 3)]
        public string awayTeam { get; set; }

        [JsonProperty(PropertyName = "homeScore", Order = 4)]
        public int? homeScore { get; set; }

        [JsonProperty(PropertyName = "awayScore", Order = 5)]
        public int? awayScore { get; set; }

        public bool isValid
        {
            get
            {
                if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(homeTeam) || string.IsNullOrEmpty(awayTeam))
                {
                    return false;
                }
                if(homeScore == null || awayScore == null)
                {
                    return false;
                }
                return true;
            }
        }

        public ScoreUpdateModel() { }

        public ScoreUpdateModel(TableModel.SoccerTable model)
        {
            this.date = model.date;
            this.homeTeam = model.homeTeam;
            this.awayTeam = model.awayTeam;
            this.homeScore = model.homeScore;
            this.awayScore = model.awayScore;
        }
    }
}
