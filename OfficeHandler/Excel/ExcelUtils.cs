using System;

namespace OfficeHandler.Excel
{
    internal class ExcelUtils
    {
        /// <summary>
        /// Obtiene al número de índice de una columna Excel en el formato A,B,C, ... ZZZ, ...
        /// en base a índice 1
        /// </summary>
        /// <param name="column">Columna Excel de la forma A, B, C, ...</param>
        /// <returns>El índice en base 1</returns>
        public static int GetColumnIndex(string column)
        {
            column = column.ToUpper();
            // cada número es dig*26^pos + dig*26^pos-1 + dig*26^pos-2 + ... + dig*26^0
            int index = 0;

            for (var i = 0; i < column.Length; i++)
            {
                var ch = column[i];
                var value = (byte)ch - (byte)'A' + 1;
                var tempval = value * (int)Math.Pow(26, column.Length - i - 1);
                index += tempval;
            }

            return index;
        }
    }
}
