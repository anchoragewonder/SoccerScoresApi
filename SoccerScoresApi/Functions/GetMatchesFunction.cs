using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

using SoccerScoresApi.MatchModels;
using SoccerScoresApi.DbSchema;
using System.Threading.Tasks;
using SoccerScoresApi.RequestModel;
using SoccerScoresApi.TableModel;

namespace SoccerScoresApi.Functions
{
    public class GetMatchesFunction
    {
        private const string EXAMPLE_TEXT = "Here is how to use my api." +
            "Enter a premier league team name and the scores and fixtures for all of thier  matches will be generated.";

        public GetMatchesFunction()
        {
        }

        public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {

            if (apigProxyEvent.PathParameters == null)
            {
                List<MatchModel> example = await GetScores("ManchesterUnited");
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
                List<MatchModel> response = await GetScores(name);

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
    
        public async Task<List<MatchModel>> GetScores(string name)
        {
            List<ScoreTable> tableModel = await SoccerTableFunctionsHandler.GetMatchesForTeam(name);
            List<MatchModel> totalResponses = new List<MatchModel>();
            tableModel.ForEach(i => totalResponses.Add(new MatchModel(i)));
            return totalResponses;
        }
    }
}
