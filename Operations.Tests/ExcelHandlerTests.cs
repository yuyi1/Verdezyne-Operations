using System;
using System.Data;
using System.IO;
using System.Linq;
using OfficeHandler;
using OfficeHandler.Contracts;
using OfficeHandler.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeHandler.Excel.EPPlus;
using OfficeHandler.Excel.Factory;

namespace Process.UnitTest
{
    [TestClass]
    public class ExcelTests
    {

        #region InitialTests
        //[TestMethod]
        //public void ShouldCreateNewFileAndPopulateCellA1_xlsx()
        //{
        //    // Arrange
        //    //const string legacyfile = @"..\..\TestFiles\new_file.xls";
        //    const string modernfile = @"..\..\TestFiles\new_file.xlsx";
        //    var injector = new Injector();

        //    // Act
        //    var retval = injector.CreateModernFile(modernfile);

        //    //Assert
        //    Assert.IsTrue(retval);
        //}

        //[TestMethod]
        //public void ShouldCreateNewFileAndPopulateCellA1_xls()
        //{
        //    // Arrange
        //    const string legacyfile = @"..\..\TestFiles\new_file.xls";
        //    //const string modernfile = @"..\..\TestFiles\new_file.xlsx";            
        //    var injector = new Injector();

        //    // Act
        //    var retval = injector.CreateLegacyFile(legacyfile);

        //    //Assert
        //    Assert.IsTrue(retval);
        //}


        //[TestMethod]
        ////[ExpectedException(typeof(NotImplementedException))]
        //public void ShouldInjectNewSheetIntoFile_xlsx()
        //{
        //    // Arrange
        //    //const string legacyfile = @"..\..\TestFiles\new_file.xls";
        //    const string modernfile = @"..\..\TestFiles\new_file.xlsx";
        //    var injector = new Injector();
        //    FileInfo fi = new FileInfo(modernfile);
        //    // Act
        //    var retval = injector.AddSheetToModernFile(fi, "DataImport");
        //    // Throws exception
        //    Assert.IsInstanceOfType(retval, typeof(IExcelSheet));
        //}

        //[TestMethod]
        ////[ExpectedException(typeof(NotImplementedException))]
        //public void ShouldInjectNewSheetIntoFile_xls()
        //{
        //    // Arrange
        //    const string legacyfile = @"..\..\TestFiles\new_file.xls";
        //    //const string modernfile = @"..\..\TestFiles\new_file.xlsx"; 
        //    var injector = new Injector();
        //    FileInfo fi = new FileInfo(legacyfile);
        //    // Act
        //    var retval = injector.AddSheetToLegacyFile(legacyfile);
        //    // Throws exception
        //    Assert.IsTrue(retval);
        //}

        //[TestMethod]
        //public void ShouldDeleteASheetFrom_xlsx()
        //{
        //    //const string legacyfile = @"..\..\TestFiles\new_file.xls";
        //    const string modernfile = @"..\..\TestFiles\new_file.xlsx";
        //    var injector = new Injector();
        //    var retval = injector.DeleteSheetFromModernFile(modernfile);
        //    Assert.IsTrue(retval);
        //}
        //[TestMethod]
        //public void ShouldDeleteASheetFrom_xls()
        //{
        //    const string legacyfile = @"..\..\TestFiles\new_file.xls";
        //    //const string modernfile = @"..\..\TestFiles\new_file.xlsx";
        //    var injector = new Injector();
        //    var retval = injector.DeleteSheetFromLegacyFile(legacyfile);
        //    Assert.IsTrue(retval);
        //}
        #endregion

        [TestMethod]
        public void ReadDataFromLiveFile()
        {
            const string modernfile = @"..\..\TestFiles\CLE_140509_140506_300L_GC1_LC1.xlsx";
            //const string modernfile = @"C:\temp\SampleApp.xlsx";
            var injector = new Injector();
            var retval = injector.ReadData(modernfile);
            Assert.IsTrue(retval);
        }


        [TestMethod]
        public void ShouldFindACertainFile()
        {

            var docx = new FileFinder();
            IQueryable<string> retval = docx.FindFileByName(@"140625_QA1-D3_Analysis.xlsm");
            System.Diagnostics.Debug.WriteLine(retval.FirstOrDefault());
            Assert.IsTrue(retval.Any());

        }

        [TestMethod]
        public void ShouldFindTheMasterSampleSubmissionExcelFile()
        {
            var ff = new FileFinder();
            ff._basefolder = @"S:\Research\Analytical\Sample Submission".Replace("S:\\", "\\\\james\\Shared\\");
            var retval = ff.FindByPartialFilename(@"Sample Submission V*.xlsx");
            Assert.IsTrue(retval.Any());
        }

    }
}
