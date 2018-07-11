using System;
using System.Runtime.InteropServices;

namespace MvvmDialogs.Contrib.ComShellDialogs.Native
{
    internal static class ShellNativeMethods
    {
        [DllImport( "shell32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
        internal static extern HResult SHCreateItemFromParsingName
        (
            [MarshalAs( UnmanagedType.LPWStr )] String displayNameOrPath,
            IntPtr bindContext,
            ref Guid interfaceId,
            [MarshalAs( UnmanagedType.Interface, IidParameterIndex = 2 )] out IShellItem2 shellItem
        );
    }
}
