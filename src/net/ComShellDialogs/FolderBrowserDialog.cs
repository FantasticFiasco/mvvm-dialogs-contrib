using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
    /// <summary>Provides static methods to display a COM Shell API Folder Browser dialog window. This dialog is the 2Windows File Explorer-style (2-pane) folder browser dialog introduced in Windows Vista and not the single tree-view based dialog from older versions of Windows.</summary>
    internal static class FolderBrowserDialog
    {
        /// <summary>Shows the folder browser dialog. Returns null if the dialog cancelled. Otherwise returns the selected path.</summary>
        /// <param name="parentWindowHandle">Handle to the Win32 window that will parent the dialog. This value can be NULL (IntPtr.Zero).</param>
        /// <param name="title">Text to display in the title bar of the window. This value may be null.</param>
        /// <param name="initialDirectory">Path to the initial directory to display in the dialog. This value may be null.</param>
        /// <returns>The full path to the selected directory.</returns>
        public static String ShowDialog(IntPtr parentWindowHandle, String title, String initialDirectory)
        {
            NativeFileOpenDialog nfod = new NativeFileOpenDialog();
            try
            {
                return ShowDialogInner( nfod, parentWindowHandle, title, initialDirectory );
            }
            finally
            {
                Marshal.ReleaseComObject( nfod );
            }
        }

        private static readonly HResult HResult_Win32_Canceled = Utility.HResultFromWin32Error( (UInt32)Win32ErrorCode.Cancelled );

        private static String ShowDialogInner(IFileOpenDialog dialog, IntPtr parentWindowHandle, String title, String initialDirectory)
        {
            //IFileDialog ifd = dialog;
            FileOpenOptions flags =
                FileOpenOptions.NoTestFileCreate |
                FileOpenOptions.PathMustExist |
                FileOpenOptions.PickFolders |
                FileOpenOptions.ForceFilesystem;

            dialog.SetOptions( flags );
            
            if( title != null )
            {
                dialog.SetTitle( title );
            }

            if( initialDirectory != null )
            {
                IShellItem2 initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
                if( initialDirectoryShellItem != null )
                {
                    dialog.SetFolder( initialDirectoryShellItem );
                }
            }

            HResult result = dialog.Show( parentWindowHandle );
            if( result == HResult.Ok )
            {
                IShellItemArray resultsArray;
                dialog.GetResults( out resultsArray );

                String[] fileNames = Utility.GetFileNames( resultsArray );
                return fileNames.Length == 0 ? null : fileNames[0];
            }
            else if( result == HResult_Win32_Canceled )
            {
                // Cancelled by user.
                return null;
            }
            else
            {
                UInt32 win32ErrorCode = Utility.Win32ErrorFromHResult( (UInt32)result );
                throw new Win32Exception( error: (Int32)win32ErrorCode );
            }
        }
    }
}
