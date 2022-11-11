using FreightSynchro.Entitys;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace FreightSynchro
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            FreightSynchro fs = new FreightSynchro();
            fs.Message += new MessageHandler(Message);
            fs.Start();
        }

        public static void Message(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}