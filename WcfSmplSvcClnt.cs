using System;
using System.ServiceModel;
using WcfSmplSvc;

class Program
{
static int Main(string[] args)
{
    if (args.Length != 1)
    {
        Console.WriteLine("Usage: WcfSmplSvcClnt.exe [Server Name | Server IP Address]");
        return -1;
    }

    Console.WriteLine("Sending message <Hello> to server.");

    BasicHttpBinding binding = new BasicHttpBinding();
    string uri = "http://" + args[0] + ":" + SmplSvcConst.TcpPort + "/" + SmplSvcConst.UriSuffix;
    ChannelFactory<ISmplSvc> factory = new ChannelFactory<ISmplSvc>(binding, new EndpointAddress(uri));
    ISmplSvc proxy = factory.CreateChannel();
    string fromServer = proxy.Echo("Hello");
    Console.WriteLine("Result from server: " + fromServer);
    return 0;
}
}

