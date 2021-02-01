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

            ScoreRequest jsonRequest;

            try
            {
                jsonRequest = JsonConvert.DeserializeObject<ScoreRequest>(apigProxyEvent.Body);
                if (!jsonRequest.isValid)
                {

                }
                var response = await this.GetResponse(jsonRequest);

                return new APIGatewayProxyResponse
                {
                    Body = "Succesfully updated the score table.",
                    StatusCode = 200,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    Body = $"Score table was not updated {ex.Message}.",
                    StatusCode = 403,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };

            }

        }


        public async Task<bool> GetResponse(ScoreRequest request)
        {
            //call db connection
            //call insert
            TableQuery scores = new TableQuery();
            //return did it work
            return true;
        }

    }
}
