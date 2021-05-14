using System;
using Elasticsearch.Net;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using System.IO;
using Moq;
using System.Text;

namespace NCI.OCPL.Api.Common
{
  public partial class ElasticsearchInterceptingConnectionTest
  {
    /// <summary>
    /// What happens when there are no handlers registered?
    /// </summary>
    [Fact]
    public void Request_NoHandlerRegistered()
    {
      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();

      // The specific value here shouldn't matter.
      RequestData rd = RequestPlaceholder;

      Assert.Throws<ArgumentOutOfRangeException>(
        () => ((IConnection)conn).Request<Nest.SearchResponse<MockType>>(rd)
      );
    }

    /// <summary>
    /// What happens when the only handler is for the wrong type and there's no default?
    /// </summary>
    [Fact]
    public void Request_NoDefault_NoMatch()
    {
      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
      conn.RegisterRequestHandlerForType<MockType>((req, res) => { });

      // The specific value here shouldn't matter.
      RequestData rd = RequestPlaceholder;

      Assert.Throws<ArgumentOutOfRangeException>(
        () => ((IConnection)conn).Request<Nest.SearchResponse<MockType2>>(rd)
      );
    }

    /// <summary>
    /// Simulate an error during a response.
    /// </summary>
    [Fact]
    public void Request_ResponseError()
    {
      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>((req, res) => {
        throw new NotImplementedException();
      });

      // The specific value here shouldn't matter.
      RequestData rd = RequestPlaceholder;

      // What matters here is that
      // a. The exception was thrown.
      // b. It's the same one.
      // Possibly b. doesn't matter as much, but if that asssumption changes,
      // it should be a deliberate change.
      Assert.Throws<NotImplementedException>(
        () => ((IConnection)conn).Request<Nest.SearchResponse<MockType>>(rd)
      );
    }


    /// <summary>
    /// Instantiate the mock RequestData needed to satisfy the NEST internals called by the
    /// Request methods.
    /// </summary>
    /// <value>A instance of RequestData.</value>
    private RequestData RequestPlaceholder
    {
      get
      {
        string data = @"{ ""string_property"": ""string value"", ""bool_property"": true, ""integer_property"": 19, ""null"": null }";

        Mock<IConnectionConfigurationValues> mockConfig = new Mock<IConnectionConfigurationValues>();
        mockConfig.Setup(cfg => cfg.RequestTimeout).Returns(new TimeSpan(0, 0, 5));
        mockConfig.Setup(cfg => cfg.PingTimeout).Returns(new TimeSpan(0, 0, 5));

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

        return new RequestData(HttpMethod.GET, "foo", mockData.Object, mockConfig.Object, null, null);
      }
    }

  }
}