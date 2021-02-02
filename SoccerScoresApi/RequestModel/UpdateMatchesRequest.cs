using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Newtonsoft.Json;
using SoccerScoresApi.MatchModels;
using SoccerScoresApi.TableModel;

namespace SoccerScoresApi.RequestModel
{
    public class UpdateMatchesRequest
    {
        [JsonProperty(PropertyName = "matches")]
        public List<MatchModel> Matches;

        public bool isValid { get { return Matches.FirstOrDefault(i => i.isValid == false) == null; } }

        public UpdateMatchesRequest() { }

        public UpdateMatchesRequest(List<MatchModel> list) {
            this.Matches = list;
        }

        public UpdateMatchesRequest(List<ScoreTable> list)
        {
            List<MatchModel> matches = new List<MatchModel>();
            foreach (ScoreTable tableInstance in list)
            {
                matches.Add(new MatchModel(tableInstance));
            }
            this.Matches = matches;
        }
    }
}
