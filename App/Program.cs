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
        static void Main(string[] args)
        {
            //setup our DI
            IServiceCollection services = new ServiceCollection()
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

            //setup the schema
            ServiceProvider provider = services.BuildServiceProvider();
            StarWarsSchema swss = provider.GetRequiredService<StarWarsSchema>();

            //do the GUI stuff
            Console.WriteLine("Welcome to the Star Wars GraphQL Implementation");
            Console.WriteLine("Press CTRL C or type EXIT to exit");
            var q = "";
            while (q.ToUpper() != "EXIT")
            {
                Console.WriteLine("Please enter a valid query:");
                q = Console.ReadLine();
                var result = swss.Execute(x =>
                {
                    x.Query = q;
                });
                Console.WriteLine($"Result = {result}");
            }
        }
    }
}
