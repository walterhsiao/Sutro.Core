using gs;
using gsGCode.builders.mockClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sutro.PathWorks.Plugins.API;
using System;

namespace gsCore.UnitTests
{
    [TestClass()]
    public class GCodeBuilderTests
    {
        [TestMethod()]
        public void AddLine()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            GCodeLine l = new GCodeLine(100, GCodeLine.LType.Comment);
            l.comment = "testComment";
            gcb.AddLine(l);
            Assert.IsTrue(mockGCA.Lines.Count == 1);
            Assert.AreEqual("testComment", mockGCA.Lines[0].comment);
            Assert.AreEqual(GCodeLine.LType.Comment, mockGCA.Lines[0].type);
            Assert.AreEqual(0, mockGCA.Lines[0].lineNumber);

            GCodeLine l2 = new GCodeLine(100, GCodeLine.LType.GCode);
            l2.comment = "testComment2";
            l2.code = 10;
            GCodeParam p = new GCodeParam();
            p.doubleValue = 2.5;
            l2.parameters = new GCodeParam[1] { p };
            gcb.AddLine(l2);
            Assert.IsTrue(mockGCA.Lines.Count == 2);
            Assert.AreEqual("testComment2", mockGCA.Lines[1].comment);
            Assert.AreEqual(10, mockGCA.Lines[1].code);
            Assert.AreEqual(GCodeLine.LType.GCode, mockGCA.Lines[1].type);
            Assert.AreEqual(1, mockGCA.Lines[1].lineNumber);
            Assert.AreEqual(p, mockGCA.Lines[1].parameters[0]);
        }

        [TestMethod()]
        public void AddCommentLine()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            gcb.AddCommentLine("comment");
            Assert.IsTrue(mockGCA.Lines.Count == 1);
            Assert.AreEqual("comment", mockGCA.Lines[0].comment);
            Assert.AreEqual(GCodeLine.LType.Comment, mockGCA.Lines[0].type);
            Assert.AreEqual(1, mockGCA.Lines[0].lineNumber);
        }

        [TestMethod()]
        public void AddExplicitLine()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            gcb.AddExplicitLine("explicit");
            Assert.IsTrue(mockGCA.Lines.Count == 1);
            Assert.AreEqual("explicit", mockGCA.Lines[0].orig_string);
            Assert.IsNull(mockGCA.Lines[0].comment);
            Assert.AreEqual(GCodeLine.LType.UnknownString, mockGCA.Lines[0].type);
            Assert.AreEqual(1, mockGCA.Lines[0].lineNumber);
        }

        [TestMethod()]
        public void BeginAndEndGLine()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            gcb.BeginGLine(1, "comment");
            Assert.IsTrue(mockGCA.Lines.Count == 0);

            gcb.EndLine();
            Assert.IsTrue(mockGCA.Lines.Count == 1);
            Assert.AreEqual("comment", mockGCA.Lines[0].comment);
            Assert.AreEqual(GCodeLine.LType.GCode, mockGCA.Lines[0].type);
            Assert.AreEqual(0, mockGCA.Lines[0].lineNumber);
        }

        [TestMethod()]
        public void BeginAndEndMLine()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            gcb.BeginMLine(1, "comment");
            Assert.IsTrue(mockGCA.Lines.Count == 0);

            gcb.EndLine();
            Assert.IsTrue(mockGCA.Lines.Count == 1);
            Assert.AreEqual("comment", mockGCA.Lines[0].comment);
            Assert.AreEqual(GCodeLine.LType.MCode, mockGCA.Lines[0].type);
            Assert.AreEqual(0, mockGCA.Lines[0].lineNumber);
        }

        [TestMethod()]
        public void BeginGLineAndAppend()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            gcb.BeginGLine(1, "comment");
            Assert.IsTrue(mockGCA.Lines.Count == 0);

            gcb.AppendComment("addingComment");
            gcb.AppendI("intTest", 3);
            gcb.AppendF("floatTest", 2.3);
            gcb.AppendS("stringTest", "stringTest");
            gcb.AppendL("labelTest");

            gcb.EndLine();
            Assert.IsTrue(mockGCA.Lines.Count == 1);
            Assert.AreEqual("addingComment", mockGCA.Lines[0].comment);
            Assert.AreEqual(GCodeLine.LType.GCode, mockGCA.Lines[0].type);
            Assert.AreEqual(0, mockGCA.Lines[0].lineNumber);

            var par = mockGCA.Lines[0].parameters;
            Assert.AreEqual(4, par.Length);
            Assert.AreEqual(GCodeParam.PType.IntegerValue, par[0].type);
            Assert.AreEqual(3, par[0].intValue);
            Assert.AreEqual("intTest", par[0].identifier);
            Assert.AreEqual(GCodeParam.PType.DoubleValue, par[1].type);
            Assert.AreEqual(2.3, par[1].doubleValue);
            Assert.AreEqual("floatTest", par[1].identifier);
            Assert.AreEqual(GCodeParam.PType.TextValue, par[2].type);
            Assert.AreEqual("stringTest", par[2].textValue);
            Assert.AreEqual("stringTest", par[2].identifier);
            Assert.AreEqual(GCodeParam.PType.NoValue, par[3].type);
            Assert.AreEqual("labelTest", par[3].identifier);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void AddCommentToNullLine()
        {
            MockIGCodeAccumulator mockGCA = new MockIGCodeAccumulator();
            GCodeBuilder gcb = new GCodeBuilder(mockGCA);

            gcb.AppendComment("addingComment");
        }
    }
}