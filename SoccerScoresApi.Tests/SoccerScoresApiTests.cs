using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using SoccerScoresApi;
using SoccerScoresApi.Extension;
using SoccerScoresApi.ResponseModel;
using Newtonsoft.Json;
using SoccerScoresApi.RequestModel;

namespace SoccerScoresApi.Tests
{
    public class SoccerScoresApiTests
    {
        public SoccerScoresApiTests()
        {
        }

        [Fact]
        public async void TestDBConnection()
        {
            DbConnector connector = new DbConnector();
            bool is_connected = await connector.IsConnected();
            Assert.True(is_connected);
            await connector.Disconnect();
        }

        [Fact]
        public async Task TestUpdateFunction()
        {
            ScoreUpdateFunction func = new ScoreUpdateFunction();
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();

            // insert json here with a few values?
            var body = new ScoreRequest(testJson);
            request.Body = JsonConvert.SerializeObject(body);

            TestLambdaContext testContext = new TestLambdaContext();


            var response = await func.Execute(request, testContext);
            Assert.NotNull(response.Body);
            Assert.Equal(200, response.StatusCode);

        }
    }   
}
