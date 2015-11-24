using System;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Selectors;
using System.Collections.Generic;
using System.Linq;
using LogicPundit.Samples.WcfSvc;

class Program
{

    static ChannelFactory<ISmplSvc> CreateChannelFactory(string host)
    {
        BasicHttpBinding binding = new BasicHttpBinding();
        string uri = "http://" + host + ":" + SmplSvcConst.TcpPort + "/" + SmplSvcConst.UriSuffix;
        ChannelFactory<ISmplSvc> factory = new ChannelFactory<ISmplSvc>(binding, new EndpointAddress(uri));
        return factory;
    }

    static ChannelFactory<ISmplSvc> CreateSecureChannelFactory(string host)
    {
        //
        // Load private client and public server certificate.
        //
        var srvPubCert = new X509Certificate2("server.cer");
        var clntPvtCert = new X509Certificate2("client.pfx");

        //
        // Create a binding with Message security.
        //
        WSHttpBinding binding = new WSHttpBinding();
        binding.Security.Mode = SecurityMode.Message;
        binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
        binding.Security.Message.NegotiateServiceCredential = true;

        //
        // Create an endpoint identity to allow certificate subject name to
        // be different than host name in URI.
        //
        EndpointIdentity identity = EndpointIdentity.CreateX509CertificateIdentity(srvPubCert);
        string uri = "http://" + host + ":" + SmplSvcConst.TcpPort + "/" + SmplSvcConst.UriSuffix;
        ChannelFactory<ISmplSvc> factory =
            new ChannelFactory<ISmplSvc>(binding,
                                         new EndpointAddress(new Uri(uri), identity,
                                                             new System.ServiceModel.Channels.AddressHeaderCollection()));

        factory.Credentials.ClientCertificate.Certificate = clntPvtCert;
        factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode =
            System.ServiceModel.Security.X509CertificateValidationMode.Custom; 
        factory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator =
            new CustomCertificateValidator((srvPubCert));

        return factory;
    }

    static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: LogicPundit.Samples.WcfSvc.Client.exe [Server Name | Server IP Address]");
            return -1;
        }
        Console.WriteLine("Sending message <Hello> to server.");
        
        // var factory = CreateChannelFactory(args[0]);
        var factory = CreateSecureChannelFactory(args[0]);

        ISmplSvc proxy = factory.CreateChannel();
        string fromServer = proxy.Echo("Hello");
        Console.WriteLine("Result from server: " + fromServer);
        return 0;
    }
}

class CustomCertificateValidator : X509CertificateValidator
{
    X509Certificate2 validServerCertificate = null;
    public CustomCertificateValidator(X509Certificate2 srvCert)
    {
        validServerCertificate = srvCert;
    }
 
    public override void Validate(X509Certificate2 certificate)
    {
        //
        // Check if validated certificate is in valid certificates list.
        //
        if (validServerCertificate.Thumbprint == certificate.Thumbprint)
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
