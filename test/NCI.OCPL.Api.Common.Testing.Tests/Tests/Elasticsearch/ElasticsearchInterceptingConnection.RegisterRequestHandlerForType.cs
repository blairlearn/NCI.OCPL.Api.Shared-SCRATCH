using System;

using Elasticsearch.Net;

using NCI.OCPL.Api.Common.Testing;
using Xunit;

namespace NCI.OCPL.Api.Common
{
  /// <summary>
  /// RegisterRequestHandlerForType tests.
  /// </summary>
  public partial class ElasticsearchInterceptingConnectionTest
  {
    /// <summary>
    /// Multiple handlers can't be registered for the same type.
    /// </summary>
    [Fact]
    public void RegisterRequestHandlerForType_DuplicateTypes()
    {
      void callback(RequestData req, ElasticsearchInterceptingConnection.ResponseData res) { }
      void callback2(RequestData req, ElasticsearchInterceptingConnection.ResponseData res) { }

      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();

      // First registration should always succeed.
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>(callback);

      // Second registration for same type should always succeed.
      Exception ex = Assert.Throws<ArgumentException>(
        () => conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>(callback)
      );

      Assert.Equal(
        "There is already a handler defined that would be called for type. Trying to add for: Nest.SearchResponse`1[NCI.OCPL.Api.Common.ElasticsearchInterceptingConnectionTest+MockType], Already Existing: Nest.SearchResponse`1[NCI.OCPL.Api.Common.ElasticsearchInterceptingConnectionTest+MockType]",
        ex.Message
      );

      // Second registration for the same type should still fail with a different handler.
      ex = Assert.Throws<ArgumentException>(
        () => conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>(callback2)
      );
    }


    /// <summary>
    /// Registering handlers for completely different types does work.
    /// </summary>
    [Fact]
    public void RegisterRequestHandlerForType_DifferentTypes()
    {
      void callback(RequestData req, ElasticsearchInterceptingConnection.ResponseData res) { }
      ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();

      // This registration should always succeed.
      conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType>>(callback);

      // This registration should also succeed.
      Exception ex = Record.Exception(
        () => conn.RegisterRequestHandlerForType<Nest.SearchResponse<MockType2>>(callback)
      );

      Assert.Null(ex);
    }
  }
}