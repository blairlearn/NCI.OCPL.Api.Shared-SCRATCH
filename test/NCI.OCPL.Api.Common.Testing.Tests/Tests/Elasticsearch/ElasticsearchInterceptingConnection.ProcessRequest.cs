using System;
using System.IO;
using System.Text;

using Elasticsearch.Net;
using Moq;
using Moq.Protected;
using Nest;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using static NCI.OCPL.Api.Common.Testing.ElasticsearchInterceptingConnection;
using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.Common
{

  /// <summary>
  /// This is dirty. Normally we'd call one of the public methods which calls ProcessRequest(),
  /// but the NEST ResponseBuilder class is static, meaning there's no way to pass in a mock and
  /// use that. On top of that, a ridiculous amount of mocking is required to support instantiating
  /// a RequestData object (another class which can't be mocked) that won't make ResponseBuilder
  /// blow up. So, if we want to test ProcessRequest(), we have to make it protected and then create
  /// a test class for purposes of actually calling the thing.
  /// </summary>
  class DirtyTestClass : ElasticsearchInterceptingConnection
  {
    public void DoProcessing<TReturn>(RequestData requestData, ResponseData responseData) where TReturn : class
    {
      ProcessRequest<TReturn>(requestData, responseData);
    }
  }

  public partial class ElasticsearchInterceptingConnectionTest
  {
    /// <summary>
    /// What happens when there are no handlers registered?
    /// </summary>
    [Fact]
    public void ProcessRequest_NoHandlerRegistered()
    {

      DirtyTestClass conn = new DirtyTestClass();

      // The specific value here shouldn't matter.
      RequestData req = RequestPlaceholder;
      ResponseData res = new ResponseData();

      Assert.Throws<ArgumentOutOfRangeException>(
        () => conn.DoProcessing<Nest.SearchResponse<MockType>>(req, res)
      );
    }

    /// <summary>
    /// What happens when the only handler is for the wrong type and there's no default?
    /// </summary>
    [Fact]
    public void ProcessRequest_NoDefault_NoMatch()
    {
      DirtyTestClass conn = new DirtyTestClass();
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>((reqd, resd) => { });

      // The specific value here shouldn't matter.
      RequestData req = RequestPlaceholder;
      ResponseData res = new ResponseData();

      Assert.Throws<ArgumentOutOfRangeException>(
        () => conn.DoProcessing<Nest.SearchResponse<MockType2>>(req, res)
      );
    }

    /// <summary>
    /// Simulate an error during a response.
    /// </summary>
    [Fact]
    public void ProcessRequest_ResponseError()
    {
      DirtyTestClass conn = new DirtyTestClass();
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>((reqd, resd) => {
        throw new NotImplementedException();
      });

      // The specific value here shouldn't matter.
      RequestData req = RequestPlaceholder;
      ResponseData res = new ResponseData();

      // What matters here is that
      // a. The exception was thrown.
      // b. It's the same one.
      // Possibly b. doesn't matter as much, but if that asssumption changes,
      // it should be a deliberate change.
      Assert.Throws<NotImplementedException>(
        () => conn.DoProcessing<Nest.SearchResponse<MockType>>(req, res)
      );
    }

    /// <summary>
    /// Match exists for the document type, no default registered.
    /// </summary>
    /// <value></value>
    [Fact]
    public void ProcessRequest_NoDefault_MatchExists()
    {
      // How many times was the handler called?
      int timesHandlerWasCalled = 0;

      DirtyTestClass conn = new DirtyTestClass();
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>((reqd, resd) => {
        resd.Stream = ElastcsearchTestingTools.MockEmptyResponse;
        resd.StatusCode = 200;
        timesHandlerWasCalled++;
      });

      // The specific value here shouldn't matter.
      RequestData req = RequestPlaceholder;
      ResponseData res = new ResponseData();

      conn.DoProcessing<Nest.SearchResponse<MockType>>(req, res);

      Assert.Equal(1, timesHandlerWasCalled);
      Assert.Equal(200, res.StatusCode);

      JToken actual;
      using (StreamReader sr = new StreamReader(res.Stream))
      {
        actual = JToken.Parse(sr.ReadToEnd());
      }

      JToken expected = JToken.Parse(ElastcsearchTestingTools.MockEmptyResponseString);

      Assert.Equal(expected, actual, new JTokenEqualityComparer());
    }


    /// <summary>
    /// Multiple handlers registered, no default.
    /// </summary>
    [Fact]
    public void ProcessRequest_NoDefault_MultipleHandlers()
    {
      // How many times was the handler called?
      int timesHandlerWasCalled = 0;

      DirtyTestClass conn = new DirtyTestClass();
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>((reqd, resd) =>
      {
        resd.Stream = ElastcsearchTestingTools.MockEmptyResponse;
        resd.StatusCode = 200;
        timesHandlerWasCalled++;
      });
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType2>>((reqd, resd) =>
      {
        throw new NotImplementedException();
      });

      // The specific value here shouldn't matter.
      RequestData req = RequestPlaceholder;
      ResponseData res = new ResponseData();

      conn.DoProcessing<Nest.SearchResponse<MockType>>(req, res);

      Assert.Equal(1, timesHandlerWasCalled);
      Assert.Equal(200, res.StatusCode);

      JToken actual;
      using (StreamReader sr = new StreamReader(res.Stream))
      {
        actual = JToken.Parse(sr.ReadToEnd());
      }

      JToken expected = JToken.Parse(ElastcsearchTestingTools.MockEmptyResponseString);

      Assert.Equal(expected, actual, new JTokenEqualityComparer());
    }

    /// <summary>
    /// Take the default handler when there's no match.
    /// </summary>
    [Fact]
    public void ProcessRequest_UseDefault()
    {
      // How many times was the handler called?
      int timesHandlerWasCalled = 0;

      DirtyTestClass conn = new DirtyTestClass();
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>((reqd, resd) =>
      {
        throw new NotImplementedException();
      });
      conn.RegisterDefaultHandler((RequestData reqd, Object resd) =>
      {
        ((ResponseData)resd).Stream = ElastcsearchTestingTools.MockEmptyResponse;
        ((ResponseData)resd).StatusCode = 200;
        timesHandlerWasCalled++;
      });

      // The specific value here shouldn't matter.
      RequestData req = RequestPlaceholder;
      ResponseData res = new ResponseData();

      // MockType is registered, but we're looking for MockType2.
      conn.DoProcessing<Nest.SearchResponse<MockType2>>(req, res);

      Assert.Equal(1, timesHandlerWasCalled);
      Assert.Equal(200, res.StatusCode);

      JToken actual;
      using (StreamReader sr = new StreamReader(res.Stream))
      {
        actual = JToken.Parse(sr.ReadToEnd());
      }

      JToken expected = JToken.Parse(ElastcsearchTestingTools.MockEmptyResponseString);

      Assert.Equal(expected, actual, new JTokenEqualityComparer());
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

        Mock<IConnectionConfigurationValues> mockGlobalConfig = new Mock<IConnectionConfigurationValues>();
        mockGlobalConfig.Setup(cfg => cfg.RequestTimeout).Returns(new TimeSpan(0, 0, 5));
        mockGlobalConfig.Setup(cfg => cfg.PingTimeout).Returns(new TimeSpan(0, 0, 5));
        mockGlobalConfig.Setup(cfg => cfg.DisableDirectStreaming).Returns(true);

        Mock<IRequestConfiguration> mockRequestConfig = new Mock<IRequestConfiguration>();
        mockRequestConfig.Setup(cfg => cfg.AllowedStatusCodes).Returns(new int[]{200});

        Mock<IRequestParameters> mockLocalConfig = new Mock<IRequestParameters>();
        mockLocalConfig.Setup(cfg => cfg.RequestConfiguration).Returns(mockRequestConfig.Object);


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

        return new RequestData(HttpMethod.GET, "foo", mockData.Object, mockGlobalConfig.Object, mockLocalConfig.Object, null);
      }
    }

  }
}