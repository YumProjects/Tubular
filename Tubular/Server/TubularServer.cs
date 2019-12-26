using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Tubular.Http;
using Tubular.Multitasking;
using Tubular.Logging;

namespace Tubular.Server
{
    public class TubularServer : RunnableTask
    {
        // Basic server info
        public bool isApp { get; private set; }
        public string filePath { get; private set; }
        public int port { get; private set; }

        HttpListener listener;

        LoggerClass log = new LoggerClass("server");

        // Creates a Server instance from server info
        public TubularServer(bool isApp, string filePath, int port)
        {
            this.isApp = isApp;
            this.filePath = filePath;
            this.port = port;

            listener = new HttpListener(port);
        }

        // Starts the server
        protected override void OnStart()
        {
            log.LogInfo("Starting server...");
            listener.Start();
            log.LogInfo("Server started on port " + port);
        }

        // Stops the server
        protected override void OnEnd()
        {
            log.LogInfo("Stopping server...");
            listener.Stop();
            log.LogInfo("Server stopped.");
        }

        // Main run method for server
        protected override void Run(object[] args)
        {
            while (!stopSignaled)
            {
                if (listener.pending)
                {
                    // Reveive an HTTP context
                    HttpContext ctx = listener.ReceiveContext();

                    // Start a task to handle the context
                    Task.Run(() => HandleContext(ctx));
                }
                Thread.Sleep(1); // Make sure the loop does't use to much CPU
            }
        }

        // Handles a single HttpContext
        void HandleContext(HttpContext ctx)
        {
            LoggerClass clientLog = new LoggerClass("client: " + ctx.client.Client.RemoteEndPoint);
            clientLog.LogInfo("Handling request...");
            try
            {

            }
            catch (Exception ex)
            {
                clientLog.LogError(ex.Message);
            }

            // Close the context
            ctx.Close();
            clientLog.LogInfo("Request closed.");
        }
    }
}