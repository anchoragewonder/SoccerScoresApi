using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

using SoccerScoresApi.RequestModel;
using SoccerScoresApi.DbSchema;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace SoccerScoresApi
{
    public class ScoreUpdateFunction
    {
        
        public ScoreUpdateFunction()
        {
        }

        public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            ScoreResponseModel jsonRequest;

            try
            {
                jsonRequest = JsonConvert.DeserializeObject<ScoreResponseModel>(apigProxyEvent.Body);
                if (!jsonRequest.isValid)
                {
                    // check for valid json from scraper
                    return new APIGatewayProxyResponse
                    {
                        Body = " Improper data for updating scores",
                        StatusCode = 503,
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                    };
                }
                bool response = await this.GetResponse(jsonRequest);
                if(!response)
                {
                    return new APIGatewayProxyResponse
                    {
                        Body = " Error Update did not execute properly",
                        StatusCode = 503,
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                    };
                }

                return new APIGatewayProxyResponse
                {
                    Body = "Succesfully updated the score table.",
                    StatusCode = 200,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    Body = $"Score table was not updated {ex.Message}.",
                    StatusCode = 403,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
            }
        }
        public async Task<bool> GetResponse(ScoreResponseModel request)
        {
            TableQuery scores = new TableQuery();
            bool didUpdate = await scores.UpdateScore(request);

            return didUpdate;
        }

    }
}
