using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubular.Http
{
    public class HttpResponce : HttpMessage
    {
        public int statusCode { get; set; }
        public string message { get; set; }

        public override void FromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void FromString(string str)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
