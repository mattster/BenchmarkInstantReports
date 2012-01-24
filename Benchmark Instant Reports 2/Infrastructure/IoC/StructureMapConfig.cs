using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Infrastructure.IoC
{
    public class StructureMapConfig
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.Scan(scan =>
                        {
                            scan.TheCallingAssembly();
                            scan.WithDefaultConventions();
                        });
                    //x.ForRequestedType<ISchoolRepository>().Use
                });

        }
    }
}