using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using Operations.Models;


namespace Operations.Utility
{
    /// <summary>
    /// Filters a List<RotatedReadings></RotatedReadings> 
    /// </summary>
    public class ColumnNameFilter
    {
        public bool FilterForNoData(ref List<RotatedReading> rotatedReadings, ref List<string> filteredColumnList)
        {
            
            try
            {
                foreach (var item in rotatedReadings)
                {
                    var local = item;
                    foreach (PropertyInfo pi in item.GetType().GetProperties())
                    {
                        var value = pi.GetValue(local, null);
                        if (value != null)
                        {
                            if ((value.ToString().Length > 0) &&
                                (!filteredColumnList.Contains(pi.Name) && (!pi.Name.Contains("Id")) &&
                                 (!pi.Name.Contains("Name")) && (!pi.Name.Contains("Run_Date")) &&
                                 (!pi.Name.Contains("LastUpdate")) && (!pi.Name.Contains("UpdateBy"))))
                            {
                                filteredColumnList.Add(pi.Name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ColumnNameFilter Exception: " + ex.StackTrace);
            }

            return true;
        }
    }
}