using Microsoft.Extensions.DependencyInjection;

namespace CyberGreenhouse.MessageBus.Abstractions
{
    public interface IMessageBusBuilder
    {
        public IServiceCollection Services { get; }
    }

    public interface IClientMessageBusBuilder
    {
        public IServiceCollection Services { get; }
    }
    public interface IMonitorMessageBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
