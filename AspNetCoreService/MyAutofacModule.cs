using Autofac;
using NetStandardLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreService
{
    public class MyAutofacModule : Module
    {
        static MyClass myClassInstance = new MyClass(5);

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(myClassInstance).As<IMyInterface>();
        }
    }
}
