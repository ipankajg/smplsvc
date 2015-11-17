using System;
using System.ServiceModel;
using WcfSmplSvc;

class Program
{

static void Main(string[] args)
{
    BasicHttpBinding binding = new BasicHttpBinding();
            
    Uri serviceUri = new Uri("http://localhost:"+ SmplSvcConst.TcpPort);
    ServiceHost host = new ServiceHost(typeof(SmplSvc), serviceUri);
    host.AddServiceEndpoint(typeof(ISmplSvc), binding, SmplSvcConst.UriSuffix);
    host.Open();

    Console.WriteLine("WCF service is running, press any key to close it.");
    Console.ReadLine();

    host.Close();            
}

}

