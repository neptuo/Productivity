using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    [Serializable]
    public class XmlTemplateException : Exception
    {
        public string FileName { get; }

        public XmlTemplateException(Exception inner, string fileName)
            : base($"An XML related exception raised processing file '{fileName}'.", inner)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Creates a new instance for deserialization.
        /// </summary>
        /// <param name="info">A serialization info.</param>
        /// <param name="context">A streaming context.</param>
        protected XmlTemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
