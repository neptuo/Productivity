function Vsix-GetId 
{
    param ([string] $manifest)
    
    [xml]$manifestXml = Get-Content $manifest

    $atomNs = New-Object System.Xml.XmlNamespaceManager $manifestXml.NameTable
    $atomNs.AddNamespace("ns", $manifestXml.DocumentElement.NamespaceURI) | Out-Null

    $identity = $manifestXml.SelectSingleNode("//ns:Identity", $atomNs);
    if ($identity) 
    {
        return $identity.Attributes["Id"].Value;
    }

    Throw "Unnable to find attribute Id of Identity element in file '$($manifest)'.";
}