﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models.Filters;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Solutions.Models.Filters
{
    public static class FilterEpicsModelTests
    {
        [Theory]
        [MockAutoData]
        public static void Constructor_PropertiesAreSetCorrectly(
            List<Capability> capabilities,
            Epic epic)
        {
            epic.Capabilities.Clear();

            for (var i = 0; i < capabilities.Count; i++)
            {
                epic.Capabilities.Add(capabilities[i]);
            }

            var model = new FilterEpicsModel(capabilities, new List<Epic> { epic });

            model.EpicGroups.Should().BeEquivalentTo(capabilities.Select(x => new { x.Id, x.Name }));
            model.EpicSelectionItems.Should().BeEquivalentTo(capabilities.Select(x => new SelectionModel
            {
                Id = $"{x.Id},{epic.Id}",
                Selected = false,
            }));
        }

        [Theory]
        [MockAutoData]
        public static void Constructor_WithSelectedIds_PropertiesAreSetCorrectly(
            List<Capability> capabilities,
            Epic epic)
        {
            for (var i = 0; i < capabilities.Count; i++)
            {
                epic.Capabilities.Add(capabilities[i]);
            }

            var selected = new Dictionary<int, string[]>
            {
                { capabilities[0].Id, new string[] { epic.Id } },
            };

            var model = new FilterEpicsModel(capabilities, new List<Epic> { epic }, selected);

            model.EpicGroups.Should().BeEquivalentTo(capabilities.Select(x => new { x.Id, x.Name }));
            model.EpicSelectionItems.Count().Should().Be(capabilities.Count);
            model.EpicSelectionItems.Where(s => s.Selected).Should().BeEquivalentTo(new[]
            {
                new SelectionModel
                {
                    Id = $"{capabilities[0].Id},{epic.Id}",
                    Selected = true,
                },
            });
        }

        [Theory]
        [MockAutoData]
        public static void Items_ReturnsExpectedResult(
            List<Capability> capabilities,
            List<Epic> epics)
        {
            for (var i = 0; i < capabilities.Count; i++)
            {
                epics[i].Capabilities.Add(capabilities[i]);
            }

            var model = new FilterEpicsModel(capabilities, epics);

            foreach (var capability in capabilities)
            {
                var expected = epics
                    .Where(x => x.Capabilities.Any(y => y.Id == capability.Id))
                    .Select(x => new { x.Id, x.Name });

                model.Items(capability.Id).Should().BeEquivalentTo(expected);
            }
        }
    }
}
