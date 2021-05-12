using System;

using Moq;
using Nest;

namespace NCI.OCPL.Api.Common
{
  public partial class ElasticsearchInterceptingConnectionTest
  {
    public class MockType : Mock<Object> { }
    public class MockSubtype : MockType { }
    public class MockType2 : Mock<Object> { }
  }
}