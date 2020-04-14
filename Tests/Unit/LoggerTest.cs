using Luilliarcec.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void CheckThatTheLogDoesNotExist()
        {
            Log.Drop();
            Assert.IsFalse(Log.Exists());
        }

        [TestMethod]
        public void CheckThatTheLogDoesExist()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            Assert.IsTrue(Log.Exists());
            Log.Drop();
        }

        [TestMethod]
        public void CheckThatTheLogHasBeenDeleted()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            Assert.IsTrue(Log.Drop());
        }

        [TestMethod]
        public void VerifyThatTheLogHasBeenCopiedToASpecifiedPath()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            System.IO.Directory.CreateDirectory("./test_files");
            Assert.IsTrue(Log.Copy("./test_files"));
            Log.Drop();
            System.IO.Directory.Delete("./test_files", true);
        }

        [TestMethod]
        public void CheckThatTheLogIsSaved()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(Log.Error(ex));
                Assert.IsTrue(Log.Warning(ex));
                Assert.IsTrue(Log.Info(ex));
            }

            Log.Drop();
        }

        [TestMethod]
        public void CheckThatTheLogIsSavedAsError()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            string text = System.IO.File.ReadAllText(Log.Path);

            StringAssert.Contains(text, "* Error *");

            Log.Drop();
        }

        [TestMethod]
        public void CheckThatTheLogIsSavedAsWarning()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Log.Warning(ex);
            }

            string text = System.IO.File.ReadAllText(Log.Path);

            StringAssert.Contains(text, "* Warning *");

            Log.Drop();
        }

        [TestMethod]
        public void CheckThatTheLogIsSavedAsInfo()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                Log.Info(ex);
            }

            string text = System.IO.File.ReadAllText(Log.Path);

            StringAssert.Contains(text, "* Info *");

            Log.Drop();
        }
    }
}
