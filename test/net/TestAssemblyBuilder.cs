using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDialogs.ContribTest
{
    public class TestAssemblyBuilder
    {
        private readonly string assemblyName;
        private readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;

        public TestAssemblyBuilder(string assemblyName)
        {
            this.assemblyName = assemblyName;

            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.RunAndSave);

            moduleBuilder = assemblyBuilder.DefineDynamicModule(
                "MainModule",
                FileName);
        }

        public Type CreateType(string fullName, Type parenType = null)
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                fullName,
                TypeAttributes.Public,
                parenType);

            return typeBuilder.CreateType();
        }

        public Assembly Build()
        {
            assemblyBuilder.Save(FileName);
            return Assembly.LoadFrom(FileName);
        }

        private string FileName => assemblyName + ".dll";
    }
}
