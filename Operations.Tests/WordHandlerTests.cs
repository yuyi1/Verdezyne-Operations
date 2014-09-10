//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OfficeHandler;
//using OfficeHandler.Word;
//using OfficeHandler.Word.Converters;
//using OfficeHandler.Word.Tables;
//using Process.Controllers;
//using Process.Models;
//using Process.Repository;

//namespace Process.UnitTest
//{
//    [TestClass]
//    public class WordHandlerTests
//    {
//        #region "Basic tests"
//        [TestMethod]
//        public void ShouldCreateAndPopulateSimpleWordDoc()
//        {
//            // Arrange
//            const string modernfile = @"..\..\TestFiles\simple.docx";
//            var docx = new DocxWriter();

//            // Act
//            var retval = docx.SimpleDocument(modernfile);

//            //Assert
//            Assert.IsTrue(retval);
//            Assert.IsTrue(File.Exists(modernfile));

//            //"simpleTable.docx"
//        }
//        [TestMethod]
//        public void ShouldCreateAndPopulateSimpleTableWordDoc()
//        {
//            // Arrange
//            const string modernfile = @"..\..\TestFiles\simpleTable.docx";
//            var docx = new DocxWriter();

//            // Act
//            var retval = docx.SimpleTable(modernfile);

//            //Assert
//            Assert.IsTrue(retval);
//            Assert.IsTrue(File.Exists(modernfile));

//        }
//        [TestMethod]
//        public void ShouldReadSimpleWordDoc()
//        {
//            // Arrange
//            //const string legacyfile = @"..\..\new_file.xls";
//            const string modernfile = @"..\..\TestFiles\simple.docx";
//            var docx = new DocxReader();

//            // Act
//            var retval = docx.FermentationOutlineText(modernfile);

//            //Assert
//            Assert.IsTrue(retval);
//        }
//        [TestMethod]
//        public void ShouldReadTestWordDocText()
//        {
//            // Arrange
//            const string modernfile = @"..\..\TestFiles\140618_QA1-D3_Outline.docx";
//            var docx = new DocxReader();

//            // Act
//            var retval = docx.FermentationOutlineText(modernfile);

//            //Assert
//            Assert.IsTrue(retval);
//        }
//        #endregion
//        #region "Test Data Files"
//        [TestMethod]
//        public void ShouldReadSamplingDetailsTable()
//        {
//            // Arrange
////            const string filename = @"C:\\Users\\Public\\Documents\\Analytics\\Analytical Submissions_140506_300L\\140506_300L_Outline.docx";
//            const string filename = @"..\..\TestFiles\140506_300L_Outline.docx";

//            // Act
//            var reader = new SamplingDetailsTable();
//            var retval = reader.FindTable(filename);

//            //Assert
//            Assert.IsNotNull(retval);
//            Assert.IsTrue(retval.Rows.Count > 0);
//        }
//        [TestMethod]
//        public void ShouldReadFermentationOutlineTable()
//        {
//            // Arrange
//            const string filename = @"..\..\TestFiles\140506_300L_Outline.docx";

//            // Act
//            var reader = new FermentationOutlineTable();
//            var retval = reader.FindTable(filename);

//            //Assert
//            Assert.IsNotNull(retval);
//            Assert.IsTrue(retval.Rows.Count > 0);
//        }

//        [TestMethod]
//        public void UploadWordTest()
//        {
            
//            //OutlineItemsController cont = new OutlineItemsController(new OutlineItemRepository(new PilotPlantEntities()));
//            //var result = cont.UploadWord();
//            //Assert.IsInstanceOfType(result, typeof(System.Web.Mvc.ActionResult));
//            Assert.IsTrue(true);
//        }
//        #endregion
//        [TestMethod]
//        public void ShouldReturnColumnListForFermPlanTable()
//        {
//            // Arrange
//            const string modernfile = @"..\..\TestFiles\140618_QA1-D3_Outline.docx";
//            var docx = new DocxReader();

//            // Act
//            var retval = docx.GetSamplingDetailsColumnList(modernfile);

//            //Assert
//            Assert.IsTrue(retval.Count == 11);
//        }
//        #region "Long running tests"
//        //[TestMethod]
//        //public void ShouldReadFileFor140506()
//        //{
//        //    const string rundate = @"140506";
//        //    var docx = new FileFinder();
//        //    IQueryable<string> retval = docx.FindOutlineFile(rundate);

//        //    Assert.IsTrue(retval.Any());

//        //}
//        //[TestMethod]
//        //public void ShouldReturnAListOfOutlineWordDocsFromFermtationFolder()
//        //{
//        //    // Arrange
//        //    const string modernfile = @"Outline";
//        //    var docx = new SamplingTableSurvey();

//        //    // Act
//        //    IQueryable<string> retval = docx.FindOutlineFiles(modernfile);

//        //    //Assert
//        //    Assert.IsInstanceOfType(retval,typeof(IQueryable<string>));
//        //}
        

//        //[TestMethod]
//        //public void SurveyReturnsListOfColumns()
//        //{
//        //    var docx = new SamplingTableSurvey();
//        //    var retval = docx.SurveyColumns();
//        //    Assert.IsInstanceOfType(retval, typeof(List<object>));

//        //}
//        #endregion
//        #region "OC Tests"
//        [TestMethod]
//        public void ConvertsDataCsvFileToADataTable()
//        {
//            const string filepath = @"..\..\TestFiles\00135 AN.csv";
//            const string tablename = @"AN_ACe_Data";
//            CsvToDataTable csvToDataTable = new CsvToDataTable();
//            DataTable dt = csvToDataTable.Convert(filepath, tablename);

//            Assert.IsInstanceOfType(dt, typeof(DataTable));
//            Assert.IsTrue(dt.Rows.Count == 17);

//            CsvToSqlCreateTableStatement stmt = new CsvToSqlCreateTableStatement();
//            string sqlpath = filepath.Replace("csv", "sql");
//            stmt.Convert(filepath, sqlpath, tablename);
//        }
//        [TestMethod]
//        public void ConvertsStdCsvFileToADataTable()
//        {
//            const string filepath = @"..\..\TestFiles\AN_ACe STD.csv";
//            const string tablename = @"AN_ACe_STD";
//            CsvToDataTable csvToDataTable = new CsvToDataTable();
//            DataTable dt = csvToDataTable.Convert(filepath, tablename);

//            Assert.IsInstanceOfType(dt, typeof(DataTable));
//            Assert.IsTrue(dt.Rows.Count == 14);

//            CsvToSqlCreateTableStatement stmt = new CsvToSqlCreateTableStatement();
//            string sqlpath = filepath.Replace("csv", "sql");
//            stmt.Convert(filepath, sqlpath, tablename);



//        }
//        #endregion
//    }

//}
