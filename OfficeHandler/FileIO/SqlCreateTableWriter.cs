using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OfficeHandler.FileIO
{
    public class SqlCreateTableWriter
    {
        public void WriteTableCreateFromList(List<object> list, string sqlfile, string tablename)
        {
            var sb = new StringBuilder();
            sb.AppendLine("USE [Analytics]");
            sb.AppendLine("GO");
            sb.AppendLine("");
            sb.Append("/****** Object:  Table [dbo].[");
            sb.Append(tablename);
            sb.AppendLine("]    Script) Date: 08/12/2014 14:17:10 ******/");
            sb.Append("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[");
            sb.Append(tablename);
            sb.AppendLine("]') AND type in (N'U'))");
            sb.Append("DROP TABLE [dbo].[");
            sb.Append(tablename);
            sb.AppendLine("]");
            sb.AppendLine("GO");
            sb.AppendLine("");
            sb.AppendLine(string.Format("CREATE TABLE [dbo].[{0}] (\n     [ID] [int] IDENTITY(1,1) NOT NULL,\n     [CalibrationDate] [DateTime] NULL,", tablename));

            sb.Append("     [");
            sb.Append(list[0]);
            sb.AppendLine("] varchar(150) null,");

            for (int index = 1; index < list.Count; index++)
            {
                sb.Append("     [");
                sb.Append(list[index]);
                sb.AppendLine("] money null,");
                if (index == list.Count - 1)
                {
                    sb.AppendLine(string.Format("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ", tablename));
                    sb.AppendLine("(");
                    sb.AppendLine("	[ID] ASC");
                    sb.AppendLine(
                        ") WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
                    sb.AppendLine(") ON [PRIMARY]");
                }
            }

            
            if (File.Exists(sqlfile))
                File.Delete(sqlfile);
            using (Stream stream = new FileStream(sqlfile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {

                var sw = new StreamWriter(stream);
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                stream.Close();
            }
        }
    }
}
