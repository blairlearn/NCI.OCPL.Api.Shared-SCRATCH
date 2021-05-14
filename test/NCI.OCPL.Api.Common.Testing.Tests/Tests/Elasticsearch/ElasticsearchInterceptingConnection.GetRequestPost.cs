using System;
using System.IO;

using Elasticsearch.Net;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using Moq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NCI.OCPL.Api.Common
{
  /// <summary>
  /// GetRequestPost tests
  /// </summary>
  public partial class ElasticsearchInterceptingConnectionTest
  {
    /// <summary>
    /// Fail to set up multiple default handlers.
    /// </summary>
    [Fact]
    public void GetRequestPost_NullPostData()
    {
      Mock<IConnectionConfigurationValues> mockConfig = new Mock<IConnectionConfigurationValues>();
      mockConfig.Setup(cfg => cfg.RequestTimeout).Returns(new TimeSpan(0, 0, 5));
      mockConfig.Setup(cfg => cfg.PingTimeout).Returns(new TimeSpan(0, 0, 5));
      IConnectionConfigurationValues config = mockConfig.Object;

      // None of these values really matter except the post data which is the third parameter.
      RequestData requestData = new RequestData(HttpMethod.GET, "foo", null, config, null, null);

      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();

      Assert.Null(conn.GetRequestPost(requestData));
    }

    [Theory]
    //[InlineData(@"{ ""string_property"": ""string value"", ""bool_property"": true, ""integer_property"": 19, ""null"": null }")]
    [InlineData(@"[""array value 1"", ""array value 2"", ""array value 3""]")]
    public void GetRequestPost_WithData(string data)
    {
      JToken expected = JToken.Parse(data);

      Mock<IConnectionConfigurationValues> mockConfig = new Mock<IConnectionConfigurationValues>();
      mockConfig.Setup(cfg => cfg.RequestTimeout).Returns(new TimeSpan(0, 0, 5));
      mockConfig.Setup(cfg => cfg.PingTimeout).Returns(new TimeSpan(0, 0, 5));
      IConnectionConfigurationValues config = mockConfig.Object;

      Mock<PostData> mockData = new Mock<PostData>();
      mockData.Setup(
        d => d.Write(It.IsAny<Stream>(), It.IsAny<IConnectionConfigurationValues>())
      )
      .Callback((
        Stream str, IConnectionConfigurationValues iccv) =>
        {
          byte[] buf = Encoding.UTF8.GetBytes(data);
          str.Write(buf, 0, buf.Length);
        }
      );


      // None of these values really matter except the post data which is the third parameter.
      RequestData requestData = new RequestData(HttpMethod.GET, "foo", mockData.Object, config, null, null);

      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();

      JToken actual = conn.GetRequestPost(requestData);

      Assert.Equal(expected, actual, new JTokenEqualityComparer());
    }

  }
}