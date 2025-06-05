using Microsoft.Extensions.DependencyInjection;

namespace CyberGreenhouse.MessageBus.Abstractions
{
    public interface IMessageBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
