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
[assembly: InternalsVisibleTo("OtpSharp.Tests")]

[assembly: CLSCompliant(true)]
