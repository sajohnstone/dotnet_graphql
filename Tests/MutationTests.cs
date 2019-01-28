using System;
using Xunit;
using App;
using GraphQL;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using GraphQL.Types;
using StarWars.Types;
using Xunit.Abstractions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
    public class MutationTests
    {
        private ITestOutputHelper _output;
        private ServiceProvider _provider = null;
        private IServiceCollection _services = null;

        public MutationTests(ITestOutputHelper output)
        {
            this._output = output;

            //setup our DI
            this._services = new ServiceCollection()
                .AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService))
                .AddSingleton<IDocumentExecuter, DocumentExecuter>()
                .AddSingleton<StarWarsData>()
                .AddSingleton<StarWarsQuery>()
                .AddSingleton<StarWarsMutation>()
                .AddSingleton<HumanType>()
                .AddSingleton<HumanInputType>()
                .AddSingleton<DroidType>()
                .AddSingleton<CharacterInterface>()
                .AddSingleton<EpisodeEnum>()
                .AddSingleton<ISchema, StarWarsSchema>()
                .AddSingleton<StarWarsSchema>();

            this._provider = _services.BuildServiceProvider();
        }

        [Fact]
        public void TestMutation()
        {
            string query = @"{ ""query"": ""mutation ($human:HumanInput!){ createHuman(human: $human) { id fullname } }"", ""variables"": { ""human"": { ""fullname"": ""Bobba Fett"",""id"": ""3"" } } }";
            string expected = @"{ ""data"": { ""createHuman"": { ""id"": ""3"",""fullname"": ""Bobba Fett"" } } }";

            StarWarsSchema swss = this._provider.GetRequiredService<StarWarsSchema>();
            var result = swss.Execute(_ =>
            {
                //get the input into GraphQL format
                GraphQL.Inputs queryInputs = query.ToInputs();
                foreach (KeyValuePair<string, object> qIn in queryInputs)
                {
                    if (qIn.Key == "variables")
                    {
                        string json = JsonConvert.SerializeObject(qIn.Value, Formatting.Indented);
                        _.Inputs = json.ToInputs();
                    }
                    if (qIn.Key == "query") _.Query = qIn.Value.ToString();
                }
            });

            _output.WriteLine("Query:" + query);
            _output.WriteLine("Result:" + result);
            JToken actualJson = JToken.Parse(result);
            JToken expectedJson = JToken.Parse(expected);
            Assert.Equal(expectedJson.ToString(), actualJson.ToString());
        }
    }
}
