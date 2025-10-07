# PowerShell helper to generate WCF proxy using dotnet-svcutil
# Usage:
# 1. Install dotnet-svcutil: dotnet tool install --global dotnet-svcutil
# 2. Run this script from the project folder: .\GenerateProxy.ps1
#
# This will generate ConnectedServices\TRNService.cs which contains the client proxy classes.

$wsdl = "https://bar.rmto.ir/Service2/TRNServiceV02.svc?wsdl"
$out = "ConnectedServices\TRNService.cs"

Write-Host "Generating proxy from $wsdl ..."
# Namespaces: map the service namespace to our library namespace
dotnet-svcutil $wsdl -n "*,TerminalInquiryLib.ConnectedServices.TRNService" -o $out

if ($LASTEXITCODE -eq 0) {
    Write-Host "Proxy generated to $out. Add the file to the project and rebuild."
} else {
    Write-Host "dotnet-svcutil failed. Exit code: $LASTEXITCODE"
}
