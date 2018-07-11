using System;
using System.Windows;
using System.Windows.Interop;
using MvvmDialogs.Contrib.ComShellDialogs.Native;
using MvvmDialogs.FrameworkDialogs;
using MvvmDialogs.FrameworkDialogs.FolderBrowser;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;

namespace MvvmDialogs.Contrib.ComShellDialogs
{
    /// <summary>Implementation of IFrameworkDialogFactory that uses the COM Shell API dialogs.</summary>
    public class ComShellDialogFactory : IFrameworkDialogFactory
    {
        /// <summary>Create an instance of the Windows folder browser dialog. Note the ShowNewFolderButton option is ignored as the New Folder button is always visible.</summary>
        /// <param name="settings">The settings for the folder browser dialog.</param>
        public IFrameworkDialog CreateFolderBrowserDialog(FolderBrowserDialogSettings settings)
        {
            if( settings == null ) throw new ArgumentNullException(nameof(settings));

            return new FolderBrowserComShellDialog( settings );
        }

        /// <summary>Create an instance of the default Windows message box.</summary>
        /// <param name="settings">The settings for the message box.</param>
        public IMessageBox CreateMessageBox(MessageBoxSettings settings)
        {
            if( settings == null ) throw new ArgumentNullException(nameof(settings));

            return new MessageBoxWrapper( settings );
        }

        /// <summary>Create an instance of the Windows open file dialog.</summary>
        /// <param name="settings">The settings for the open file dialog.</param>
        public IFrameworkDialog CreateOpenFileDialog(OpenFileDialogSettings settings)
        {
            if( settings == null ) throw new ArgumentNullException(nameof(settings));

            return new FileOpenComShellDialog( settings );
        }

        /// <summary>Create an instance of the Windows save file dialog.</summary>
        /// <param name="settings">The settings for the save file dialog.</param>
        public IFrameworkDialog CreateSaveFileDialog(SaveFileDialogSettings settings)
        {
            if( settings == null ) throw new ArgumentNullException(nameof(settings));

            return new FileSaveComShellDialog( settings );
        }
    }

    internal static class Extensions
    {
        public static IntPtr GetWindowHandle(this Window window)
        {
            if( window == null ) return IntPtr.Zero;
            WindowInteropHelper wih = new WindowInteropHelper( window );
            return wih.Handle;
        }
    }

    internal class FileOpenComShellDialog : IFrameworkDialog
    {
        private readonly OpenFileDialogSettings settings;

        public FileOpenComShellDialog(OpenFileDialogSettings settings)
        {
            this.settings = settings;
        }

        public Boolean? ShowDialog(Window owner)
        {
            Filter[] filters = Filter.ParseWindowsFormsFilter( this.settings.Filter );
            Int32 filterIndex = this.settings.FilterIndex - 1; // convert 1-based to 0-based index.

            if( this.settings.Multiselect )
            {
                String[] fileNames = FileOpenDialog.ShowMultiSelectDialog(owner.GetWindowHandle(), this.settings.Title, this.settings.InitialDirectory, this.settings.FileName, filters, filterIndex);
                if ( fileNames != null )
                {
                    this.settings.FileNames = fileNames;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                String fileName = FileOpenDialog.ShowSingleSelectDialog(owner.GetWindowHandle(), this.settings.Title, this.settings.InitialDirectory, this.settings.FileName, filters, filterIndex);
                if( fileName != null )
                {
                    this.settings.FileName = fileName;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    internal class FileSaveComShellDialog : IFrameworkDialog
    {
        private readonly SaveFileDialogSettings settings;

        public FileSaveComShellDialog(SaveFileDialogSettings settings)
        {
            this.settings = settings;
        }

        public Boolean? ShowDialog(Window owner)
        {
            Filter[] filters = Filter.ParseWindowsFormsFilter( this.settings.Filter );
            Int32 filterIndex = this.settings.FilterIndex - 1; // convert 1-based to 0-based index.

            String fileName = FileSaveDialog.ShowDialog( owner.GetWindowHandle(), this.settings.Title, this.settings.InitialDirectory, this.settings.FileName, filters, filterIndex );
            if( fileName != null )
            {
                this.settings.FileName = fileName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    internal class FolderBrowserComShellDialog : IFrameworkDialog
    {
        private readonly FolderBrowserDialogSettings settings;

        public FolderBrowserComShellDialog(FolderBrowserDialogSettings settings)
        {
            this.settings = settings;
        }

        public Boolean? ShowDialog(Window owner)
        {
            String folderPath = FolderBrowserDialog.ShowDialog( owner.GetWindowHandle(), this.settings.Description, this.settings.SelectedPath );
            if( folderPath != null )
            {
                this.settings.SelectedPath = folderPath;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
