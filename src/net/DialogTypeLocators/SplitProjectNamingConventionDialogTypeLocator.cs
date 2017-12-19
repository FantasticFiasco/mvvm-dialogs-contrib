using System;
using System.ComponentModel;
using System.Reflection;

namespace MvvmDialogs.DialogTypeLocators
{
    /// <summary>
    /// Supercedes NamingConventionDialogTypeLocator for Mvvm projects where
    /// views follow the naming convention but are in a separate project or dll.
    /// </summary>
    public class SplitProjectNamingConventionDialogTypeLocator : NamingConventionDialogTypeLocator
    {
        internal static new DialogTypeLocatorCache Cache { get => NamingConventionDialogTypeLocator.Cache; }

        /// <summary>
        /// Locates the dialog type representing the specified view model in a user interface where
        /// these might be in separate projects.
        /// </summary>
        /// <param name="viewModel">The view model to find the dialog type for.</param>
        /// <returns>
        /// The dialog type representing the specified view model in a user interface.
        /// </returns>
        public new Type Locate(INotifyPropertyChanged viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Type viewModelType = viewModel.GetType();

            Type dialogType = Cache.Get(viewModelType);
            if (dialogType != null)
            {
                return dialogType;
            }

            string dialogName = GetDialogName(viewModelType);

            // Added for Mvvm projects where views follow the naming convention but are in a separate project or dll
            dialogType = GetAssemblyFromType(viewModelType).GetType(dialogName) ?? GetReferencedAssembly().GetType(dialogName);

            if (dialogType == null)
                throw new TypeLoadException(AppendInfoAboutDialogTypeLocators($"Dialog with name '{dialogName}' is missing."));

            Cache.Add(viewModelType, dialogType);

            return dialogType;
        }

        private static string GetDialogName(Type viewModelType)
        {
            string dialogName = viewModelType.FullName.Replace(".ViewModels.", ".Views.");

            var indexOfGenericMarker = dialogName.IndexOf('`');
            dialogName = (indexOfGenericMarker > 0 ? dialogName.Substring(0, indexOfGenericMarker) : dialogName);

            if (!dialogName.EndsWith("ViewModel", StringComparison.Ordinal))
                throw new TypeLoadException(AppendInfoAboutDialogTypeLocators($"View model of type '{viewModelType}' doesn't follow naming convention since it isn't suffixed with 'ViewModel'."));

            return dialogName.Substring(
                0,
                dialogName.Length - "ViewModel".Length);
        }

        private static Assembly GetAssemblyFromType(Type type)
        {
#if NETFX_CORE
            // GetTypeInfo is supported on UWP
            return type.GetTypeInfo().Assembly;
#else
            // Assembly is supported on all .NET versions
            return type.Assembly;
#endif
        }

        /// <summary>
        /// Added for Mvvm projects where views follow the naming convention but are in a separate project or dll
        /// </summary>
        /// <returns></returns>
        private static Assembly GetReferencedAssembly()
        {
            Assembly assembly = null;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.FullName.Contains("Views"))
                {
                    assembly = a;
                }
            }
            return assembly;
        }

        private static string AppendInfoAboutDialogTypeLocators(string errorMessage)
        {
            return
                errorMessage + Environment.NewLine +
                "If your project structure doesn't conform to the default convention of MVVM " +
                "Dialogs you can always define a new convention by implementing your own dialog " +
                "type locator. For more information on how to do that, please read the GitHub " +
                "wiki or ask the author.";
        }
    }
}
