using System;
using Spectre.Console.Cli;

namespace SpectreConsoleAppTemplate.Spectre
{
    /// <summary>
    /// ITypeRegistrar and ITypeResolver in Spectre Console allows for abstraction of the specific DI implementation
    /// </summary>
    public sealed class SpectreTypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public SpectreTypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public object Resolve(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return _provider.GetService(type);
        }

        public void Dispose()
        {
            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}