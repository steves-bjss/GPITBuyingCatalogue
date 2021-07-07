﻿using System;
using System.Collections.Generic;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Settings;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders;

namespace NHSD.GPIT.BuyingCatalogue.Services.Orders
{
    public sealed class CreateOrderItemValidator : ICreateOrderItemValidator
    {
        private readonly ValidationSettings validationSettings;

        public CreateOrderItemValidator(ValidationSettings validationSettings)
        {
            this.validationSettings = validationSettings ?? throw new ArgumentNullException(nameof(validationSettings));
        }

        public AggregateValidationResult Validate(Order order, CreateOrderItemModel model, CatalogueItemType itemType)
        {
            if (order.CommencementDate is null)
                throw new ArgumentNullException(nameof(order), $"{nameof(order)}.{nameof(Order.CommencementDate)} should not be null.");

            var aggregateValidationResult = new AggregateValidationResult();
            var itemIndex = -1;

            foreach (var recipient in model.ServiceRecipients)
            {
                itemIndex++;
                aggregateValidationResult.AddValidationResult(
                    new ValidationResult(ValidateDeliveryDate(recipient.DeliveryDate, order.CommencementDate.Value, itemType)),
                    itemIndex);
            }

            return aggregateValidationResult;
        }

        private static ErrorDetails DeliveryDateError(string error)
        {
            return new(
                nameof(CreateOrderItemModel.ServiceRecipients),
                nameof(OrderItemRecipientModel.DeliveryDate),
                nameof(OrderItemRecipientModel.DeliveryDate) + error);
        }

        private static IReadOnlyList<ErrorDetails> NoErrors() => Array.Empty<ErrorDetails>();

        private static IReadOnlyList<ErrorDetails> Errors(params ErrorDetails[] details) => details;

        private IReadOnlyList<ErrorDetails> ValidateDeliveryDate(DateTime? deliveryDate, DateTime commencementDate, CatalogueItemType itemType)
        {
            if (itemType.Equals(CatalogueItemType.AssociatedService))
                return NoErrors();

            if (deliveryDate is null)
                return Errors(DeliveryDateError("Required"));

            return IsDeliveryDateWithinWindow(deliveryDate.Value, commencementDate)
                ? NoErrors()
                : Errors(DeliveryDateError("OutsideDeliveryWindow"));
        }

        private bool IsDeliveryDateWithinWindow(DateTime deliveryDate, DateTime commencementDate)
        {
            return deliveryDate >= commencementDate
                && deliveryDate <= commencementDate.AddDays(validationSettings.MaxDeliveryDateOffsetInDays);
        }
    }
}
