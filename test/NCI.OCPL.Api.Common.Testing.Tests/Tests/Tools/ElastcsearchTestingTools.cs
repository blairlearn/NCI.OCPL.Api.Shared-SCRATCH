using System.IO;

using Newtonsoft.Json.Linq;
using Xunit;

namespace NCI.OCPL.Api.Common.Testing
{
  public partial class ElastcsearchTestingToolsTest
  {
    [Fact]
    public void EmptyResponse()
    {
      JToken expected = JToken.Parse(ElastcsearchTestingTools.MockEmptyResponseString);
      JToken actual;

      using(StreamReader sr = new StreamReader(ElastcsearchTestingTools.MockEmptyResponse))
      {
        actual = JToken.Parse(sr.ReadToEnd());
      }

      Assert.Equal(expected, actual, new JTokenEqualityComparer());
    }
  }
}