using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Infrastructure.Repositories;
using StructureMap.Pipeline;

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
                    
                    // School repo; scoped as Singleton since this data will not change; use Oracle Data Adapter repo
                    x.For<ISchoolRepository>().Singleton().Use<SchoolRepositoryODA>();

                    // Answer Key repos; scoped to the HTTP Context; use Oracle Data Adapter repo
                    x.For<IAnswerKeyRepository>().LifecycleIs(new HttpContextLifecycle()).Use<AnswerKeyRepositoryODA>();
                    x.For<IAnswerKeyCampusRepository>().LifecycleIs(new HttpContextLifecycle()).Use<AnswerKeyCampusRepositoryODA>();

                    // Roster repo; scoped as a Singleton; use Oracle Data Adapter rep
                    x.For<IRosterRepository>().Singleton().Use<RosterRepositoryODA>();

                    // Scan repo; scoped to the HTTP Context; use Oracle Data Adapter repo
                    x.For<IScanRepository>().LifecycleIs(new HttpContextLifecycle()).Use<ScanRepositoryODA>();

                    // Test Definition repo; scoped to the HTTP Context; use Oracle Data Adapter repo
                    x.For<ITestRepository>().LifecycleIs(new HttpContextLifecycle()).Use<TestRepositoryODA>();

                    x.SetAllProperties(set => set.OfType<IRepoService>());
                });
        }
    }
}