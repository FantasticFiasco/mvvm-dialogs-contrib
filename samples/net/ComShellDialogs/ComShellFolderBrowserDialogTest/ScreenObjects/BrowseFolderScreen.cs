using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ComShellFolderBrowserDialogTest.ScreenObjects
{
    public class BrowseFolderScreen : AppScreen
    {
        public BrowseFolderScreen(Window window, ScreenRepository screenRepository)
            : base(window, screenRepository)
        {
        }

        public virtual void ClickOK()
        {
            Window.Get<Button>(SearchCriteria.ByText("Select Folder")).Click();
        }

        public virtual void ClickCancel()
        {
            Window.Get<Button>(SearchCriteria.ByText("Cancel")).Click();
        }
    }
}
