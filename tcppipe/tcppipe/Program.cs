using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tcppipe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 3)
            {
                DisplayUsage();
                return;
            }

            int incommingPort, outcommingPort;

            try
            {
                incommingPort = int.Parse(args[0]);
                outcommingPort = int.Parse(args[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            string outcommingIP = args[1];
            string certificatePath = null;

            if (args.Length == 4)
            {
                certificatePath = args[3];

                if (!File.Exists(certificatePath))
                {
                    Console.WriteLine("Certificate file is not exist: " + certificatePath);
                    Environment.Exit(1);
                    return;
                }
            }

            new SslTcpServer(incommingPort, outcommingIP, outcommingPort, certificatePath).RunServer();
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("To start the server specify:");
            Console.WriteLine("tcppipe incommingPort=8080 outcommingIP=8.8.8.8 outcommingPort=80 incommingCertificateFile=\"...\" outcommingCertificateFile=\"...\"");
            Environment.Exit(1);
        }
    }
}
