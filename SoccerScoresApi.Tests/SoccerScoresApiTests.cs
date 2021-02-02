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
using SoccerScoresApi.Functions;

using System.Text.RegularExpressions;
using Xunit.Abstractions;

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

            ScoreUpdateModel model = new ScoreUpdateModel("date", "home", "away", 1, 0);
            List<ScoreUpdateModel> list = new List<ScoreUpdateModel>() { model };
            var body = new ScoreResponseModel(list);
            request.Body = JsonConvert.SerializeObject(body);

            TestLambdaContext testContext = new TestLambdaContext();

            var response = await func.Execute(request, testContext);
            Assert.NotNull(response.Body);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task TestEmptyGetFunction()
        {
            ScoreGetFunction func = new ScoreGetFunction();
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            TestLambdaContext testContext = new TestLambdaContext();
           
            var response = await func.Execute(request, testContext);

            Assert.NotNull(response.Body);
            Assert.Equal(200, response.StatusCode);
        }

    }
}
