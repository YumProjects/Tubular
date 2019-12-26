using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Tubular.Multitasking;
using Tubular.Logging;

namespace Tubular.Http
{
    public class HttpListener : RunnableTask
    {
        public int port { get; private set; }
        public bool pending => received.Count > 0;

        TcpListener listener;
        List<HttpContext> received; // Stores newly received contexts

        object receivedLock; // Lock object for mutex

        LoggerClass log;

        // Creates a new HttpListener bound to 0.0.0.0
        public HttpListener(int port)
        {
            this.port = port;
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            received = new List<HttpContext>();

            receivedLock = new object();

            log = new LoggerClass("http");
        }

        // Returns the next context in the received list
        public HttpContext ReceiveContext()
        {
            while (!pending) Thread.Sleep(1);
            HttpContext result = received[0];
            lock (receivedLock) received.RemoveAt(0);
            return result;
        }

        // Async version of ReceiveContext
        public Task<HttpContext> ReceiveContextAsync()
        {
            return Task.Run(() => ReceiveContext());
        }

        // Starts the listener
        protected override void OnStart()
        {
            log.LogInfo("Starting HTTP server bound to endpoint " + listener.LocalEndpoint + "...");
            listener.Start();
            log.LogInfo("Started.");
        }

        // Stops the listener
        protected override void OnEnd()
        {
            log.LogInfo("Stopping HTTP server...");
            listener.Stop();
            log.LogInfo("Stopped.");
        }

        // Main run method for HttpListener
        protected override void Run(object[] args)
        {
            while (!stopSignaled)
            {
                if (listener.Pending())
                {
                    try
                    {
                        // Accepts a new TCP client
                        TcpClient client = listener.AcceptTcpClient();
                        Task.Run(() => ReveiveConnection(client));
                    }
                    catch(Exception ex)
                    {
                        log.LogError(ex.Message);
                    }
                }
                Thread.Sleep(1); // Make sure the loop does't use to much CPU
            }
        }

        void ReveiveConnection(TcpClient client)
        {
            LoggerClass clientLog = new LoggerClass("http: " + client.Client.RemoteEndPoint);
            try
            {
                clientLog.LogInfo("Receiving connection from " + client.Client.RemoteEndPoint + ".");
                // Reads the request stream into an HttpRequest instance
                HttpRequest request = new HttpRequest();
                request.FromStream(client.GetStream());

                // Waits for the mutex then adds the new context
                lock (receivedLock) received.Add(new HttpContext(request, client));
                clientLog.LogInfo("Connection Received .");
            }
            catch (Exception ex)
            {
                clientLog.LogError(ex.Message);
            }
        }
    }
}