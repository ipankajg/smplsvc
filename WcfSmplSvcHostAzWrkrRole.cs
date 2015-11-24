using System;
using System.Threading;
using System.ServiceModel;
using Microsoft.WindowsAzure.ServiceRuntime;
using LogicPundit.Samples.WcfSvc;

public class WorkerRole : RoleEntryPoint
{
    ServiceHost host = null;

    public override void Run()
    {
        while (true)
        {
            Thread.Sleep(10000);
        }
    }

    public override bool OnStart()
    {
        host = SmplSvcUtils.SecureHostCreate();
        return base.OnStart();
    }

    public override void OnStop()
    {
        SmplSvcUtils.HostDelete(host);
        base.OnStop();
    }
}

