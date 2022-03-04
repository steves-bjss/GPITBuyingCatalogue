﻿using System.Collections.Generic;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.PublicBrowse.Homepage
{
    public class ContactUsConfirmation : AnonymousTestBase, IClassFixture<LocalWebApplicationFactory>
    {
        private static readonly Dictionary<string, string> Parameters = new();

        public ContactUsConfirmation(LocalWebApplicationFactory factory)
               : base(
                   factory,
                   typeof(HomeController),
                   nameof(HomeController.ContactUsConfirmation),
                   null)
        {
        }

        [Fact]
        public void ContactUsConfirmation_AllSectionsDisplayed()
        {
            CommonActions.PageTitle().Should().Be("Your message has been sent".FormatForComparison());
            CommonActions.GoBackLinkDisplayed().Should().BeTrue();
        }

        [Fact]
        public void ContactUsConfirmation_ClickBacklink()
        {
            CommonActions.ClickGoBackLink();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(HomeController),
                nameof(HomeController.Index))
                .Should().BeTrue();
        }

        [Theory]
        [InlineData(ContactUsModel.ContactMethodTypes.TechnicalFault, "Helpdesk Team")]
        [InlineData(ContactUsModel.ContactMethodTypes.Other, "Buying Catalogue Team")]
        public void ContactUsConfirmation_SetsCorrectContactMethod(
            ContactUsModel.ContactMethodTypes contactReason,
            string expectedContactMethod)
        {
            var parameters = new Dictionary<string, string>
            {
                { "ContactReason", contactReason.ToString() },
            };

            NavigateToUrl(
                typeof(HomeController),
                nameof(HomeController.ContactUsConfirmation),
                null,
                parameters);

            CommonActions.ElementTextContains(ByExtensions.DataTestId("contact-method-text"), expectedContactMethod).Should().BeTrue();
        }
    }
}