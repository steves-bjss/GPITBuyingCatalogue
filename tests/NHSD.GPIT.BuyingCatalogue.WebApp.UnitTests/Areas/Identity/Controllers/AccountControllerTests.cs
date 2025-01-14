﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Users.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Constants;
using NHSD.GPIT.BuyingCatalogue.Framework.Identity;
using NHSD.GPIT.BuyingCatalogue.Framework.Settings;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Identity;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Identity.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Identity.Models;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Identity.Controllers
{
    public static class AccountControllerTests
    {
        [Fact]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(AccountController).Should().BeDecoratedWith<AreaAttribute>(x => x.RouteValue == "Identity");
            typeof(AccountController).Should().BeDecoratedWith<RouteAttribute>(r => r.Template == "Identity/Account");
        }

        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(AccountController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [MockAutoData]
        public static void Get_Login_ReturnsDefaultViewWithReturnUrlSet(
            AccountController controller)
        {
            var result = controller.Login("ReturnLink");
            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();

            var model = actualResult.Model.Should().BeAssignableTo<LoginViewModel>().Subject;

            model.ReturnUrl.Should().Be("ReturnLink");
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_InvalidModelState_ReturnsDefaultView(
            AccountController controller)
        {
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.Login(new LoginViewModel());
            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<LoginViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_UserNotFound_ReturnsDefaultView(
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager)
        {
            const string expectedErrorMessage = "The username or password was not recognised. Try again or contact your administrator.";

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns((AspNetUser)null);

            var controller = CreateController(mockUserManager, mockSignInManager);

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);

            controller.ModelState.Should().ContainKey(nameof(model.EmailAddress));
            controller.ModelState[nameof(model.EmailAddress)]?.Errors.Should().Contain(x => x.ErrorMessage == expectedErrorMessage);

            controller.ModelState.Should().ContainKey(nameof(model.Password));
            controller.ModelState[nameof(model.Password)]?.Errors.Should().Contain(x => x.ErrorMessage == expectedErrorMessage);

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<LoginViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_FailedSignIn_ReturnsDefaultView(
            AspNetUser user,
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager)
        {
            const string expectedErrorMessage = "The username or password was not recognised. Try again or contact your administrator.";

            user.Disabled = false;

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns(user);

            mockSignInManager
                .PasswordSignInAsync(user, model.Password, false, true)
                .Returns(SignInResult.Failed);

            var controller = CreateController(mockUserManager, mockSignInManager);

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);
            await mockSignInManager.Received().PasswordSignInAsync(user, model.Password, false, true);

            controller.ModelState.Should().ContainKey(nameof(model.EmailAddress));
            controller.ModelState[nameof(model.EmailAddress)]?.Errors.Should().Contain(x => x.ErrorMessage == expectedErrorMessage);

            controller.ModelState.Should().ContainKey(nameof(model.Password));
            controller.ModelState[nameof(model.Password)]?.Errors.Should().Contain(x => x.ErrorMessage == expectedErrorMessage);

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<LoginViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_UserAccountDisabled_ReturnsDefaultView(
            AspNetUser user,
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager)
        {
            const string expectedErrorMessage = "There is a problem accessing your account.";

            user.Disabled = true;

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns(user);

            var controller = CreateController(mockUserManager, mockSignInManager);

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);

            controller.ModelState.Should().ContainKey(nameof(model.DisabledError));
            controller.ModelState[nameof(model.DisabledError)]?.Errors.Should().Contain(x => x.ErrorMessage.Contains(expectedErrorMessage));

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<LoginViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_UserAccountLocked_ReturnsView(
            AspNetUser user,
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IUrlHelper mockUrlHelper)
        {
            user.Disabled = false;

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns(user);

            mockSignInManager
                .PasswordSignInAsync(user, model.Password, false, true)
                .Returns(SignInResult.LockedOut);

            var controller = CreateController(mockUserManager, mockSignInManager);

            controller.Url = mockUrlHelper;

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);
            await mockSignInManager.Received().PasswordSignInAsync(user, model.Password, false, true);

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.Should().NotBeNull();
            actualResult.Model.Should().BeOfType<LoginViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_UserAccountActive_ReturnsRedirect(
            string odsCode,
            AspNetUser user,
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IOdsService mockOdsService)
        {
            user.Disabled = false;
            user.PrimaryOrganisation = new Organisation { ExternalIdentifier = odsCode };

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns(user);

            mockSignInManager
                .PasswordSignInAsync(user, model.Password, false, true)
                .Returns(SignInResult.Success);

            var controller = CreateController(mockUserManager, mockSignInManager, odsService: mockOdsService);

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);
            await mockSignInManager.Received().PasswordSignInAsync(user, model.Password, false, true);
            await mockOdsService.Received().UpdateOrganisationDetails(odsCode);

            var actualResult = result.Should().BeAssignableTo<RedirectResult>().Subject;

            actualResult.Url.Should().Be(model.ReturnUrl);
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_NoReturnUrl_NotAdminUser_ReturnsRedirectToHome(
            string odsCode,
            AspNetUser user,
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IOdsService mockOdsService,
            IUrlHelper mockUrlHelper)
        {
            const string userName = "Name";
            const string url = "~/";

            model.ReturnUrl = string.Empty;

            user.Disabled = false;
            user.PrimaryOrganisation = new Organisation { ExternalIdentifier = odsCode };

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns(user);

            mockUserManager
                .IsInRoleAsync(user, OrganisationFunction.Authority.Name)
                .Returns(false);

            mockSignInManager
                .PasswordSignInAsync(user, model.Password, false, true)
                .Returns(SignInResult.Success);

            mockUrlHelper
                .Action(Arg.Any<UrlActionContext>())
                .Returns(url);

            var userPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new(ClaimTypes.Name, userName),
                        new(CatalogueClaims.PrimaryOrganisationInternalIdentifier, odsCode),
                    },
                    "mock"));
            var controller = CreateController(mockUserManager, mockSignInManager, odsService: mockOdsService);

            controller.Url = mockUrlHelper;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal, },
            };

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);
            await mockUserManager.Received().IsInRoleAsync(user, OrganisationFunction.Authority.Name);
            await mockSignInManager.Received().PasswordSignInAsync(user, model.Password, false, true);
            await mockOdsService.Received().UpdateOrganisationDetails(odsCode);

            var actualResult = result.Should().BeAssignableTo<RedirectResult>().Subject;

            actualResult.Url.Should().Be("~/");
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_Login_NoReturnUrl_AdminUser_ReturnsRedirectToHome(
            string odsCode,
            AspNetUser user,
            LoginViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IOdsService mockOdsService,
            IUrlHelper mockUrlHelper,
            string adminUrl)
        {
            model.ReturnUrl = string.Empty;

            user.Disabled = false;
            user.PrimaryOrganisation = new Organisation { ExternalIdentifier = odsCode };

            mockUserManager
                .FindByNameAsync(model.EmailAddress)
                .Returns(user);

            mockUserManager
                .IsInRoleAsync(user, OrganisationFunction.Authority.Name)
                .Returns(true);

            mockSignInManager
                .PasswordSignInAsync(user, model.Password, false, true)
                .Returns(SignInResult.Success);

            mockUrlHelper.Action(Arg.Any<UrlActionContext>()).Returns(adminUrl);

            var controller = CreateController(mockUserManager, mockSignInManager, odsService: mockOdsService);
            controller.Url = mockUrlHelper;

            var result = await controller.Login(model);

            await mockUserManager.Received().FindByNameAsync(model.EmailAddress);
            await mockUserManager.Received().IsInRoleAsync(user, OrganisationFunction.Authority.Name);
            await mockSignInManager.Received().PasswordSignInAsync(user, model.Password, false, true);
            await mockOdsService.Received().UpdateOrganisationDetails(odsCode);

            var actualResult = result.Should().BeAssignableTo<RedirectResult>().Subject;

            actualResult.Url.Should().Be(adminUrl);
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_Logout_WhenNotLoggedIn_RedirectsHome(AccountController controller)
        {
            var result = await controller.Logout();

            Assert.IsAssignableFrom<LocalRedirectResult>(result);
            Assert.Equal("~/", ((LocalRedirectResult)result).Url);
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_Logout_WhenLoggedIn_SignsOut_RedirectsHome(
            string userName,
            AspNetUser user,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager)
        {
            mockUserManager
                .FindByNameAsync(userName)
                .Returns(user);

            mockSignInManager
                .IsSignedIn(Arg.Any<ClaimsPrincipal>())
                .Returns(true);

            mockSignInManager
                .SignOutAsync()
                .Returns(Task.CompletedTask);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new(ClaimTypes.Name, userName) },
                "mock"));

            var controller = CreateController(
                mockUserManager,
                mockSignInManager);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal, },
            };
            var result = await controller.Logout();

            await mockUserManager
                .Received()
                .FindByNameAsync(userName);

            await mockUserManager
                .Received()
                .UpdateSecurityStampAsync(user);

            mockSignInManager
                .Received()
                .IsSignedIn(Arg.Any<ClaimsPrincipal>());

            await mockSignInManager
                .Received()
                .SignOutAsync();

            Assert.IsAssignableFrom<LocalRedirectResult>(result);
            Assert.Equal("~/", ((LocalRedirectResult)result).Url);
        }

        [Theory]
        [MockAutoData]
        public static void Get_ForgotPassword_ReturnsDefaultView(AccountController controller)
        {
            var result = controller.ForgotPassword();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_ForgotPassword_InvalidModelState_ReturnsDefaultView(AccountController controller)
        {
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.ForgotPassword(new ForgotPasswordViewModel());

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
            Assert.IsAssignableFrom<ForgotPasswordViewModel>(((ViewResult)result).Model);
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_ForgotPassword_ValidModelState_ReturnsRedirect(
            PasswordResetToken token,
            Uri uri,
            ForgotPasswordViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IPasswordService mockPasswordService,
            IPasswordResetCallback mockPasswordResetCallback)
        {
            mockPasswordService
                .GeneratePasswordResetTokenAsync(model.EmailAddress)
                .Returns(token);

            mockPasswordResetCallback
                .GetPasswordResetCallback(token)
                .Returns(uri);

            mockPasswordService
                .SendResetEmailAsync(token.User, uri)
                .Returns(Task.CompletedTask);

            var controller = CreateController(mockUserManager, mockSignInManager, passwordService: mockPasswordService, passwordResetCallback: mockPasswordResetCallback);

            var result = await controller.ForgotPassword(model);

            await mockPasswordService
                .Received()
                .GeneratePasswordResetTokenAsync(model.EmailAddress);

            mockPasswordResetCallback
                .Received()
                .GetPasswordResetCallback(token);

            await mockPasswordService
                .Received()
                .SendResetEmailAsync(token.User, uri);

            var actualResult = result.Should().BeAssignableTo<RedirectToActionResult>();

            actualResult.Subject.ActionName.Should().Be(nameof(AccountController.ForgotPasswordLinkSent));
        }

        [Theory]
        [MockAutoData]
        public static void Get_ForgotPasswordLinkSent_ReturnsDefaultView(AccountController controller)
        {
            var result = controller.ForgotPasswordLinkSent();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Theory]
        [MockAutoData]
        public static async void Get_ResetPassword_ReturnsDefaultView(
            string email,
            string token,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IPasswordService mockPasswordService)
        {
            mockPasswordService
                .IsValidPasswordResetTokenAsync(email, token)
                .Returns(true);

            var controller = CreateController(mockUserManager, mockSignInManager, passwordService: mockPasswordService);

            var result = await controller.ResetPassword(email, token);

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<ResetPasswordViewModel>();
            Assert.NotNull(actualResult.Model);
            Assert.Equal(email, ((ResetPasswordViewModel)actualResult.Model).Email);
            Assert.Equal(token, ((ResetPasswordViewModel)actualResult.Model).Token);
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_ResetPassword_InvalidModelState_ReturnsDefaultView(AccountController controller)
        {
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.ResetPassword(new ResetPasswordViewModel());

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
            Assert.IsAssignableFrom<ResetPasswordViewModel>(((ViewResult)result).Model);
        }

        [Theory]
        [MockAutoData]
        public static async Task PostResetPassword_WithPreviouslyUsedPassword_ReturnsModelError(
            ResetPasswordViewModel model,
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            IPasswordService mockPasswordService)
        {
            IdentityError[] expectedErrorMessages = new IdentityError[]
            {
                new IdentityError() { Code = PasswordValidator.PasswordAlreadyUsedCode, Description = PasswordValidator.PasswordAlreadyUsed },
            };

            mockPasswordService
                .ResetPasswordAsync(model.Email, model.Token, model.Password)
                .Returns(IdentityResult.Failed(expectedErrorMessages));

            var controller = CreateController(mockUserManager, mockSignInManager, mockPasswordService);

            var result = await controller.ResetPassword(model);

            controller.ModelState.Should().ContainKey(nameof(ResetPasswordViewModel.Password));
            controller.ModelState[nameof(ResetPasswordViewModel.Password)]?.Errors.Should().Contain(x => x.ErrorMessage.Contains(PasswordValidator.PasswordAlreadyUsed));

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<ResetPasswordViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task PostResetPassword_InvalidPasswordError_ReturnsModelError(
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            ResetPasswordViewModel model,
            IPasswordService mockPasswordService)
        {
            IdentityError[] expectedErrorMessages = new IdentityError[]
            {
                new IdentityError() { Code = PasswordValidator.InvalidPasswordCode, Description = PasswordValidator.PasswordConditionsNotMet },
            };

            mockPasswordService
                .ResetPasswordAsync(model.Email, model.Token, model.Password)
                .Returns(IdentityResult.Failed(expectedErrorMessages));

            var controller = CreateController(
                mockUserManager,
                mockSignInManager,
                passwordService: mockPasswordService);

            var result = await controller.ResetPassword(model);

            await mockPasswordService
                .Received()
                .ResetPasswordAsync(model.Email, model.Token, model.Password);

            controller.ModelState.Should().ContainKey(nameof(ResetPasswordViewModel.Password));
            controller.ModelState[nameof(ResetPasswordViewModel.Password)]?.Errors.Should().Contain(x => x.ErrorMessage.Contains(PasswordValidator.PasswordConditionsNotMet));

            var actualResult = result.Should().BeAssignableTo<ViewResult>().Subject;

            actualResult.ViewName.Should().BeNull();
            actualResult.Model.Should().BeAssignableTo<ResetPasswordViewModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task PostResetPassword_InvalidTokenError_ReturnsRedirectToAction(
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            ResetPasswordViewModel model,
            IPasswordService mockPasswordService)
        {
            IdentityError[] expectedErrorMessages = new IdentityError[]
            {
                new IdentityError() { Code = IPasswordService.InvalidTokenCode },
            };

            mockPasswordService
                .ResetPasswordAsync(model.Email, model.Token, model.Password)
                .Returns(IdentityResult.Failed(expectedErrorMessages));

            var controller = CreateController(
                mockUserManager,
                mockSignInManager,
                passwordService: mockPasswordService);

            var result = await controller.ResetPassword(model);

            await mockPasswordService
                .Received()
                .ResetPasswordAsync(model.Email, model.Token, model.Password);

            var actualResult = result.Should().BeAssignableTo<RedirectToActionResult>().Subject;
            actualResult.ActionName.Should().Be(nameof(AccountController.ResetPasswordExpired));
        }

        [Theory]
        [MockAutoData]
        public static async Task PostResetPassword_ResetSuccess_ReturnsRedirectToAction(
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            ResetPasswordViewModel model,
            IPasswordService mockPasswordService)
        {
            mockPasswordService
                .ResetPasswordAsync(model.Email, model.Token, model.Password)
                .Returns(IdentityResult.Success);

            var controller = CreateController(
                mockUserManager,
                mockSignInManager,
                passwordService: mockPasswordService);

            var result = await controller.ResetPassword(model);

            await mockPasswordService
                .Received()
                .ResetPasswordAsync(model.Email, model.Token, model.Password);

            var actualResult = result.Should().BeAssignableTo<RedirectToActionResult>().Subject;
            actualResult.ActionName.Should().Be(nameof(AccountController.ResetPasswordConfirmation));
        }

        [Theory]
        [MockAutoData]
        public static async Task PostResetPassword_UnexpectedErrors_ThrowsException(
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            ResetPasswordViewModel model,
            IPasswordService mockPasswordService)
        {
            const string code = "Code";
            const string errorMessage = "Error";

            IdentityError[] expectedErrorMessages = new IdentityError[] { new IdentityError() { Code = code, Description = errorMessage }, };

            mockPasswordService
                .ResetPasswordAsync(model.Email, model.Token, model.Password)
                .Returns(IdentityResult.Failed(expectedErrorMessages));

            var controller = CreateController(
                mockUserManager,
                mockSignInManager,
                passwordService: mockPasswordService);

            var result = Assert.ThrowsAsync<InvalidOperationException>(async () => await controller.ResetPassword(model));
            result.Result.Message.Should().Be("Unexpected errors whilst resetting password: " + errorMessage);

            await mockPasswordService
                .Received()
                .ResetPasswordAsync(model.Email, model.Token, model.Password);
        }

        [Theory]
        [MockAutoData]
        public static void Get_ResetPasswordConfirmation_ReturnsDefaultView(AccountController controller)
        {
            var result = controller.ResetPasswordConfirmation();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Theory]
        [MockAutoData]
        public static void Get_ResetPasswordExpired_ReturnsDefaultView(AccountController controller)
        {
            var result = controller.ResetPasswordExpired();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Theory]
        [MockAutoData]
        public static void Get_UpdatePassword_ReturnsDefaultView(AccountController controller)
        {
            var result = controller.UpdatePassword();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_UpdatePassword_InvalidModelState_ReturnsDefaultView(AccountController controller)
        {
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.UpdatePassword(new UpdatePasswordViewModel());

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
            Assert.IsAssignableFrom<UpdatePasswordViewModel>(((ViewResult)result).Model);
        }

        [Theory]
        [MockAutoData]
        public static void UpdatePassword_UnexpectedErrors_ThrowsException(
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            UpdatePasswordViewModel model,
            IPasswordService mockPasswordService)
        {
            const string userName = "Name";
            const string code = "Code";
            const string errorMessage = "Error";

            IdentityError[] expectedErrorMessages = new IdentityError[] { new IdentityError() { Code = code, Description = errorMessage }, };

            mockPasswordService
                .ChangePasswordAsync(userName, model.CurrentPassword, model.NewPassword)
                .Returns(IdentityResult.Failed(expectedErrorMessages));

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new(ClaimTypes.Name, userName) },
                "mock"));

            var controller = CreateController(
                mockUserManager,
                mockSignInManager,
                mockPasswordService);

            controller.ObjectValidator = Substitute.For<IObjectModelValidator>();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal, },
            };

            var result = Assert.ThrowsAsync<InvalidOperationException>(async () => await controller.UpdatePassword(model));
            result.Result.Message.Should().Be("Unexpected errors whilst updating password: " + errorMessage);
        }

        [Theory]
        [MockAutoData]
        public static async Task UpdatePassword_PasswordUpdated_ReturnsRedirect(
            UserManager<AspNetUser> mockUserManager,
            SignInManager<AspNetUser> mockSignInManager,
            UpdatePasswordViewModel model,
            IPasswordService mockPasswordService)
        {
            const string userName = "Name";

            mockPasswordService
                .ChangePasswordAsync(userName, model.CurrentPassword, model.NewPassword)
                .Returns(IdentityResult.Success);

            mockSignInManager
                .SignOutAsync()
                .Returns(Task.CompletedTask);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new(ClaimTypes.Name, userName) },
                "mock"));

            var controller = CreateController(
                mockUserManager,
                mockSignInManager,
                mockPasswordService);

            controller.ObjectValidator = Substitute.For<IObjectModelValidator>(); // mockValidator;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal, },
            };

            var result = await controller.UpdatePassword(model);

            await mockPasswordService
                .Received()
                .ChangePasswordAsync(userName, model.CurrentPassword, model.NewPassword);

            await mockSignInManager
                .Received()
                .SignOutAsync();

            var actualResult = result.Should().BeAssignableTo<RedirectToActionResult>().Subject;
            actualResult.ActionName.Should().Be(nameof(AccountController.Login));
        }

        private static AccountController CreateController(
            UserManager<AspNetUser> userManager,
            SignInManager<AspNetUser> signInManager,
            IPasswordService passwordService = null,
            IOdsService odsService = null,
            IPasswordResetCallback passwordResetCallback = null)
        {
            return new AccountController(
                signInManager,
                userManager,
                odsService ?? Substitute.For<IOdsService>(),
                passwordService ?? Substitute.For<IPasswordService>(),
                passwordResetCallback ?? Substitute.For<IPasswordResetCallback>(),
                new DisabledErrorMessageSettings(),
                new PasswordSettings());
        }
    }
}
