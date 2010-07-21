using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class CommandLineArguments
    {
        public string Host { get; set; }
        public int Port { get; set; }

        //private static readonly CommandLineArguments Defaults = new CommandLineArguments("uploadz.myftp.org", 8081); 
        //private static readonly CommandLineArguments Defaults = new CommandLineArguments("78.133.42.34", 8081); 
        private static readonly CommandLineArguments Defaults = new CommandLineArguments("localhost", 8081); 

        public CommandLineArguments(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public static CommandLineArguments Parse(string[] args)
        {
            try
            {
                if (args.Length == 2)
                {
                    return new CommandLineArguments(args[0], Convert.ToInt32(args[1]));
                }
                return Defaults;
            }
            catch (Exception)
            {
                return Defaults;
            }
        }
    }
}
