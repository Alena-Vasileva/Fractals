using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalsLibrary
{
    [System.Serializable]
    public class FractalException : Exception
    {
        public FractalException() { }
        public FractalException(string message) : base(message) { }
        public FractalException(string message, Exception inner) : base(message, inner) { }
        protected FractalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
