ASP.NET Core Service Fabric DI Sample
=====================================

This quick code sample demonstrates an ASP.NET Core web API as a Service Fabric service. It specifically looks at different techniques for configuring ASP.NET Core's dependency injection container in cases where the DI container will also be needed for Service Fabric use (outside of the ASP.NET Core web API).

Key Technologies
----------------

The key technologies that enable these DI scenarios are:

* ASP.NET Core can [use Autofac (or other DI) containers](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#replacing-the-default-services-container) for its dependency injection container.
* Services can be registered in ASP.NET Core containers [at web host creation time](https://docs.microsoft.com/en-us/aspnet/core/api/microsoft.aspnetcore.hosting.webhostbuilder#Microsoft_AspNetCore_Hosting_WebHostBuilder_ConfigureServices_System_Action_Microsoft_Extensions_DependencyInjection_IServiceCollection__) (no need to wait until in `Startup.ConfigureServices`).
* Autofac containers can be easily created (so creating multiple containers is straight-forward) using [modules](http://docs.autofac.org/en/latest/configuration/modules.html). 

Dependency Injection Thoughts
-----------------------------

The challenge is that both Autofac and ASP.NET Core DI containers should be treated as immutable. So, once a container is built, no services should be added to it. However, ASP.NET Core needs to add a whole slew of services (over 100) to its container at the time the web app starts. This can make it challenging to use the same container in an ASP.NET Core app and in the hosting Service Fabric service.

Some techniques that might help:

1. The simplest solution, if possible, is to just create the container at the time that ASP.NET Core starts up. Some things that can help make this more feasible:
	1. Services can be added when the web host is created (rather than in `Startup.ConfigureServices`), so it's easy to register services known to the Service Fabric service without having to pass them into the web app. This is demonstrated in [AspNetCoreService.CreateServiceInstanceListeners](./AspNetCoreService/AspNetCoreService.cs), which registers the Service Fabric context for use in the ASP.NET Core app.
	2. If the services will be required outside of the web app, an Autofac module can be created in `Startup.ConfigureServices` and stored in a static property available outside of the web app.
2. If DI resources are needed before the ASP.NET Core app is setup, there are a couple options:
	1. Create two containers: one for use in the web app and one for use outside of it. Autofac modules can make it easy to setup matching containers. This is demonstrated in [Startup.cs](AspNetCoreService/Startup.cs#L53).
	2. It's also possible to update an existing Autofac container (also demonstrated in [Startup.cs](./AspNetCoreService/Startup.cs#L48)), though it is an anti-pattern. If you feel you need this route, consider commenting on [autofac/autofac#811](https://github.com/autofac/Autofac/issues/811) to explain why you need the Update method.