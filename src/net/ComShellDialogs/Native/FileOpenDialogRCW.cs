using System.Runtime.InteropServices;

namespace MvvmDialogs.Contrib.ComShellDialogs.Native
{
    [ComImport]
    [ClassInterface( ClassInterfaceType.None )]
    [TypeLibType( TypeLibTypeFlags.FCanCreate )]
    [Guid( ShellCLSIDGuid.FileOpenDialog )]
    internal class FileOpenDialogRCW
    {
    }
}
