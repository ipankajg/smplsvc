using System;
using System.Threading;
using System.ServiceModel;

namespace WcfSmplSvc
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

}
