using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlBuilder.Util
{
    /// <summary>
    /// Throw Exceptions
    /// </summary>
    internal class Throw
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
    }

    internal static class StringExtentions
    {
        public static string Left(this string self, int length)
        {
            Throw.IfIsNullOrEmpty(self);
            Throw.IfLessThanZero(length);

            return self.Length > length ? self.Substring(0, length) : self;
        }

        public static string RemoveLastChars(this string self, int length = 1)
        {
            Throw.IfIsNullOrEmpty(self);
            return self.Left(self.Length - length);
        }
    }
}