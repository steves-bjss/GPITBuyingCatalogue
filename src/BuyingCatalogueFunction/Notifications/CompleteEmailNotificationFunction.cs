﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;

namespace BuyingCatalogueFunction.Notifications
{
    public class CompleteEmailNotificationFunction
    {
        private readonly ILogger<SendEmailNotificationFunction> logger;
        private readonly BuyingCatalogueDbContext dbContext;

        public CompleteEmailNotificationFunction(
            ILogger<SendEmailNotificationFunction> logger,
            BuyingCatalogueDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [Function(nameof(CompleteEmailNotification))]
        public async Task CompleteEmailNotification([QueueTrigger("%QUEUE:COMPLETE_EMAIL_NOTIFICATION%", Connection = "AzureWebJobsStorage")] string message)
        {
            try
            {
                if (!Guid.TryParse(message, out var guid))
                {
                    throw new NoneTransientException($"{nameof(CompleteEmailNotification)} - Invalid message {message}");
                }

                var notification = await dbContext.EmailNotifications.FindAsync(guid);
                if (notification != null)
                {
                    dbContext.EmailNotifications.Remove(notification);
                    dbContext.SaveChanges();
                }
            }
            catch (NoneTransientException ex)
            {
                logger.LogError(
                    ex,
                    "{Name} error - {Message}. None transient exception not thrown to suppress retries",
                    nameof(CompleteEmailNotification),
                    message);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "{Name} error - {Message}.",
                    nameof(CompleteEmailNotification),
                    message);
                throw;
            }
        }
    }
}
