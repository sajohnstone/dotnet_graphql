using System;
using Xunit;
using App;
using GraphQL;
using Newtonsoft.Json.Linq;

namespace Tests
{
    public class SchemaTests
    {
        public SchemaTests()
        {
        }

        [Theory]
        [InlineData("Hello World!", @"{""data"": { ""hello"": ""Hello World!""} }")]
        [InlineData("Hello Dave!", @"{""data"": { ""hello"": ""Hello Dave!""} }")]
        public void ValidSchema(object testString, string result)
        {
            JToken expected = JToken.Parse(result);

            var root = new { Hello = testString };
            string json = Program.MySchema.Execute(x =>
            {
                x.Query = "{ hello }";
                x.Root = root;
            });

            JToken actual = JToken.Parse(json);
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
