using CyberGreenhouse.MessageBus.RabbitMQ.Configurations;
using CyberGreenhouse.MessageBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace CyberGreenhouse.MessageBus.RabbitMQ.Extensions
{
    public static class RabbitMqDependencyInjectionExtensions
    {
        public static IMonitorMessageBusBuilder AddMonitorRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBaseRabbitMqServices(configuration);

            services.AddSingleton<IMonitorMessageBus, MonitorMessageBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<MonitorMessageBusRabbitMQ>>();

                return new MonitorMessageBusRabbitMQ(rabbitMQPersistentConnection, logger, sp);
            });

            services.AddSingleton<IHostedService>(sp => (MonitorMessageBusRabbitMQ)sp.GetRequiredService<IMonitorMessageBus>());

            return new MonitorMessageBusBuilder(services);
        }

        public static IMessageBusBuilder AddClientRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration, string clientName)
        {
            services.AddBaseRabbitMqServices(configuration);

            services.AddSingleton<IMessageBus, MessageBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<MessageBusRabbitMQ>>();
                var registerOptions = sp.GetRequiredService<IOptions<MessageBusRegister>>();

                return new MessageBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, registerOptions, clientName);
            });

            services.AddSingleton<IHostedService>(sp => (MessageBusRabbitMQ)sp.GetRequiredService<IMessageBus>());

            return new MessageBusBuilder(services);
        }

        private static IServiceCollection AddBaseRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBusSection = configuration.GetSection(RabbitMQSettings.Section);
            services.Configure<RabbitMQSettings>(messageBusSection);

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var messageBusSettings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value ?? throw new ArgumentNullException(nameof(RabbitMQSettings));

                var factory = new ConnectionFactory
                {
                    HostName = messageBusSettings.Host ?? throw new ArgumentNullException(nameof(messageBusSettings.Host)),
                };

                if (!string.IsNullOrEmpty(messageBusSettings.Username))
                {
                    factory.UserName = messageBusSettings.Username;
                }

                if (!string.IsNullOrEmpty(messageBusSettings.Password))
                {
                    factory.Password = messageBusSettings.Password;
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });

            return services;
        }

        private class MessageBusBuilder(IServiceCollection services) : IMessageBusBuilder
        {
            public IServiceCollection Services => services;
        }

        private class MonitorMessageBusBuilder(IServiceCollection services) : IMonitorMessageBusBuilder
        {
            public IServiceCollection Services => services;
        }
    }
}
