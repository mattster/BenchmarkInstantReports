using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;
using System.Web.UI.WebControls;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class UIHelper
    {
        #region DropDownLists
        /// <summary>
        /// determines if the specified string is a separator value
        /// used in DropDownLists
        /// </summary>
        /// <param name="selection">string to test</param>
        /// <returns>true if it is the separator value, false otherwise</returns>
        public static bool isDDSeparatorValue(string selection)
        {
            if (selection == Constants.DropDownSeparatorString)
                return true;
            else
                return false;
        }

        /// <summary>
        /// returns the index of the specified item in the specified DropDownList
        /// </summary>
        /// <param name="itemName">text of the item to find</param>
        /// <param name="ddl">the DropDownList to search</param>
        /// <returns>index of the found item, or -1 if not found</returns>
        public static int getIndexOfDDItem(string itemName, DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
                if (ddl.Items[i].Text.Trim() == itemName)
                    return i;

            return -1;
        }

        /// <summary>
        /// toggles whether there is an initial blank line in a DropDownList
        /// </summary>
        /// <param name="ddl">the DropDownList to toggle</param>
        /// <param name="initial">true -> put a blank in the first row; false -> remove blank in first row</param>
        public static void toggleDDLInitView(DropDownList ddl, bool initial)
        {
            if (ddl.Items.Count > 0)                    // make sure it has something in it
            {
                //
                if (initial)                            // put in blank 1st row, select it
                {
                    if (ddl.Items[0].Value != "-1")     // there is not a blank first row
                        ddl.Items.Insert(0, new ListItem(" ", "-1"));
                    ddl.SelectedIndex = 0;
                }
                else                                    // remove blank 1st row if necessary
                {
                    if (ddl.Items[0].Value == "-1")     // there is a blank 1st row
                        ddl.Items.RemoveAt(0);
                }
            }
            return;
        }

        #endregion


        #region ListBoxes
        /// <summary>
        /// gets a string array of selected items in a ListBox
        /// </summary>
        /// <param name="lb">the ListBox to use</param>
        /// <returns>string array of the text of the selected items</returns>
        public static string[] getLBSelectionsAsArray(System.Web.UI.WebControls.ListBox lb)
        {
            string[] returnstring = new string[lb.GetSelectedIndices().Count()];
            int[] indices = lb.GetSelectedIndices();

            for (int i = 0; i < indices.Length; i++)
            {
                returnstring[i] = lb.Items[indices[i]].ToString();
            }

            return returnstring;
        }


        public static void selectAllInLB(ListBox lb)
        {
            if (lb.Items.Count > 0)
            {
                foreach (ListItem item in lb.Items)
                {
                    item.Selected = true;
                }
            }

            return;
        }


        /// <summary>
        /// selects specific items in a ListBox
        /// </summary>
        /// <param name="lb">the ListBox in which to select the items</param>
        /// <param name="items">string array of item text values to select</param>
        public static void selectItemsInLB(ListBox lb, string[] items)
        {
            int[] indices = getIndicesOfLBItems(items, lb);
            if (indices != null)
                selectItemsInLB(lb, indices);

            return;
        }

        
        private static int[] getIndicesOfLBItems(string[] itemNames, ListBox lb)
        {
            List<int> indicesList = new List<int>();
            for (int i = 0; i < lb.Items.Count; i++)
                //if (isStringInStringArray(lb.Items[i].Text, itemNames))
                if (itemNames.Contains<string>(lb.Items[i].Text))
                    indicesList.Add(i);

            if (indicesList.Count > 0)
                return indicesList.ToArray();
            else
                return null;
        }


        private static void selectItemsInLB(ListBox lb, int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
                lb.Items[indices[i]].Selected = true;

            return;
        }
        #endregion

    }
}