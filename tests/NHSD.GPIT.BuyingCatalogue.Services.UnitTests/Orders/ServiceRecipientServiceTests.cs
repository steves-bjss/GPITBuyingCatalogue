﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using NHSD.GPIT.BuyingCatalogue.Services.Orders;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.Services.UnitTests.Orders
{
    public static class ServiceRecipientServiceTests
    {
        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(ServiceRecipientService).GetConstructors();

            assertion.Verify(constructors);
        }
    }
}
