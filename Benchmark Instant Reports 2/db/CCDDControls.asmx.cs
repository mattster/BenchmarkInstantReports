using System.Collections.Generic;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;

namespace Benchmark_Instant_Reports_2.db
{
    /// <summary>
    /// Summary description for CCDDControls.cs
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    
    [ScriptService()]
    public class CascadingDropDown : System.Web.Services.WebService 
    {

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
