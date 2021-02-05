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
using SoccerScoresApi.RequestModel;
using Newtonsoft.Json;
using SoccerScoresApi.MatchModels;
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
            UpsertMatchesFunction func = new UpsertMatchesFunction();
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();

            MatchModels.MatchModel model = new MatchModels.MatchModel("date", "home", "away", 1, 1);
            List<MatchModels.MatchModel> list = new List<MatchModels.MatchModel>() { model };
            var body = new UpdateMatchesRequest(list);
            request.Body = JsonConvert.SerializeObject(body);

            TestLambdaContext testContext = new TestLambdaContext();

            var response = await func.Execute(request, testContext);
            Assert.NotNull(response.Body);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task TestEmptyGetFunction()
        {
            GetMatchesFunction func = new GetMatchesFunction();
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            request.PathParameters = new Dictionary<string, string>();
            TestLambdaContext testContext = new TestLambdaContext();

            var response = await func.Execute(request, testContext);

            Assert.NotNull(response.Body);
            Assert.Equal(200, response.StatusCode);
            Assert.Contains(GetMatchesFunction.EXAMPLE_HEADER, response.Body);
        }

        [Fact]
        public async Task TestGetFunction()
        {
            await TestUpdateFunction();

            GetMatchesFunction func = new GetMatchesFunction();
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            TestLambdaContext testContext = new TestLambdaContext();
            request.PathParameters = new Dictionary<string, string>()
            {
                { "name", "home" }
            };

            var response = await func.Execute(request, testContext);

            Assert.NotNull(response.Body);
            Assert.Equal(200, response.StatusCode);

            var json = JsonConvert.DeserializeObject<List<MatchModel>>(response.Body);
            Assert.NotEmpty(json);
        }
    }
}
