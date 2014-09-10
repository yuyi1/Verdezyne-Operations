using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.DTOs;

namespace Operations.RepositoryHelpers
{

    /// <summary>
    /// Consider moving this class to the filters project and adding a Operations.method.
    /// </summary>
    public static class DecimalComparisonFilter
    {
        /// <summary>
        /// Compares the value to the standard for each area and sets the comparison constant for the cell
        /// </summary>
        /// <param name="areaInput">decimal = The area value</param>
        /// <param name="colindex">int - the column in the array being Operations.d</param>
        /// <param name="stdaArray">decimal [,] - the array of standards</param>
        /// <returns>decimal - the comparison value from Operations.Dtos.ViewConstants</returns>

        public static decimal Filter(decimal? areaInput, int colindex, ref decimal[,] stdaArray)
        {
            decimal min = 0;
            decimal max = 0;

            // NOTE: Iron has 11 columns
            int columns = stdaArray.GetUpperBound(1);  // the number of columns ([,] arrays are 0 for rows, and 1 for columns
            int half = columns / 2;
            if (colindex > half)
                colindex -= half + 1;

            // Get the min and max standard for the area
            min = int.MaxValue;
            for (int i = 0; i < stdaArray.GetUpperBound(0) + 1; i++)
                if (stdaArray[i, colindex] < min)
                {
                    min = stdaArray[i, colindex];
                }
            max = 0;
            for (int i = 0; i < stdaArray.GetUpperBound(0) + 1; i++)
                if (stdaArray[i, colindex] >= max)
                {
                    max = stdaArray[i, colindex];
                }

            // Apply the standard to the area 
            var input = areaInput ?? 0;
            if (input == 0)
                return ViewConstants.EqualsZero;

            if (input < 0)
            {
                return ViewConstants.LessThanZero;
            }
            if (input < min)
            {
                return ViewConstants.LessThanMin;
            }
            if (input > max)
            {
                return ViewConstants.GreaterThanMax;
            }
            return ViewConstants.InRange;


        }
    }
}