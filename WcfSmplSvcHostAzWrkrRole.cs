using System;
using System.Threading;
using System.ServiceModel;
using Microsoft.WindowsAzure.ServiceRuntime;
using WcfSmplSvc;

public class WorkerRole : RoleEntryPoint
{
    public override void Run()
    {
        while (true)
        {
            Thread.Sleep(10000);
        }
    }

    public override bool OnStart()
    {
        BasicHttpBinding binding = new BasicHttpBinding();
            
        Uri serviceUri = new Uri("http://localhost:"+ SmplSvcConst.TcpPort);
        ServiceHost host = new ServiceHost(typeof(SmplSvc), serviceUri);
        host.AddServiceEndpoint(typeof(ISmplSvc), binding, "SmplSvc");
        host.Open();

        return base.OnStart();
    }
}

