# WCF based Azure cloud service without using Visual Studio
This is a sample WCF based azure cloud service created without using visual studio. I created this to learn the underlying mechanics of how cloud services are built without feeling lost due to magic that happens with visual studio and autogenerated code. This is a very basic example but shows how you can build a WCF service from scratch that can run in Azure worker role. It also shows how to do right abstractions for a service (such as separate of contract from implementation) so that you can avoid duplication and/or need for generation of proxy classes by visual studio.

# key component
1. Service contract - This describes the interface of the service. It is implemented in WcfSmplSvcCntrct.cs
2. Service implementation - This implements the actual service. It is implemented in WcfSmplSvc.cs
3. Console service host - A WCF service needs a host to run and this is a console based application to host the service. It is implemented in WcfSmplSvcHostCmd.cs
4. Azure cloud service host - This is another host for the service that allows us to run this service in Azure cloud service worker role. It is implemented in WcfSmplSvcHostAzWrkrRole.
5. Service client - A console based program that acts as a client to this service. It is implemented in WcfSmplSvcClnt.cs
6. Cloud service files - A csdef and cscfg that describes the cloud service (required for hosting this service in Azure).

You can either host this service on your local machine by running <i>LogicPundit.Samples.WcfSvc.Host.Console.exe</i> on the local machine in an elevated command prompt, or you can host it in azure cloud service by uploading WcfSmplSvc.cspkg and WcfSmplSvc.cscfg to the cloud service.

In this release, service only implements BasicHttpBinding but in future versions, I may add support for WsHttpBinding and support for cert based authentication etc.

# Compilation
Run cmpl.cmd from a visual studio x64 native tools command prompt. Please make sure that the cspack is in the same location as specified in cmpl.cmd. If not, change the cmpl.cmd with correct path for cspack.exe and other azure binaries.

