﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Validators
{
    public sealed class EditSupplierDetailsModelValidator : AbstractValidator<EditSupplierDetailsModel>
    {
        internal static readonly string ErrorElementName = $"edit-supplier-details";

        public EditSupplierDetailsModelValidator()
        {
            RuleFor(m => m.AvailableSupplierContacts)
                .Must(HaveAtLeastOneSelectedContact)
                .WithMessage("Select a supplier contact")
                .OverridePropertyName(ErrorElementName)
                .Must(HaveNoMoreThanTwoSelectedContacts)
                .WithMessage("You can only select up to two supplier contacts")
                .OverridePropertyName(ErrorElementName);
        }

        private static bool HaveAtLeastOneSelectedContact(IReadOnlyList<AvailableSupplierContact> availableSupplierContacts)
            => availableSupplierContacts.Any(c => c.Selected);

        private static bool HaveNoMoreThanTwoSelectedContacts(IReadOnlyList<AvailableSupplierContact> availableSupplierContacts)
            => availableSupplierContacts.Count(c => c.Selected) <= 2;
    }
}