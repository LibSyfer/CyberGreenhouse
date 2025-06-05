using Microsoft.Extensions.DependencyInjection;

namespace CyberGreenhouse.MessageBus.Abstractions
{
    public interface IMonitorMessageBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
