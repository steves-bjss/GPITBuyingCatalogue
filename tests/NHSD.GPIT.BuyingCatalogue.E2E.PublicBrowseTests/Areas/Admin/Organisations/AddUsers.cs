﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Admin;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.RandomData;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Users.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Admin.Organisations
{
    public sealed class AddUsers : AuthorityTestBase, IClassFixture<LocalWebApplicationFactory>
    {
        private const int OrganisationId = 2;

        private const string FirstNameRequired = "Enter a first name";
        private const string LastNameRequired = "Enter a last name";
        private const string TelephoneNumberRequired = "Enter a telephone number";
        private const string EmailAddressRequired = "Enter an email address";
        private const string EmailFormatIncorrect = "Enter an email address in the correct format, like name@example.com";
        private const string EmailAlreadyExists = "A user with this email address is already registered on the Buying Catalogue";

        private static readonly Dictionary<string, string> Parameters = new()
        {
            { nameof(OrganisationId), OrganisationId.ToString() },
        };

        public AddUsers(LocalWebApplicationFactory factory)
            : base(
                  factory,
                  typeof(OrganisationsController),
                  nameof(OrganisationsController.AddUser),
                  Parameters)
        {
        }

        [Fact]
        public void AddUser_ClickGoBackButton_ExpectedResult()
        {
            CommonActions.ClickGoBackLink();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrganisationsController),
                nameof(OrganisationsController.Details))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void AddUser_AddUser_ExpectedResult()
        {
            var user = GenerateUser.Generate();

            AdminPages.AddUser.EnterFirstName(user.FirstName);
            AdminPages.AddUser.EnterLastName(user.LastName);
            AdminPages.AddUser.EnterTelephoneNumber(user.TelephoneNumber);
            AdminPages.AddUser.EnterEmailAddress(user.EmailAddress);

            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrganisationsController),
                nameof(OrganisationsController.AddUserConfirmation))
                .Should()
                .BeTrue();

            var confirmationMessage = AdminPages.AddUser.GetConfirmationMessage();

            confirmationMessage.Should().BeEquivalentTo($"{user.FirstName} {user.LastName} account added");
        }

        [Fact]
        public void AddUser_EmptyInput_ThrowsErrors()
        {
            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrganisationsController),
                nameof(OrganisationsController.AddUser))
                .Should()
                .BeTrue();

            CommonActions.ErrorSummaryDisplayed().Should().BeTrue();
            CommonActions.ErrorSummaryLinksExist().Should().BeTrue();

            CommonActions.ElementShowingCorrectErrorMessage(AddUserObjects.FirstNameError, FirstNameRequired).Should().BeTrue();
            CommonActions.ElementShowingCorrectErrorMessage(AddUserObjects.LastNameError, LastNameRequired).Should().BeTrue();
            CommonActions.ElementShowingCorrectErrorMessage(AddUserObjects.TelephoneNumberError, TelephoneNumberRequired).Should().BeTrue();
            CommonActions.ElementShowingCorrectErrorMessage(AddUserObjects.EmailError, EmailAddressRequired).Should().BeTrue();
        }

        [Fact]
        public void AddUser_EmailIncorrectFormat_ThrowsError()
        {
            var user = GenerateUser.Generate();

            AdminPages.AddUser.EnterFirstName(user.FirstName);
            AdminPages.AddUser.EnterLastName(user.LastName);
            AdminPages.AddUser.EnterTelephoneNumber(user.TelephoneNumber);
            AdminPages.AddUser.EnterEmailAddress("test");

            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrganisationsController),
                nameof(OrganisationsController.AddUser))
                .Should()
                .BeTrue();

            CommonActions.ErrorSummaryDisplayed().Should().BeTrue();
            CommonActions.ErrorSummaryLinksExist().Should().BeTrue();

            CommonActions.ElementShowingCorrectErrorMessage(AddUserObjects.EmailError, EmailFormatIncorrect).Should().BeTrue();
        }

        [Fact]
        public async void AddUser_EmailAlreadyExists_ThrowsError()
        {
            var user = await AddUser();

            AdminPages.AddUser.EnterFirstName(user.FirstName);
            AdminPages.AddUser.EnterLastName(user.LastName);
            AdminPages.AddUser.EnterTelephoneNumber(user.PhoneNumber);
            AdminPages.AddUser.EnterEmailAddress(user.Email);

            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrganisationsController),
                nameof(OrganisationsController.AddUser))
                .Should()
                .BeTrue();

            CommonActions.ErrorSummaryDisplayed().Should().BeTrue();
            CommonActions.ErrorSummaryLinksExist().Should().BeTrue();

            CommonActions.ElementShowingCorrectErrorMessage(AddUserObjects.EmailError, EmailAlreadyExists).Should().BeTrue();
        }

        private async Task<AspNetUser> AddUser(bool isEnabled = true)
        {
            var user = GenerateUser.GenerateAspNetUser(OrganisationId, DefaultPassword, isEnabled);
            await using var context = GetEndToEndDbContext();
            context.Add(user);
            await context.SaveChangesAsync();
            Driver.Navigate().Refresh();

            return user;
        }
    }
}
