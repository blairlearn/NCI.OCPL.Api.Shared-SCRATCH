using Nest;
using Xunit;

namespace NCI.OCPL.Api.Common.Testing
{
#pragma warning disable CS0618
  [ElasticsearchType(Name = "terms")]
#pragma warning restore CS0618
  public class TestType
  {
    /// <summary>
    /// The Backend ID for this item
    /// </summary>
    /// <returns></returns>
    [Text(Name = "term")]
    public string Term { get; set; }

  }

  public partial class ElasticToolsTest
  {
    [Fact]
    public void GetInMemoryElasticClient()
    {
      IElasticClient client = ElasticTools.GetInMemoryElasticClient("elastic-tools-get-in-memory-client.json");
      var response = client.SearchTemplate<TestType>(sd => sd
                .Index("AliasName")
                .Params(pd => pd
                    .Add("searchstring", "search_term")
                    .Add("my_size", 10)
                )
            );

      Assert.True(response.IsValid);
      Assert.Equal(222, response.Total);
      Assert.Equal(20, response.Documents.Count);
      Assert.All(response.Documents, doc => Assert.NotNull(doc));
      Assert.All(response.Documents, doc => Assert.IsType<TestType>(doc));
    }
  }
}