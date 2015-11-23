using System;
using System.ServiceModel;
using LogicPundit.Samples.WcfSvc;

class Program
{

static void Main(string[] args)
{
    ServiceHost host = SmplSvcUtils.HostCreate();

    Console.WriteLine("WCF service is running, press any key to close it.");
    Console.ReadLine();

    SmplSvcUtils.HostDelete(host);
}

}

