using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
	[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
	internal struct FilterSpec
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string Name;
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string Spec;

		internal FilterSpec(string name, string spec)
		{
			this.Name = name;
			this.Spec = spec;
		}
	}
}
