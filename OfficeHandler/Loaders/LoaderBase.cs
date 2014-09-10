using System;
using System.Data;
using System.IO;
using Excel;
using OfficeHandler.Contracts;
using OfficeHandler.Notifications;

namespace OfficeHandler.Loaders
{
    public class LoaderBase : ILoader
    {
        public event EventHandler<OfficeHandlerEventArgs> RaiseExceptionEvent;
        public event EventHandler<OfficeHandlerEventArgs> RaiseSuccessEvent;
        public event EventHandler<OfficeHandlerEventArgs> RaiseProgressEvent;

        
        public string ExcelFilePath { get; set; }
        private DataSet data = null;
        public bool isExcelXML { get; set; }

        public DataSet Data
        {
            get
            {
                if (data == null)
                {
                    GetSpreadsheetAsDataSet();
                }
                return data;
            }
            set { data = value; }
        }

        public LoaderBase()
        {

        }
        public LoaderBase(string filename)
        {
            ExcelFilePath = filename;
        }

        public LoaderBase LoadBase()
        {
            if (ExcelFilePath == null)
                throw new Exception("ExcelFilePath cannot be blank.");
            if (data == null)
                data = GetSpreadsheetAsDataSet();

            return this;
        }

        public virtual void Load()
        {
            if (ExcelFilePath == null)
                throw new Exception("ExcelFilePath cannot be blank.");
            if (data == null)
                data = GetSpreadsheetAsDataSet();
        }


        protected virtual void OnRaiseExceptionEvent(OfficeHandlerEventArgs e)
        {
            EventHandler<OfficeHandlerEventArgs> handler = RaiseExceptionEvent;
            if (handler != null)
            {
                e.Message += String.Format(" at {0}", DateTime.Now.ToString());
                handler(this, e);
            }
        }

        protected virtual void OnRaiseLoadedEvent(OfficeHandlerEventArgs e)
        {
            EventHandler<OfficeHandlerEventArgs> handler = RaiseSuccessEvent;
            if (handler != null)
            {
                e.Message += String.Format(" at {0}", DateTime.Now.ToString());
                handler(this, e);
            }
        }
        protected virtual void OnLoadProgressEvent(OfficeHandlerEventArgs e)
        {
            EventHandler<OfficeHandlerEventArgs> handler = RaiseProgressEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private DataSet GetSpreadsheetAsDataSet()
        {
            IExcelDataReader excelReader = null;
            FileInfo fi = new FileInfo(ExcelFilePath);
            switch (fi.Extension)
            {
                case ".xls":
                    isExcelXML = false;
                    excelReader = ExcelReaderFactory.CreateBinaryReader(GetWorkbookStream(ExcelFilePath));
                    break;
                case ".xlsx":
                    isExcelXML = true;
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(GetWorkbookStream(ExcelFilePath));
                    break;
                case ".xlsm":
                    isExcelXML = true;
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(GetWorkbookStream(ExcelFilePath));
                    break;
                default:
                    throw new Exception("Invalid file extension.  Must be .xls, .xlsx, or .xlsm");

            }
            DataSet result = excelReader.AsDataSet();

            excelReader.Close();
            excelReader.Dispose();
            return result;
        }

        #region Utility
        /// <summary>
        /// Gets a Workbook as a stream
        /// </summary>
        /// <param name="PersistedFileCheckList">string - the path to the excel file</param>
        /// <returns>Stream - A System.Stream to the workbook</returns>
        public static Stream GetWorkbookStream(string fileName)
        {
            System.Threading.Thread.Sleep(1000);
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.StackTrace);
            }
            return fs;
        }
        #endregion
    }
}
