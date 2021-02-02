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
        private const string EXAMPLE_TEXT = "Here is how to use my api." +
            "Enter a premier league team name and the scores and fixtures for all of thier  matches will be generated.";

        public MatchesGetFunction()
        {
        }

        public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {

            if (apigProxyEvent.PathParameters == null)
            {
                List<ScoreUpdateModel> example = await GetScores("Manchester United");
                return new APIGatewayProxyResponse
                {
                    Body = $"{EXAMPLE_TEXT}\n{JsonConvert.SerializeObject(example, Formatting.Indented)}",
                    StatusCode = 200,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }

            _ = apigProxyEvent.PathParameters.TryGetValue("name", out string name);

            try
            {
                List<ScoreUpdateModel> response = await GetScores(name);

                return new APIGatewayProxyResponse
                {
                    Body = JsonConvert.SerializeObject(response, Formatting.Indented),
                    StatusCode = 200,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
            catch(Exception)
            {
                return new APIGatewayProxyResponse
                {
                    Body = $"No Team found with the name: {name}",
                    StatusCode = 403,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
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
