﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.ViewsTagHelpers
{
    [HtmlTargetElement(TagHelperName)]
    [RestrictChildren(CheckBoxContainerTagName, RadioButtonTagName)]
    public sealed class FieldSetFormTagHelper : TagHelper
    {
        public const string TagHelperName = "nhs-fieldset-form";
        public const string CheckBoxContainerTagName = CheckboxContainerTagHelper.TagHelperName;
        public const string RadioButtonTagName = RadioButtonsTagHelper.TagHelperName;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(TagHelperConstants.For)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(TagHelperConstants.LabelTextName)]
        public string LabelText { get; set; }

        [HtmlAttributeName(TagHelperConstants.LabelHintName)]
        public string LabelHint { get; set; }

        [HtmlAttributeName(TagHelperConstants.DisableLabelAndHint)]
        public bool? DisableLabelAndHint { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string formName = GetModelKebabNameFromFor();

            var formGroup = TagHelperBuilders.GetFormGroupBuilder();
            var fieldset = GetFieldSetLegendHeadingBuilder(formName);
            var hint = TagHelperBuilders.GetLabelHintBuilder(For, LabelHint, formName, DisableLabelAndHint);

            var content = await output.GetChildContentAsync();

            formGroup.InnerHtml.AppendHtml(fieldset);
            formGroup.InnerHtml.AppendHtml(hint);
            formGroup.InnerHtml.AppendHtml(content);

            TagHelperBuilders.UpdateOutputDiv(output, null, ViewContext, formGroup, true, formName);
        }

        private static TagBuilder GetFieldsetBuilder(string formName)
        {
            var builder = new TagBuilder(TagHelperConstants.FieldSet);

            builder.AddCssClass(TagHelperConstants.NhsFieldset);
            builder.MergeAttribute(TagHelperConstants.AriaDescribedBy, $"{formName}-hint");

            return builder;
        }

        private static TagBuilder GetFieldsetLegendBuilder()
        {
            var builder = new TagBuilder(TagHelperConstants.Legend);

            builder.AddCssClass(TagHelperConstants.NhsFieldsetLegend);
            builder.AddCssClass(TagHelperConstants.NhsFieldsetLegendOne);

            return builder;
        }

        private TagBuilder GetFieldSetLegendHeadingBuilder(string formName)
        {
            var fieldset = GetFieldsetBuilder(formName);
            var fieldsetLegend = GetFieldsetLegendBuilder();
            var fieldsetlegendheader = GetFieldsetLegendHeadingTagBuilder();

            fieldsetLegend.InnerHtml.AppendHtml(fieldsetlegendheader);
            fieldset.InnerHtml.AppendHtml(fieldsetLegend);

            return fieldset;
        }

        private TagBuilder GetFieldsetLegendHeadingTagBuilder()
        {
            if (LabelText == null || DisableLabelAndHint == true)
                return new TagBuilder("empty");

            var builder = new TagBuilder(TagHelperConstants.H1);

            builder.AddCssClass(TagHelperConstants.NhsFieldSetLegendHeading);

            builder.InnerHtml.Append(LabelText);

            return builder;
        }

        private string GetModelKebabNameFromFor()
        {
            string name = For.Model.GetType().Name;
            name = name.Remove(name.Length - 5);

            var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            return string.Join("-", pattern.Matches(name)).ToLower();
        }
    }
}