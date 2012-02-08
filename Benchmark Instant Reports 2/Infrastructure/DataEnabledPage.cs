using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Infrastructure
{
    /// <summary>
    /// base class used by all report web pages; contains access 
    /// to data repositories via the IRepoService member that
    /// is injected by StructureMap
    /// </summary>
    public abstract class DataEnabledPage : System.Web.UI.Page
    {
        // will be injected by StructureMap setter injection
        public IRepoService DataService { get; set; }
    }
}