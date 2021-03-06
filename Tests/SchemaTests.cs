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

namespace Tests
{
    public class SchemaTests
    {
        private ITestOutputHelper _output;
        private ServiceProvider _provider = null;
        private IServiceCollection _services = null;

        public SchemaTests(ITestOutputHelper output)
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

        [Theory]
        [InlineData(@"{ human(id: ""2"") { fullname id} }", @"{""data"":{""human"":{""fullname"": ""Vader"",""id"": ""2""}}}")]
        [InlineData(@"{ human(id: ""1"") { fullname id} }", @"{""data"":{""human"":{""fullname"": ""Luke"",""id"": ""1""}}}")]
        public void ValidSchema(string query, string expected)
        {
            //                
            StarWarsSchema swss = this._provider.GetRequiredService<StarWarsSchema>();
            var result = swss.Execute(x =>
            {
                x.Query = query;
            });

            _output.WriteLine("Query:" + query);
            _output.WriteLine("Result:" + result);
            JToken actualJson = JToken.Parse(result);
            JToken expectedJson = JToken.Parse(expected);
            Assert.Equal(expectedJson.ToString(), actualJson.ToString());
        }

        [Theory]
        [InlineData(@"{ human(id: ""2"") { fullname id} }",
            @"{""data"":{""human"":{""fullname"": ""Vader"",""id"": ""2""}}}")]
        public void ValidSchemaVariables(string query, string expected)
        {
            //                
            StarWarsSchema swss = this._provider.GetRequiredService<StarWarsSchema>();
            var result = swss.Execute(x =>
            {
                
                x.Query = query;
            });

            _output.WriteLine("Query:" + query);
            _output.WriteLine("Result:" + result);
            JToken actualJson = JToken.Parse(result);
            JToken expectedJson = JToken.Parse(expected);
            Assert.Equal(expectedJson.ToString(), actualJson.ToString());
        }
    }
}
