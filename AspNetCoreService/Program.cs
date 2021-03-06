﻿using Autofac;
using Microsoft.ServiceFabric.Services.Runtime;
using NetStandardLibrary;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreService
{
    internal static class Program
    {
        public static IContainer Container { get; set; }

        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            // Create an Autofac container prior to setting up the web host
            // in order to demonstrate how to use an existing container withing
            // an ASP.NET Core host.
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyAutofacModule());
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                // Exercising the container prior to setting up the ASP.NET Core app
                var myInterfaceImpl = scope.Resolve<IMyInterface>();
                Debug.WriteLine($"ASP.NET Core Service not yet started, IMyInterface available with MyProperty value of {myInterfaceImpl.MyProperty}");
            }

            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                ServiceRuntime.RegisterServiceAsync("AspNetCoreServiceType",
                    context => new AspNetCoreService(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(AspNetCoreService).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
