using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SQLBuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var select = new SelectBuilder();

            int? age = 1;

            int? teste = null;

            var command =
            select.Top(10)
                  .Distinct()
                  .Column("ID")
                  .Column("Name")
                  .Column("Age")
                  .From("Table")
                  .Where("ID").Eq(1).If(() => true)
                    .And.Where("Age").Gt(age).If(age)
                    .And.Where("Age").In(1, 2, 3, 4, 5)
                    .Or.Where("Age").NotIn(1, 2, 5, 6)
                    .Or.Where("Age").Between(1, 3).If(true)
                    .Or.Where("Name").Like("%Angelo%")
                    .And.Where("CreatedDate").Eq(teste)
                  .GetSelectCommand();

            Console.WriteLine(command);
            Console.ReadKey();
        }
    }
}

namespace System
{
    /// <summary>
    /// Throw Exceptions
    /// </summary>
    public class Throw
    {
        /// <summary>
        /// Throw ArgumentNullException if value is null
        /// </summary>
        /// <param name="value">Value</param>

        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfIsNull(object value, string paramName = "", Exception ex = null)
        {
            if (value == null)
                if (ex == null)
                    throw new ArgumentNullException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if value is Equals to Zero
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfEqZero(long value, string paramName = "", Exception ex = null)
        {
            if (value == 0)
                if (ex == null)
                    throw new ArgumentOutOfRangeException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if value is Less Than Zero 
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfLessThanZero(long value, string paramName = "", Exception ex = null)
        {
            if (value < 0)
                if (ex == null)
                    throw new ArgumentOutOfRangeException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if value is Less Than or Equals to Zero 
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfLessThanOrEqZero(long value, string paramName = "", Exception ex = null)
        {
            if (value <= 0)
                if (ex == null)
                    throw new ArgumentOutOfRangeException(paramName);
                else
                    throw ex;
        }


        /// <summary>
        /// Throw ArgumentNullException if value is Null or Empty
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfIsNullOrEmpty(string value, string paramName = "", Exception ex = null)
        {
            if (string.IsNullOrEmpty(value))
                if (ex == null)
                    throw new ArgumentNullException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw ArgumentNullException if value is Null or White Space
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfIsNullOrWhiteSpace(string value, string paramName = "", Exception ex = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                if (ex == null)
                    throw new ArgumentNullException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if value is Null or Empty. 
        /// Throw ArgumentOutOfRangeException if value length is Bigger Than length value parameter.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="length">Length</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfStringLengthBiggerThan(string value, long length, string paramName = "", Exception ex = null)
        {
            IfIsNullOrEmpty(value, paramName);

            if (value.Length > length)
                if (ex == null)
                    throw new ArgumentOutOfRangeException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if value list items count is Equals to Zero
        /// </summary>
        /// <typeparam name="T">List Type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfIsEmpty<T>(IEnumerable<T> value, string paramName = "", Exception ex = null)
        {
            if (value == null || (value != null) && value.Count() == 0)
                if (ex == null)
                    throw new ArgumentOutOfRangeException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw InvalidOperationException if value is False
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfIsFalse(bool value, string paramName = "", Exception ex = null)
        {
            if (!value)
                if (ex == null)
                    throw new InvalidOperationException(paramName);
                else
                    throw ex;
        }

        /// <summary>
        /// Throw InvalidOperationException if value is True
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="paramName">Param name</param>
        /// <param name="ex">Custom Exception</param>
        public static void IfIsTrue(bool value, string paramName = "", Exception ex = null)
        {
            if (value)
                if (ex == null)
                    throw new InvalidOperationException(paramName);
                else
                    throw ex;
        }
    }
}

namespace System
{
    public static class StringExtentions
    {
        public static string RemoveNonNumeric(this string self)
        {
            Throw.IfIsNullOrEmpty(self);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < self.Length; i++)
                if (Char.IsNumber(self[i]))
                    sb.Append(self[i]);
            return sb.ToString();
        }

        public static string RemoveNumeric(this string self)
        {
            Throw.IfIsNullOrEmpty(self);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < self.Length; i++)
                if (!Char.IsNumber(self[i]))
                    sb.Append(self[i]);
            return sb.ToString();
        }

        public static string RemoveSpecialChars(this string self)
        {
            char[] buffer = new char[self.Length];
            int idx = 0;

            foreach (char c in self)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z')
                    || (c >= 'a' && c <= 'z') || (c == '.') || (c == '_'))
                {
                    buffer[idx] = c;
                    idx++;
                }
            }

            return new string(buffer, 0, idx);
        }

        public static string ReplaceSpecialChars(this string self)
        {
            var specialChars = new List<char> { 'ã', 'õ', 'ñ', 'ê', 'û', 'î', 'ô', 'â', 'ç', 'ä', 'ü', 'ï', 'ö', 'Ã', 'Õ', 'Ñ', 'Ê', 'Û', 'Î', 'Ô', 'Â', 'Ç', 'Ä', 'Ü', 'Ï', 'Ö' };
            var normalChars = new List<char> { 'a', 'o', 'n', 'e', 'u', 'i', 'o', 'a', 'c', 'a', 'u', 'i', 'o', 'A', 'O', 'N', 'E', 'U', 'I', 'O', 'A', 'C', 'A', 'U', 'I', 'O' };

            var buffer = new char[self.Length];
            var index = 0;

            foreach (char c in self)
            {
                var indexOf = specialChars.IndexOf(c);
                if (indexOf >= 0)
                    buffer[index] = normalChars[indexOf];
                else
                    buffer[index] = c;

                index++;
            }

            var newString = new string(buffer, 0, self.Length);
            return newString.RemoveSpecialChars();
        }

        public static string Truncate(this string self, int length, string suffix = "")
        {
            Throw.IfIsNullOrEmpty(self);
            Throw.IfLessThanOrEqZero(length);

            if (self.Length <= length) return self;
            var fragment = self.Left(length);

            return string.Format("{0}{1}", fragment, suffix);
        }

        public static string Right(this string self, int length)
        {
            Throw.IfIsNullOrEmpty(self);
            Throw.IfLessThanZero(length);

            return self.Length > length ? self.Substring(self.Length - length) : self;
        }

        public static string Left(this string self, int length)
        {
            Throw.IfIsNullOrEmpty(self);
            Throw.IfLessThanZero(length);

            return self.Length > length ? self.Substring(0, length) : self;
        }

        public static bool In(this string self, params string[] items)
        {
            Throw.IfIsNullOrEmpty(self);
            Throw.IfIsNull(items);
            Throw.IfEqZero(items.Length);

            return items.Contains(self);
        }

        public static string RemoveLastChars(this string self, int length = 1)
        {
            Throw.IfIsNullOrEmpty(self);
            return self.Left(self.Length - length);
        }
    }
}