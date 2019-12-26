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
        public string message { get; set; } = "";

        // Empty constructor
        public HttpResponce() { }

        // Standard constructor for status code and message
        public HttpResponce(int statusCode, string message)
        {
            this.statusCode = statusCode;
            this.message = message;
        }

        public override void FromStream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);

            // Read the standard first line and create the request instance
            string nextLine = reader.ReadLine();
            statusCode = int.Parse(nextLine.Split(' ')[1]);
            message = nextLine.Split(' ')[2];

            // Parse header until an empty line is found
            do
            {
                nextLine = reader.ReadLine();
                if (nextLine != "")
                {
                    string name = nextLine.Split(':')[0].Trim();
                    string value = nextLine.Split(':')[1].Trim();
                    SetHeader(name, value);
                }
            } while (nextLine != "");

            /* If the header contains a Content-Length then save that
               many bytes as the body */
            if (ContainsHeader("Content-Length"))
            {
                int length = int.Parse(GetHeader("Content-Length"));
                char[] buffer = new char[length];
                reader.ReadBlock(buffer, 0, length);
                body = new string(buffer);
            }
        }

        public override void FromString(string str)
        {
            // Split the responce into lines
            string[] lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Read the standard first line
            statusCode = int.Parse(lines[0].Split(' ')[1]);
            message = lines[0].Split(' ')[2];

            // Parse headers until an empty line is found
            int line;
            for (line = 1; line < lines.Length && lines[line] != ""; line++)
            {
                string name = lines[line].Split(':')[0].Trim();
                string value = lines[line].Split(':')[1].Trim();
                SetHeader(name, value);
            }

            // Copy the remaining lines as the body
            for (; line < lines.Length; line++)
            {
                body += lines[line];
            }
        }

        public override string ToString()
        {
            // Start with the standard request line
            string result = "HTTP/1.0 " + statusCode + " " + message;

            // Add all headers
            foreach (KeyValuePair<string, string> header in GetHeaders())
            {
                result += header.Key + ": " + header.Value + "\r\n";
            }

            // If the responce has a body then add the required empty line + body
            if (body.Length > 0) result += "\r\n" + body;

            return result;
        }
    }
}
