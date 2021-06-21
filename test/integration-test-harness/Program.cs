using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NCI.OCPL.Api.Common;

namespace integration_test_harness
{
    public class Program : NciApiProgramBase
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder<Startup>(args).Build().Run();
        }
    }
}
