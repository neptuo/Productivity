param([string] $version = $env:APPVEYOR_BUILD_NUMBER)

if (-not($version)) 
{ 
    Throw "Parameter -Version is required";
}

$targetVersion = "0.0.$version";

Push-Location $PSScriptRoot;

Write-Host "Updating manifests to version '$targetVersion'.";

$manifestFiles = Get-ChildItem -Path ..\ -Filter *.vsixmanifest -Recurse -File -Name;
foreach ($manifestFile in $manifestFiles) 
{
    $manifestFile = Resolve-Path -Path "..\$manifestFile";
    Write-Host "Updating manifest $manifestFile";

    [xml]$manifestXml = Get-Content $manifestFile

    $ns = New-Object System.Xml.XmlNamespaceManager $manifestXml.NameTable
    $ns.AddNamespace("ns", $manifestXml.DocumentElement.NamespaceURI) | Out-Null

    $attrVersion = ""

    if ($manifestXml.SelectSingleNode("//ns:Identity", $ns))
    {
        $attrVersion = $manifestXml.SelectSingleNode("//ns:Identity", $ns).Attributes["Version"]
        
        $attrVersion.InnerText = $targetVersion
        $manifestXml.Save($manifestFile) | Out-Null
    }
}

Pop-Location;