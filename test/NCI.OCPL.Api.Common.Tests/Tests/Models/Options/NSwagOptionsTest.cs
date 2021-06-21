using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NCI.OCPL.Api.Common.Models.Options
{
  /// <summary>
  /// NSwag configuration tests
  /// </summary>
  public partial class NSwagOptionsTest
  {
    [Fact]
    public void Serialize()
    {
      // Use JToken for comparisons because we don't care about the order.
      JToken expected = JToken.Parse(@"
{
  ""Title"": ""Swagger Doc Title"",
  ""Description"": ""API Description""
}");

      NSwagOptions options = new NSwagOptions()
      {
        Title = "Swagger Doc Title",
        Description = "API Description"
      };

      string actualText = JsonConvert.SerializeObject(options);
      JToken actualObject = JToken.Parse(actualText);

      Assert.Equal(expected, actualObject, new JTokenEqualityComparer());
    }

    [Fact]
    public void Deserialize()
    {
      string expectedTitle = "Swagger Doc Title";
      string expectedDescription = "API Description";

      string input = @"
{
  ""Title"": ""Swagger Doc Title"",
  ""Description"": ""API Description""
}";

      NSwagOptions actual = JsonConvert.DeserializeObject<NSwagOptions>(input);

      Assert.Equal(expectedTitle, actual.Title);
      Assert.Equal(expectedDescription, actual.Description);
    }

  }
}