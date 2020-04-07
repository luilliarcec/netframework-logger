using System;
using System.Diagnostics;
using System.IO;

namespace Luilliarcec.Logger
{
    public class Log
    {
        /// <summary>
        /// Log Path
        /// </summary>
        public static string Path { get; set; } = "./.log";

        /// <summary>
        /// Save the file with error type
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>True or False</returns>
        public static bool Error(Exception ex)
        {
            return Save(ex, "Error");
        }

        /// <summary>
        /// Save the file with warning type
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>True or False</returns>
        public static bool Warning(Exception ex)
        {
            return Save(ex, "Warning");
        }

        /// <summary>
        /// Save the file with information type
        /// </summary>
        /// <param name="ex">Excepcion</param>
        /// <returns>True or False</returns>
        public static bool Info(Exception ex)
        {
            return Save(ex, "Info");
        }

        /// <summary>
        /// Delete the file
        /// </summary>
        /// <returns>True or False</returns>
        public static bool Drop()
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Copy the file to a destination path
        /// </summary>
        /// <param name="path">Destination path</param>
        /// <returns>True or False</returns>
        public static bool Copy(string path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    if (File.Exists($"{path}/logs.txt"))
                    {
                        File.Delete($"{path}/logs.txt");
                    }

                    File.Copy(Path, $"{path}/logs.txt");
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Verify that the file exists in the directory
        /// </summary>
        /// <returns>True or False</returns>
        public static bool Exists()
        {
            return File.Exists(Path);
        }

        #region <<< PRIVATE FUNCTIONS >>>
        /// <summary>
        /// Save the log in the specific path
        /// </summary>
        /// <param name="exception">Excepcion</param>
        /// <param name="error_level">Error level</param>
        /// <returns>True or False</returns>
        private static bool Save(Exception exception, string error_level, bool inner = false)
        {
            StreamWriter streamWriter = new StreamWriter(Path, true);

            try
            {
                StackTrace stacktrace = new StackTrace(exception, true);

                streamWriter.WriteLine(Header(error_level, inner) + Body(exception, stacktrace) + Footer(exception));
                streamWriter.Dispose();
                streamWriter.Close();

                if (exception.InnerException != null)
                {
                    Save(exception.InnerException, error_level, true);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                streamWriter.Dispose();
                streamWriter.Close();
                return false;
            }
        }

        /// <summary>
        /// Body Log
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="stacktrace">StackTrace</param>
        /// <returns>string</returns>
        private static string Body(Exception exception, StackTrace stacktrace)
        {
            return GetCurrenTime() + GetOrigin(exception) + GetNamespaceFile(stacktrace) + GetTriggerEvent(exception) + GetErrorLine(stacktrace) + GetExceptionType(exception) + GetErrorMessage(exception);
        }

        /// <summary>
        /// Footer Log
        /// </summary>
        /// <param name="inner">Exception</param>
        /// <returns>string</returns>
        private static string Footer(Exception exception)
        {
            return exception.InnerException != null ? "" : Environment.NewLine;
        }

        /// <summary>
        /// Header Log
        /// </summary>
        /// <param name="error_level">Error level</param>
        /// <param name="inner">Inner exception</param>
        /// <returns>string</returns>
        private static string Header(string error_level, bool inner)
        {
            return inner ? "" : $"************************ {error_level} ************************{Environment.NewLine}";
        }

        /// <summary>
        /// Get current date and time
        /// </summary>
        /// <returns>string</returns>
        private static string GetCurrenTime()
        {
            return $"Timestamp: {DateTime.Now}{Environment.NewLine}";
        }

        /// <summary>
        /// Get the error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>string</returns>
        private static string GetErrorMessage(Exception exception)
        {
            return $"Error Message: {exception.Message}{Environment.NewLine}";
        }

        /// <summary>
        /// Gets the type of exception 
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>string</returns>
        private static string GetExceptionType(Exception exception)
        {
            return $"Exception Type: {exception.GetType().FullName}{Environment.NewLine}";
        }

        /// <summary>
        /// Gets the line where the error occurred
        /// </summary>
        /// <param name="stacktrace">StackTrace</param>
        /// <returns>string</returns>
        private static string GetErrorLine(StackTrace stacktrace)
        {
            return $"Error Line: {stacktrace.GetFrame(0).GetFileLineNumber()}{Environment.NewLine}";
        }

        /// <summary>
        /// Gets the error triggering event
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>string</returns>
        private static string GetTriggerEvent(Exception exception)
        {
            return $"Trigger Event: {exception.TargetSite}{Environment.NewLine}";
        }

        /// <summary>
        /// Gets the namespace of the file that generated the error
        /// </summary>
        /// <param name="stacktrace">StackTrace</param>
        /// <returns>string</returns>
        private static string GetNamespaceFile(StackTrace stacktrace)
        {
            return $"File Location: {stacktrace.GetFrame(0).GetMethod().DeclaringType.FullName}{Environment.NewLine}";
        }

        /// <summary>
        /// Get the application, library or compilation that generated the error 
        /// </summary>
        /// <param name="exception">Excepcion</param>
        /// <returns>string</returns>
        private static string GetOrigin(Exception exception)
        {
            return $"Origin: {exception.Source}{Environment.NewLine}";
        }
        #endregion
    }
}
