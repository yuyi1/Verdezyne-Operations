using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilterPipeline.Filters;

namespace FilterPipeline.Pipelines
{
    public static class WordTableColumnName
    {
        public static List<object> FilterColumnList(List<object> columnList)
        {
            List<object> filteredColumnList = new List<object>();
            for (int index = 0; index < columnList.Count; index++)
            {
                // Filter out unwanted characters
                var input = columnList[index].ToString();
                var pipeline = new Pipeline<string>();
                var result = pipeline.Register(new SpaceFilter())
                    .Register(new CrlfFilter())
                    .Register(new NH4Filter())
                    .Execute(input);
                filteredColumnList.Add((object)result);
            }
            return filteredColumnList;            
        }
    }
}
