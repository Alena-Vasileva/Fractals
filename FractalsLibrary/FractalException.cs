using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalsLibrary
{
    /// <summary>
    /// Автоматически сгенерированный класс исключения для библиотеки.
    /// </summary>
    [System.Serializable]
    public class FractalException : Exception
    {
        /// <summary>
        /// Исключение без сообщения.
        /// </summary>
        public FractalException() { }
        /// <summary>
        /// Исключение с сообщением.
        /// </summary>
        public FractalException(string message) : base(message) { }
        /// <summary>
        /// Исключение с сообщения и внутренним исключением.
        /// </summary>
        public FractalException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Исключение с дополнительной информацией.
        /// </summary>
        protected FractalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
