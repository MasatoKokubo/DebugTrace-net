using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DebugTrace.Log4net")]
[assembly: AssemblyDescription("Bridge library that uses Log4net for DebugTrace output.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Masato Kokubo")]
[assembly: AssemblyProduct("DebugTrace.Log4net")]
[assembly: AssemblyCopyright("(C) 2018 Masato Kokubo")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e5bba6b6-c829-4ad8-950a-07270948679a")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.6.0.0")]
[assembly: AssemblyFileVersion("0.6.0.0")]

// read Log4Net Configuration file
[assembly: log4net.Config.XmlConfigurator(ConfigFile=@"Log4net.config", Watch=true)] // added
