﻿using FluentValidation.TestHelper;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Models.Supplier;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Validators.Supplier;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Order.Validators.Supplier
{
    public static class ConfirmSupplierModelValidatorTests
    {
        [Theory]
        [MockInlineAutoData(null)]
        [MockInlineAutoData(false)]
        public static void Validate_ValuesMissing_ThrowsValidationError(
            bool? confirmSupplier,
            ConfirmSupplierModel model,
            ConfirmSupplierModelValidator systemUnderTest)
        {
            model.ConfirmSupplier = confirmSupplier;

            var result = systemUnderTest.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ConfirmSupplier)
                .WithErrorMessage(ConfirmSupplierModelValidator.ConfirmSupplierErrorMessage);
        }
    }
}
