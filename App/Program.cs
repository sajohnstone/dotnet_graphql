using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarWars;
using StarWars.Types;
using System;
using System.Collections.Generic;

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
            var inputLine = "";
            while (inputLine.ToUpper() != "EXIT")
            {
                Console.WriteLine("Please enter a valid query:");
                inputLine = Console.ReadLine();

                var result = swss.Execute(_ =>
                {
                    //get the input into GraphQL format
                    GraphQL.Inputs queryInputs = inputLine.ToInputs();
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
                Console.WriteLine($"Result = {result}");
            }
        }

        
    }
}
