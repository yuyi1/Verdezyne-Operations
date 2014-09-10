using System.Collections.Generic;
using System.Text;
using NPOI.XWPF.UserModel;

namespace OfficeHandler.Word
{
    public static class CellText
    {
        public static string GetCellText(XWPFTableCell cel)
        {
            StringBuilder sb = new StringBuilder();
            IList<XWPFParagraph> parasList = cel.Paragraphs;
            foreach (XWPFParagraph para in parasList)
            {
                sb.Append(para.ParagraphText);
            }
            return sb.ToString();
        }
    }
}