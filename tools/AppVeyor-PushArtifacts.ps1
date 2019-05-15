param([string] $version = $env:APPVEYOR_BUILD_NUMBER, [string] $jobId = $env:APPVEYOR_JOB_ID)

if (-not($version)) 
{ 
    Throw "Parameter -Version is required";
}

if (-not($jobId)) 
{ 
    Throw "Parameter -JobId is required";
}

Push-Location $PSScriptRoot;
. ".\Vsix-GetId.ps1";

# Feed file paths.
$templatePath = Resolve-Path -Path "..\NightlyFeedTemplate.xml";
$outputPath = "..\NightlyFeed.xml";
Set-Content $outputPath "";
$outputPath = Resolve-Path -Path $outputPath;

$targetVersion = "0.0.$version";

$timestamp = Get-Date -Format o;
[xml]$xml = Get-Content $templatePath;

# Namespaces
$atomNs = New-Object System.Xml.XmlNamespaceManager $xml.NameTable;
$atomNs.AddNamespace("ns", "http://www.w3.org/2005/Atom") | Out-Null;
$vsixNs = New-Object System.Xml.XmlNamespaceManager $xml.NameTable;
$vsixNs.AddNamespace("vsix", "http://schemas.microsoft.com/developer/vsx-syndication-schema/2010") | Out-Null;

$xml.SelectSingleNode("//ns:updated", $atomNs).InnerText = $timestamp;

$entryXml = $xml.SelectSingleNode("//ns:entry", $atomNs);
$xml.DocumentElement.RemoveChild($entryXml) | Out-Null;

foreach ($artifact in (Get-ChildItem ..\*.vsix -Recurse)) 
{
    $artifactFileName = "$($artifact.BaseName)-v$($targetVersion)$($artifact.Extension)";
    Push-AppveyorArtifact $artifact.FullName -FileName $artifactFileName;

    $projectDirectory = $artifact.Directory.Parent.Parent;
    $manifestPath = Join-Path $projectDirectory.FullName "source.extension.vsixmanifest";
    $id = Vsix-GetId -Manifest $manifestPath;

    $artifactXml = $entryXml.Clone();

    # Modify entry
    $artifactXml.SelectSingleNode("//ns:id", $atomNs).InnerText = $id;
    $artifactXml.SelectSingleNode("//ns:title", $atomNs).InnerText = $artifact.BaseName;
    $artifactXml.SelectSingleNode("//ns:published", $atomNs).InnerText = $timestamp;
    $artifactXml.SelectSingleNode("//ns:updated", $atomNs).InnerText = $timestamp;
    $url = $artifactXml.SelectSingleNode("//ns:content", $atomNs).Attributes["src"].Value;
    $url = $url.Replace("{FileName}", $artifactFileName);
    $url = $url.Replace("{JobId}", $jobId);
    $artifactXml.SelectSingleNode("//ns:content", $atomNs).Attributes["src"].Value = $url;

    # Modify VSIX
    $artifactXml.SelectSingleNode("//vsix:Id", $vsixNs).InnerText = $id;
    $artifactXml.SelectSingleNode("//vsix:Version", $vsixNs).InnerText = $targetVersion;

    $xml.DocumentElement.AppendChild($artifactXml) | Out-Null;
}

$xml.Save($outputPath);
Push-AppveyorArtifact $outputPath -FileName "Feed.xml";

Pop-Location;