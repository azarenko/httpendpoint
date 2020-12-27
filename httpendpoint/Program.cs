using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace httpendpoint
{
    class Program
    {
        static HttpListener listener;

        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            listener = new HttpListener();
            foreach (string prefix in args)
            {
                listener.Prefixes.Add(prefix);
            }
            listener.Start();
            Console.WriteLine("Listening for connections");

            do
            {
                IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                result.AsyncWaitHandle.WaitOne();
            }
            while (true);
        }

        static void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            LogRequest(request.RawUrl);


            if (request.HasEntityBody)
            {
                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                if (request.ContentType != null)
                {
                    Console.WriteLine("Client data content type {0}", request.ContentType);
                }
                Console.WriteLine("Client data content length {0}", request.ContentLength64);

                Console.WriteLine("Start of client data:");
                // Convert the data to a string and display it on the console.
                string s = reader.ReadToEnd();
                Console.WriteLine(s);
                Console.WriteLine("End of client data:");
                body.Close();
                reader.Close();
            }

            response.StatusCode = 500;
            response.Close();
        }

        static void LogRequest(string data)
        {
            Console.WriteLine(data);
        }

        static void LogResponse(string data)
        {
            
        }
    }
}
