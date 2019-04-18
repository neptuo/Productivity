using Neptuo.Productivity;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Neptuo")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyTitle(ProductInfo.Name)]
[assembly: AssemblyDescription(ProductInfo.Description)]

[assembly: AssemblyVersion(VersionInfo.Version)]
[assembly: AssemblyInformationalVersion(VersionInfo.Version)]
[assembly: AssemblyFileVersion(VersionInfo.Version)]