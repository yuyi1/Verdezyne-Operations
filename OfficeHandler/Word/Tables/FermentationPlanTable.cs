﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using OfficeHandler.Contracts;
using OfficeHandler.Excel.Factory;
using OfficeHandler.Word.Converters;

namespace OfficeHandler.Word.Tables
{
    public class FermentationPlanTable : IWordTable
    {
        public DataTable FindTable(string filepath)
        {
            using (FileStream stream = File.OpenRead(filepath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                foreach (XWPFTable table in doc.Tables)
                {
                    if (Row2Cell1Contains(table, @"Run") && Row1Cell1Contains(table, @"General"))
                    {
                        return WordTableToDataTable.GetDataTable(table);
                    }
                }
                return null;
            }
        }

        private void SetHeadings(ref XWPFDocument doc, XWPFTable table)
        {
            XWPFParagraph p1 = doc.CreateParagraph();
            XWPFRun r1 = p1.CreateRun();
            r1.SetBold(true);
            r1.SetText("Category");
            r1.SetBold(true);
            r1.SetFontFamily("Courier");
            table.GetRow(0).GetCell(0).SetParagraph(p1);

            XWPFParagraph p2 = doc.CreateParagraph();
            XWPFRun r2 = p2.CreateRun();
            r2.SetBold(true);
            r2.SetText("Category");
            r2.SetBold(true);
            r2.SetFontFamily("Courier");
            table.GetRow(0).GetCell(1).SetParagraph(p2);


        }
        private bool Row2Cell1Contains(XWPFTable table, string value)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Rows: {0}", table.Rows.Count));
                if (table.Rows.Count > 2)
                {
                    XWPFTableCell cel = table.Rows[2].GetCell(1);
                    {
                        string celtext = CellText.GetCellText(cel);
                        System.Diagnostics.Debug.WriteLine(string.Format("Row: 2, Cell: 1 - {0} ", celtext));
                        if (celtext.StartsWith(value))
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return false;
        }

        private bool Row1Cell1Contains(XWPFTable table, string value)
        {
            try
            {
                if (table.Rows.Count > 1)
                {
                    XWPFTableCell cel = table.Rows[1].GetCell(0);
                    {
                        string celtext = CellText.GetCellText(cel);
                        System.Diagnostics.Debug.WriteLine(string.Format("Row: 1, Cell: 0 - ", celtext));
                        if (celtext.StartsWith(value))
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return false;
        }


    }
}
