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

            Console.WriteLine(request.RawUrl);

            foreach (var header in request.Headers.AllKeys)
            {
                Console.WriteLine(string.Format("{0}: {1}", header, request.Headers[header]));
            }

            if (request.HasEntityBody)
            {
                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                Console.WriteLine(reader.ReadToEnd());
                body.Close();
                reader.Close();
            }

            response.StatusCode = 200;
            response.Close();

            Console.WriteLine();
        }
    }
}
