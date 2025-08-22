using AOE3_HomeCity.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AOE3_HomeCity
{
    public static class Helper
    {
        public enum EqualityType : byte
        {
            ReferenceEquals = 0,
            ExactEquals = 1,
            UniqueIdentifierEquals = 2
        }

        public static bool IsXmlFile(string path)
        {
            return Path.GetExtension(path) == ".xml";
        }
        public static bool DoesContainXmlFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return false;
            var files = (new DirectoryInfo(path)).GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            return files != null && files.Length > 0;
        }
        public static string GetLastModifiedXmlFile(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException();
            var files = (new DirectoryInfo(directory)).GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            if (files == null || files.Length == 0)
                throw new FileNotFoundException();
            return files.Where(x => x.LastWriteTime.Equals(files.Max(y => y.LastWriteTime))).First().FullName;
        }
        public static string ReadText(string file)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            StreamReader reader = null;
            var text = string.Empty;
            try
            {
                text = (reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read))).ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return text;
        }
        public static void WriteText(string text, string file)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (file == null)
                throw new ArgumentNullException("file");
            StreamWriter writer = null;
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
                writer = new StreamWriter(File.Create(file), Encoding.Unicode);
                writer.Write(text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
        public static DialogResult ShowMessageBox(
            Exception Exception = null,
            string PrimaryMessage = "Operation failed.",
            string Title = "",
            string Seperator = ":\n",
            MessageBoxButtons Buttons = MessageBoxButtons.OK,
            MessageBoxIcon Icon = MessageBoxIcon.Error)
        {
            string msg = "", title = string.IsNullOrEmpty(Title) ? Icon.ToString() : Title, sep = string.IsNullOrEmpty(Seperator) ? " " : Seperator;
            for (var ex = Exception; ex != null; ex = ex.InnerException)
                msg += sep + ex.Message;
            msg = string.IsNullOrEmpty(PrimaryMessage) ? msg.Substring(sep.Length) : (PrimaryMessage + msg);
            return MessageBox.Show(msg, title, Buttons, Icon);
        }

        public static bool IsEqual<T>(this T item, T value, EqualityType eq)
            where T : HomeCityComponent<T>
        {
            return
                    (eq == EqualityType.ReferenceEquals ? ReferenceEquals(item, value) :
                        (eq == EqualityType.ExactEquals ? item.ExactEquals(value) :
                            (eq == EqualityType.UniqueIdentifierEquals ? item.UniqueIdEquals(value) :
                                item.Equals(value))));
        }
        public static bool Includes<T>(this IEnumerable<T> collection, T value, EqualityType eq)
            where T : HomeCityComponent<T>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (value == null)
                throw new ArgumentNullException("value");

            bool includes = false;
            foreach (var item in collection)
                if (item.IsEqual(value, eq))
                {
                    includes = true;
                    break;
                }
            return includes;
        }
        public static int HowManyOf<T>(this IEnumerable<T> collection, T value, EqualityType eq) 
            where T : HomeCityComponent<T>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (value == null)
                throw new ArgumentNullException("value");

            Func<T, bool> Equals = (item) =>
            {
                return 
                    (eq == EqualityType.ReferenceEquals ? ReferenceEquals(item, value) : 
                        (eq == EqualityType.ExactEquals ? item.ExactEquals(value) : 
                            (eq == EqualityType.UniqueIdentifierEquals ? item.UniqueIdEquals(value) : 
                                item.Equals(value))));
            };
            int count = 0;
            foreach (var item in collection)
                if (Equals.Invoke(item))
                    count++;
            return count;
        }
        public static IEnumerable<T> Excluding<T>(this IEnumerable<T> collection, T value, EqualityType eq)
            where T : HomeCityComponent<T>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (value == null)
                throw new ArgumentNullException("value");
            return collection.Where(x => !x.IsEqual(value, eq));
        }
        public static IEnumerable<T> Excluding<T>(this IEnumerable<T> collection, IEnumerable<T> values, EqualityType eq)
            where T : HomeCityComponent<T>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (values == null)
                throw new ArgumentNullException("value");
            return collection.Where(x => !values.Includes(x, eq));
        }
        public static IEnumerable<T> CombineDistinctly<T>(this IEnumerable<T> first, IEnumerable<T> second, EqualityType eq)
            where T : HomeCityComponent<T>
        {
            var l1 = new List<T>(first);
            var l2 = (new List<T>(second)).Excluding(l1, eq).ToList();
            l1.TrimExcess();
            l2.TrimExcess();
            return l1.Union(l2);
        }
    }

    public class GenericCompare<T> : IEqualityComparer<T>
        where T : class
    {
        public GenericCompare(Func<T, object> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            _expression = expression;
        }


        private Func<T, object> _expression;



        public bool Equals(T x, T y)
        {
            var first = _expression.Invoke(x);
            var second = _expression.Invoke(y);
            return first != null && second != null && first.Equals(second);
        }
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}