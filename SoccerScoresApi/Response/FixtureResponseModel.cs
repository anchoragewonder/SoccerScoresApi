using Newtonsoft.Json;
using SoccerScoresApi.ResponseModel;
using SoccerScoresApi.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoccerScoresApi.RequestModel
{
    public class FixtureResponseModel
    {
        [JsonProperty(PropertyName = "matches")]
        public List<FixtureUpdateModel> Matches;

        public bool isValid { get { return Matches.FirstOrDefault(i => i.isValid == false) == null; } }

        public FixtureResponseModel() { }

        public FixtureResponseModel(List<FixtureTable> list)
        {
            List<FixtureUpdateModel> matches = new List<FixtureUpdateModel>();
            foreach (FixtureTable tableInstance in list)
            {
                matches.Add(new FixtureUpdateModel(tableInstance));
            }
            this.Matches = matches;
        }
    }
}
