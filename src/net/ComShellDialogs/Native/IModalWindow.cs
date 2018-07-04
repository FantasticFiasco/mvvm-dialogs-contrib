using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MvvmDialogs.Contrib.ComShellDialogs.Native
{
    [ComImport]
    [Guid( ShellIIDGuid.IModalWindow )]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    internal interface IModalWindow
    {
        [MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime ),
        PreserveSig]
        HResult Show([In] IntPtr parent);
    }
}
