using System;
using System.Runtime.Remoting.Messaging;
using FilterPipeline.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Process.UnitTest
{
    [TestClass]
    public class FilterPipelineTest
    {
        [TestMethod]
        public void StringWith_Id_ReturnsAnEmptyString()
        {
            var input = "RunId";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new IdFilter())
                .Register(new NameFilter())
                .Execute(input);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void StringWith_Name_ReturnsAnEmptyString()
        {
            var input = "Machine Name";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new IdFilter())
                .Register(new NameFilter())
                .Execute(input);

            Assert.AreEqual(string.Empty, result);
        }
        [TestMethod]
        public void StringWith_Phase_ReturnsTheInput()
        {
            var input = "Growth Phase Name";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new IdFilter())
                .Register(new NameFilter())
                .Register(new RunDateFilter())
                .Execute(input);

            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void StringWith_RunDate_ReturnsEmpty()
        {
            var input = "Run_Date";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new IdFilter())
                .Register(new NameFilter())
                .Register(new RunDateFilter())
                .Execute(input);

            Assert.AreEqual(string.Empty, result);

            input = "Run Date";
            result = pipeline
                .Execute(input);

            Assert.AreEqual(string.Empty, result);

        }
        [TestMethod]
        public void StringWith_LastUpdate_ReturnsEmpty()
        {
            var input = "LastUpdate";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new UpdateFilter())
                .Execute(input);

            Assert.AreEqual(string.Empty, result);
        }
        [TestMethod]
        public void StringWith_UpdateBy_ReturnsValue()
        {
            var input = "UpdateBy";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new UpdateFilter())
                .Execute(input);

            Assert.AreEqual(input, result);
        }
        [TestMethod]
        public void FilterOutCrLf()
        {
            var input = "2nd\nLC";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new CrlfFilter())
                .Execute(input);

            Assert.AreEqual("2ndLC", result);
        }
        [TestMethod]
        public void FilterOutSpaces()
        {
            var input = "Citrate & Isocitrate";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new SpaceFilter())
                .Execute(input);

            Assert.AreEqual("Citrate&Isocitrate", result);
        }
        [TestMethod]
        public void FilterNh4()
        {
            var input = "NH4+ / NH3";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new NH4Filter())
                .Execute(input);
            Assert.AreEqual("NH4+/NH3", result);

            input = @"NH₄⁺/NH₃";
            result = pipeline.Execute(input);
            Assert.AreEqual("NH4+/NH3", result);

            input = @"NH3 & NH4";
            result = pipeline.Execute(input);
            Assert.AreEqual("NH4+/NH3", result);
        }

        [TestMethod]
        public void HappyPath_ShouldReturnA5DigitSampleNo()
        {
            var input = "1_00143_140709_sAA2178_300L_0h_";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new SampleSerialNoFromIdentFilter())
                .Execute(input);

            Assert.AreEqual("00143", result);
        }

        [TestMethod]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void SadPath_OnlyoneUnderscore()
        {
            var input = "1_00143";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new SampleSerialNoFromIdentFilter())
                .Execute(input);

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void SadPath_NoUnderscore()
        {
            var input = "Fe standard curve 1 ppm";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new SampleSerialNoFromIdentFilter())
                .Execute(input);

            Assert.AreEqual("", result);
        }

        //HP6097_100_2PercentHNO3 0.1 ppm
        [TestMethod]
        public void SadPath_WrongLengthforReturn()
        {
            var input = "HP6097_100_2PercentHNO3 0.1 ppm";
            Pipeline<string> pipeline = new Pipeline<string>();
            string result = pipeline.Register(new SampleSerialNoFromIdentFilter())
                .Execute(input);

            Assert.AreEqual("", result);
        }

    }
}
