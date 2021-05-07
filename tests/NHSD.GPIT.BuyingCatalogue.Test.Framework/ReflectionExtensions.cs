using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;

namespace NHSD.GPIT.BuyingCatalogue.Test.Framework
{
    public static class ReflectionExtensions
    {
        public static T CopyObjectToNew<T>(this T input) where T : new()
        {
            var copy = new T();
            foreach (var item in input.GetType().GetProperties())
            {
                copy.GetType().GetProperty(item.Name).SetValue(copy,
                    item.GetValue(input, null), null);
            }

            return copy;
        }

        public static void ValidateAllPropertiesExcept<T>(this T input, T toCheck, string[] propertiesToExclude)
        {
            var remainingProperties = input.GetType().GetProperties()
                .Where(x => !propertiesToExclude.Contains(x.Name));

            foreach (var propertyInfo in remainingProperties)
            {
                input.GetType().GetProperty(propertyInfo.Name).GetValue(toCheck)
                    .Should()
                    .BeEquivalentTo(typeof(ClientApplication).GetProperty(propertyInfo.Name).GetValue(input));
            }
        }
    }
}