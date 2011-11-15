using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AjaxControlToolkit;
using System.Data;

namespace Benchmark_Instant_Reports_2
{
    /// <summary>
    /// Summary description for CascadingDropDown1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CascadingDropDown1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetCampusList(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> returnList = new List<CascadingDropDownNameValue>();
            DataSet ds = dbIFOracle.getDataRows(birIF.getCampusListQuery);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                returnList.Add(new CascadingDropDownNameValue(row["SCHOOLNAME"].ToString(), row["SCHOOL_ABBR"].ToString()));
            }

            return returnList.ToArray();
        }
    }
}
