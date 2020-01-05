using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Extensions
{
    public static class WebJobsBuilderExtensions
    {
        /// <summary>
        /// Adds the CosmosDB extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        public static IWebJobsBuilder AddMsSqlDb(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            
            builder.AddExtension<MsSqlDbExtensionConfigProvider>()               
                .ConfigureOptions<MsSqlDbOptions>((config, path, options) =>
                {
                    options.ConnectionString = config.GetConnectionString(Constants.DefaultConnectionStringName);

                    var section = config.GetSection(path);
                    section.Bind(options);
                });                

            builder.Services.AddSingleton<IMsSqlDbServiceFactory, MsSqlDbServiceFactory>();

            return builder;
        }

        /// <summary>
        /// Adds the CosmosDB extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        /// <param name="configure">An <see cref="Action{MsSqlDbOptions}"/> to configure the provided <see cref="MsSqlDbOptions"/>.</param>
        public static IWebJobsBuilder AddMsSqlDb(this IWebJobsBuilder builder, Action<MsSqlDbOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddMsSqlDb();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}