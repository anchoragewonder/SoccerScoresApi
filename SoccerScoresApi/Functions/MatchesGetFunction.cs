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

namespace SoccerScoresApi.Functions
{
    public class MatchesGetFunction
    {
        public MatchesGetFunction()
        {
        }

        public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {

        }
    }
}
