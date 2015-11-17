using System;
using System.ServiceModel;

namespace WcfSmplSvc
{

[ServiceContract]
public interface ISmplSvc
{
    [OperationContract]
    string Echo(string inputText);
}

public static class SmplSvcConst
{
    public static int TcpPort = 8500;
    public static string UriSuffix = "SmplSvc";
}

}

