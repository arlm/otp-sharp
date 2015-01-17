using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OtpSharp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("OtpSharp")]
[assembly: AssemblyCopyright("Copyright © 2012 by Devin Martin and released under the MIT license")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("24ed90a7-75fe-4faf-940c-a21234445eee")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// The CI server will provide the Build Number and Revision
[assembly: AssemblyVersion("1.2.*")]
[assembly: AssemblyFileVersion("1.2.0.0")]

// allow the test assembly to unit test internals
#if DEBUG
[assembly: InternalsVisibleTo("OtpSharp.Tests")]
#else
[assembly: InternalsVisibleTo("OtpSharp.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100dfee9dc22137af56d87defec35ff3376e4665d10ce3784e4625ffed71f2512eef4831a043c606333f333a732fd4168a58447049b6d58fe4b49522b9c9ac7203d72f44b75a55d44ecbc1691cd361e01b570eec2b538afd1e8e6df3dc3afac811e122fec112b2fc4c4b09aa112643471ae21eb0ca492f178451310baab6ca725a7")]
#endif

[assembly: CLSCompliant(true)]
