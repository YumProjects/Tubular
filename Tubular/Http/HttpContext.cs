﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Tubular.Http
{
    // This class is used to pair each TCP client with it's HTTP request
    public class HttpContext
    {
        public HttpRequest request { get; private set; }
        public TcpClient client { get; private set; }

        public HttpContext(HttpRequest request, TcpClient client)
        {
            this.request = request;
            this.client = client;
        }

        public void Close()
        {
            client.Close();
        }
    }
}