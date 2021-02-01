using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Newtonsoft.Json;
using SoccerScoresApi.ResponseModel;
using SoccerScoresApi.TableModel;

namespace SoccerScoresApi.RequestModel
{
    public class ScoreRequest
    {
        [JsonProperty(PropertyName = "matches")]
        public List<ScoreUpdateModel> Matches;

        public bool isValid { get { return Matches.FirstOrDefault(i => i.isValid == false) == null; } }

        public ScoreRequest() { }

        public ScoreRequest(List<SoccerTable> list)
        {
            List<ScoreUpdateModel> matches = new List<ScoreUpdateModel>();
            foreach (SoccerTable tableInstance in list)
            {
                matches.Add(new ScoreUpdateModel(tableInstance));
            }
            this.Matches = matches;
        }
    }
}
