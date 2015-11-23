echo off
setlocal

set _out=out
set _contractDll=LogicPundit.Samples.WcfSvc.Contract.dll
set _svcDll=LogicPundit.Samples.WcfSvc.dll
set _hostAzWrkrRole=LogicPundit.Samples.WcfSvc.Host.AzureWorkerRole.dll
set _hostCmd=LogicPundit.Samples.WcfSvc.Host.Console.exe
set _svcClient=LogicPundit.Samples.WcfSvc.Client.exe
set _azSdk=C:\Program Files\Microsoft SDKs\Azure\.NET SDK\v2.7\bin
set _azSdkAsms=%_azSdk%\runtimes\base
set _azRuntimeDll=Microsoft.WindowsAzure.ServiceRuntime.dll
set _azCspack=%_azSdk%\cspack.exe
set _libPath="%_out%","%_azSdkAsms%"
set _cspkg=%_out%\WcfSmplSvc.cspkg

del %_cspkg%
mkdir %_out%

REM Build WCF service contract DLL
csc /t:library /out:"%_out%\%_contractDll%" WcfSmplSvcCntrct.cs 

REM Build WCF service DLL
csc /t:library /lib:%_libPath% /r:"%_contractDll%" /out:"%_out%\%_svcDll%" WcfSmplSvc.cs 

REM Build Azure worker role host (.dll) for service
csc /t:library /lib:%_libPath% /r:"%_contractDll%" /r:"%_svcDll%" /r:"%_azRuntimeDll%" /out:"%_out%\%_hostAzWrkrRole%" WcfSmplSvcHostAzWrkrRole.cs 

REM Build console host for the service
csc /lib:%_libPath% /r:"%_contractDll%" /r:"%_svcDll%" /out:"%_out%\%_hostCmd%" WcfSmplSvcHostCmd.cs 

REM Build a client for the service
csc /lib:%_libPath% /r:"%_contractDll%" /out:"%_out%\%_svcClient%" WcfSmplSvcClnt.cs 

REM Create Azure package for service.
"%_azCspack%" WcfSmplSvc.csdef /role:WcfSmplSvc;"%CD%\%_out%";%_hostAzWrkrRole% /out:%_cspkg%

endlocal
echo on
