using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace CyberGreenhouse.MessageBus.Extensions
{
    public static class MessageBusBuilderExtensions
    {
        public static IMonitorMessageBusBuilder RegisterMonitorHandler<T>(this IMonitorMessageBusBuilder messageBusBuilder)
            where T : class, IMonitorMessageBusHandler
        {
            messageBusBuilder.Services.AddTransient<IMonitorMessageBusHandler, T>();

            return messageBusBuilder;
        }

        public static IClientMessageBusBuilder RegisterMessageHandler<T, TH>(this IClientMessageBusBuilder messageBusBuilder)
            where T: BusMessage
            where TH: class, IIntegrationMessageHandler<T>
        {
            messageBusBuilder.Services.AddKeyedTransient<IIntegrationMessageHandler, TH>(typeof(T));

            messageBusBuilder.Services.Configure<MessageBusRegister>(r =>
            {
                r.MessageTypes[typeof(T).Name] = typeof(T);
            });

            return messageBusBuilder;
        }
    }
}
