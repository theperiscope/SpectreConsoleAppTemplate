﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace SpectreConsoleAppTemplate.Spectre
{
    /// <summary>
    /// ITypeRegistrar and ITypeResolver in Spectre Console allows for abstraction of the specific DI implementation
    /// </summary>
    public sealed class SpectreTypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _builder;

        public SpectreTypeRegistrar(IServiceCollection builder)
        {
            _builder = builder;
        }

        public ITypeResolver Build()
        {
            return new SpectreTypeResolver(_builder.BuildServiceProvider());
        }

        public void Register(Type service, Type implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            _builder.AddSingleton(service, (provider) => func());
        }
    }
}