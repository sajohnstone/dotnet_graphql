using System;
using Xunit;
using App;
using GraphQL;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using GraphQL.Types;
using StarWars.Types;

namespace Tests
{
    public class SchemaTests
    {
        private ServiceProvider Provider = null;
        private IServiceCollection Services = null;
        public SchemaTests()
        {
            //setup our DI
            this.Services = new ServiceCollection()
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

            this.Provider = Services.BuildServiceProvider();
        }

        [Theory]
        [InlineData(@"{ human(id: ""2"") { name id} }", @"{""data"":{""human"":{""name"": ""Vader"",""id"": ""2""}}}")]
        [InlineData(@"{ human(id: ""1"") { name id} }", @"{""data"":{""human"":{""name"": ""Luke"",""id"": ""1""}}}")]
        public void ValidSchema(string query, string expected)
        {
            //                
            StarWarsSchema swss = this.Provider.GetRequiredService<StarWarsSchema>();
            var result = swss.Execute(x =>
            {
                x.Query = query;
            });

            JToken actualJson = JToken.Parse(result);
            JToken expectedJson = JToken.Parse(expected);
            Assert.Equal(expectedJson.ToString(), actualJson.ToString());
        }
    }
}
