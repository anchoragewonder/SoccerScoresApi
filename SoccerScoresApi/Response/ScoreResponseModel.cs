using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Newtonsoft.Json;
using SoccerScoresApi.ResponseModel;
using SoccerScoresApi.TableModel;

namespace SoccerScoresApi.RequestModel
{
    public class ScoreResponseModel
    {
        [JsonProperty(PropertyName = "matches")]
        public List<ScoreUpdateModel> Matches;

        public bool isValid { get { return Matches.FirstOrDefault(i => i.isValid == false) == null; } }

        public ScoreResponseModel() { }

        public ScoreResponseModel(List<ScoreUpdateModel> list) {
            this.Matches = list;
        }

        public ScoreResponseModel(List<ScoreTable> list)
        {
            List<ScoreUpdateModel> matches = new List<ScoreUpdateModel>();
            foreach (ScoreTable tableInstance in list)
            {
                matches.Add(new ScoreUpdateModel(tableInstance));
            }
            this.Matches = matches;
        }
    }
}
