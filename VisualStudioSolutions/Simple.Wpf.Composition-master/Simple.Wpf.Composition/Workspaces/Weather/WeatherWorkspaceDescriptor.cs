﻿namespace Simple.Wpf.Composition.Workspaces.Weather
{
    using System;
    using Autofac;
    using Services;

    public sealed class WeatherWorkspaceDescriptor : IWorkspaceDescriptor
    {
        private readonly Uri _resources = new Uri("/simple.wpf.composition;component/workspaces/weather/resources.xaml", UriKind.RelativeOrAbsolute);

        private readonly WorkspaceFactory _workspaceFactory;

        public WeatherWorkspaceDescriptor(WorkspaceFactory workspaceFactory)
        {
            _workspaceFactory = workspaceFactory;
        }

        public int Position { get { return 0; } }

        public string Name { get { return "Weather Workspace"; } }

        public Workspace CreateWorkspace()
        {
            return _workspaceFactory.Create<Registrar, WeatherController>(_resources);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Registrar
        {
            // Scoped registrations, these will be thrown away when the workspace is disposed...
            public Registrar(ContainerBuilder container)
            {
                container.RegisterType<WeatherService>().As<IWeatherService>().InstancePerLifetimeScope();

                container.RegisterType<WeatherViewModel>();
                container.RegisterType<WeatherController>();
            }
        }
    }
}