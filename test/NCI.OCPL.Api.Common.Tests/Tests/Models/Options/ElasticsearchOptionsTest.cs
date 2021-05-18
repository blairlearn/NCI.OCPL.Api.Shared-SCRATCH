using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NCI.OCPL.Api.Common.Models.Options
{
  /// <summary>
  /// Elasticsearch configuration tests
  /// </summary>
  public partial class ElasticsearchOptionsTest
  {
    [Fact]
    public void Serialize()
    {
      // Use JToken for comparisons because we don't care about the order.
      JToken expected = JToken.Parse(@"
{
  ""Servers"": ""Server list"",
  ""Userid"": ""the user id"",
  ""Password"": ""secret password"",
  ""MaximumRetries"": 3
}");

      ElasticsearchOptions options = new ElasticsearchOptions()
      {
        Servers = "Server list",
        Userid = "the user id",
        Password = "secret password",
        MaximumRetries = 3
      };

      string actualText = JsonConvert.SerializeObject(options);
      JToken actualObject = JToken.Parse(actualText);

      Assert.Equal(expected, actualObject, new JTokenEqualityComparer());
    }

    [Fact]
    public void Deserialize()
    {
      string expectedServers = "Server list";
      string expectedUserid = "the user id";
      string expectedPassword = "secret password";
      int expectedMaximumRetries = 5;

      string input = @"
{
  ""Servers"": ""Server list"",
  ""Userid"": ""the user id"",
  ""Password"": ""secret password"",
  ""MaximumRetries"": 5
}";

      ElasticsearchOptions actual = JsonConvert.DeserializeObject<ElasticsearchOptions>(input);

      Assert.Equal(expectedServers, actual.Servers);
      Assert.Equal(expectedUserid, actual.Userid);
      Assert.Equal(expectedPassword, actual.Password);
      Assert.Equal(expectedMaximumRetries, actual.MaximumRetries);
    }
  }
}