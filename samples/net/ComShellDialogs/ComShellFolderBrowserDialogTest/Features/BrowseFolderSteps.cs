﻿using System.IO;
using System.Reflection;
using ComShellFolderBrowserDialogTest.ScreenObjects;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TestBaseClasses.Features;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.ScreenObjects;

namespace ComShellFolderBrowserDialogTest.Features
{
    [Binding]
    public class BrowseFolderSteps : FeatureSteps<MainScreen>
    {
        private BrowseFolderScreen browseFolderScreen;

        protected override Application LaunchApplication()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            string applicationFilePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ComShellFolderBrowserDialog.exe");

            return Application.Launch(applicationFilePath);
        }

        protected override MainScreen GetMainScreen(ScreenRepository screenRepository)
        {
            return screenRepository.Get<MainScreen>("COM Shell - Folder Browser Dialog", InitializeOption.NoCache);
        }
        
        [Given("I have browsed a folder")]
        public void BrowseFolder()
        {
            browseFolderScreen = MainScreen.ClickBrowse();
        }
        
        [When("I press confirm")]
        public void Confirm()
        {
            browseFolderScreen.ClickOK();
        }
        
        [When("I cancel")]
        public void Cancel()
        {
            browseFolderScreen.ClickCancel();
        }
        
        [Then("the folder should be opened")]
        public void VerifyFolderWasOpened()
        {
            Assert.That(MainScreen.FileName, Is.Not.Empty);
        }
        
        [Then("the folder should not be opened")]
        public void VerifyFolderWasNotOpened()
        {
            Assert.That(MainScreen.FileName, Is.Empty);
        }
    }
}
