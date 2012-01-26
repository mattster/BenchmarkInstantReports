using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Infrastructure.Repositories;

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
                    x.For<IRepoService>().Use<RepoService>();
                    
                    // School Repo - use a Singleton since this data will not change; use Oracle Data Adapter repo
                    x.For<ISchoolRepository>().Singleton().Use<SchoolRepositoryODA>();


                    x.SetAllProperties(set => set.OfType<IRepoService>());
                });
        }
    }
}