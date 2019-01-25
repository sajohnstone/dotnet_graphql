using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using StarWars.Types;
using System;

namespace App
{
    public class Program
    {
        public static ISchema MySchema
        {
            get
            {
                return Schema.For(@"
                    type Query {
                        hello: String
                    }
                ");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Star Wars GraphQL Implementation");
            Console.WriteLine("Please enter a valid quiery:");
            Console.ReadLine();

            //setup our DI
            var services = new ServiceCollection()
                .AddSingleton<StarWarsData>()
                .AddSingleton<StarWarsQuery>()
                .AddSingleton<StarWarsMutation>()
                .AddSingleton<HumanType>()
                .AddSingleton<HumanInputType>()
                .AddSingleton<DroidType>()
                .AddSingleton<CharacterInterface>()
                .AddSingleton<EpisodeEnum>()
                .AddSingleton<ISchema, StarWarsSchema>();

            var provider = services.BuildServiceProvider();
            var swss = provider.GetService<StarWarsSchema>();

            var q = @"{ human(id: ""1000"") { name height} }";

            var result = swss.Execute(x =>
            {
                x.Query = q;
            });

            Console.WriteLine($"Result = {result}");
        }
    }
}
