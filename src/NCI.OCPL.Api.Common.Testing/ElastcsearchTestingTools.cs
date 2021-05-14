using System.IO;
using System.Text;

namespace NCI.OCPL.Api.Common.Testing
{
  /// <summary>
  /// Helper tools for loading responses from files and
  /// deserializing XML to objects.
  /// </summary>
  public static class ElastcsearchTestingTools
  {
    static public Stream MockEmptyResponse
    {
      get
      {
        string empty = @"
{
    ""took"": 223,
    ""timed_out"": false,
    ""_shards"": {
        ""total"": 1,
        ""successful"": 1,
        ""skipped"": 0,
        ""failed"": 0
    },
    ""hits"": {
        ""total"": 0,
        ""max_score"": null,
        ""hits"": []
    }
}";
        byte[] byteArray = Encoding.UTF8.GetBytes(empty);
        return new MemoryStream(byteArray);
      }
    }

  }
}