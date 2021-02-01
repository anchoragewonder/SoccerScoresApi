using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

using SoccerScoresApi.RequestModel;
using SoccerScoresApi.DbSchema;
using System.Threading.Tasks;
using SoccerScoresApi.ResponseModel;
using SoccerScoresApi.TableModel;

namespace SoccerScoresApi.Functions
{
    public class MatchesGetFunction
    {
        public MatchesGetFunction()
        {
        }

        public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            _ = apigProxyEvent.PathParameters.TryGetValue("name", out string name);

            try
            {

            }
        }
        public async Task<List<ScoreUpdateModel>> GetScores(string name)
        {
            TableQuery newScore = new TableQuery();
            List<SoccerTable> tableModel = await newScore.GetTeam(name);
            List<ScoreUpdateModel> totalResponses = new List<ScoreUpdateModel>();

            foreach (SoccerTable match in tableModel)
            {

                ScoreUpdateModel response = new ScoreUpdateModel(match);

                totalResponses.Add(response);
            }
                
            return totalResponses;
        }
    }
}
