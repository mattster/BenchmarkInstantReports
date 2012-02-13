
namespace Benchmark_Instant_Reports_2.Infrastructure.Entities
{
    public class School
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Principal { get; set; }
        public string Area { get; set; }
        public int Loc { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Abbr { get; set; }
        public int Cluster { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public School()
        {
            ID = 0;
            Name = "";
            Password = "";
            Principal = "";
            Area = "";
            Loc = 0;
            Phone = "";
            Username = "";
            Abbr = "";
            Cluster = 0;
        }


        /// <summary>
        /// Constructor for a temporary school with just a name and abbreviation
        /// </summary>
        /// <param name="abbr">Abbreviation for this school</param>
        /// <param name="name">Name for this school</param>
        public School(string abbr, string name)
        {
            ID = 0;
            Name = name;
            Password = "";
            Principal = "";
            Area = "";
            Loc = 0;
            Phone = "";
            Username = "";
            Abbr = abbr;
            Cluster = 0;
        }
    }
}