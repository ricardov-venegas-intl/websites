using Markdig;
using Markdown.ColorCode;

namespace VenegasIntl.PowershellKitchen.Helpers
{
    public static class MarkdownToHtml
    {
        private static MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
			    .UseColorCode()
				.Build();
        public static string Convert(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown, _pipeline);

        }
    }
}