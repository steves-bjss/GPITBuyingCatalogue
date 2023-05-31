using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Competitions.Models;

namespace NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;

public sealed class CompetitionCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        static ISpecimenBuilder ComposerTransformation(ICustomizationComposer<Competition> composer) => composer
            .Without(x => x.Organisation)
            .Without(x => x.LastUpdatedByUser)
            .Without(x => x.Filter);

        fixture.Customize<Competition>(ComposerTransformation);
    }
}