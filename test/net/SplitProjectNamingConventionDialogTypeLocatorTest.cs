using System;
using System.ComponentModel;
using System.Reflection;
using NUnit.Framework;
using MvvmDialogs.DialogTypeLocators;

namespace MvvmDialogs.ContribTest
{
    [TestFixture]
    public class SplitProjectNamingConventionDialogTypeLocatorTest
    {
        private Assembly testAssembly;
        private SplitProjectNamingConventionDialogTypeLocator dialogTypeLocator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // TODO set-up assemblies for testing
        }

        [SetUp]
        public void SetUp()
        {
            dialogTypeLocator = new SplitProjectNamingConventionDialogTypeLocator();
        }

        [Test]
        public void TestLocatorMapsViewAndVm()
        {
            Assert.Fail("Haven't written any tests yet, sorry.");
        }
    }
}
