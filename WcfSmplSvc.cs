using System;
using System.Threading;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Selectors;
using System.Collections.Generic;
using System.Linq;

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
        host.AddServiceEndpoint(typeof(ISmplSvc), binding, SmplSvcConst.UriSuffix);
        host.Open();
        return host;
    }

    public static ServiceHost SecureHostCreate()
    {
        WSHttpBinding binding = new WSHttpBinding();

        //
        // Use Message Security.
        //
        binding.Security.Mode = SecurityMode.Message;
        binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
        binding.Security.Message.NegotiateServiceCredential = true;

        //
        // Load server private certificate and client public certificate.
        //
        var srvPvtCert = new X509Certificate2("server.pfx");
        var clntPubCert = new X509Certificate2("client.cer");
        List<X509Certificate2> clntCertList = new List<X509Certificate2>();
        clntCertList.Add(clntPubCert);

        //
        // Create the service host and use custom certificate validation.
        //
        Uri serviceUri = new Uri("http://localhost:"+ SmplSvcConst.TcpPort);
        ServiceHost host = new ServiceHost(typeof(SmplSvc), serviceUri);
        host.Credentials.ServiceCertificate.Certificate = srvPvtCert;
        host.Credentials.ClientCertificate.Authentication.CertificateValidationMode =
            System.ServiceModel.Security.X509CertificateValidationMode.Custom; 
        host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator =
            new SmplSvcCertificateValidator(clntCertList);
        host.AddServiceEndpoint(typeof(ISmplSvc), binding, SmplSvcConst.UriSuffix);
        host.Open();
        return host;
    }

    public static void HostDelete(ServiceHost host)
    {
        host.Close();
    }
}

class SmplSvcCertificateValidator : X509CertificateValidator
{
    List<X509Certificate2> _validCertificates = null;
    /// <summary>        
    /// List of valid public certificates (*.cer) loaded from some kind of
    /// repository.
    /// </summary>
    public SmplSvcCertificateValidator(List<X509Certificate2> validCertificates)
    {
        _validCertificates = validCertificates;
    }
     
    public override void Validate(X509Certificate2 certificate)
    {
        //
        // Check if validated certificate is in valid certificates list.
        //
        if (_validCertificates.Where(c => c.Thumbprint == certificate.Thumbprint).Any())
        {
            return;
        }
        else
        {
            //
            // Check the certificate using default validation.
            //
            X509CertificateValidator.PeerOrChainTrust.Validate(certificate);
        }
    }
}

}
