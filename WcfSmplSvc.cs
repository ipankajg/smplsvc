using System;
using System.Threading;
using System.ServiceModel;

namespace LogicPundit.Samples.WcfSvc
{

public class SmplSvc : ISmplSvc
{
    public string Echo(string msg)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Message from client : {0}", msg);
        Console.WriteLine("--");
        Console.WriteLine("RequestMessage: ");
        Console.WriteLine(OperationContext.Current.RequestContext.RequestMessage.ToString());
        return string.Format("{0}", msg);
    }
}

public static class SmplSvcUtils
{
    public static ServiceHost HostCreate()
    {
        BasicHttpBinding binding = new BasicHttpBinding();
        Uri serviceUri = new Uri("http://localhost:"+ SmplSvcConst.TcpPort);
        ServiceHost host = new ServiceHost(typeof(SmplSvc), serviceUri);
        host.AddServiceEndpoint(typeof(ISmplSvc), binding, "SmplSvc");
        host.Open();
        return host;
    }

    public static void HostDelete(ServiceHost host)
    {
        host.Close();
    }
}

}
