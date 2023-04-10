using VenegasIntl.PowershellKitchen.Models;

namespace VenegasIntl.PowershellKitchen.Repositories
{
    public class PowershellKitchenBlogRepository
    {
        readonly Configuration _configuration;

        public PowershellKitchenBlogRepository(Configuration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerable<BlogEntry> ReadAllBlogEntries()
        {
            foreach (var file in Directory.EnumerateFiles(_configuration.PowershellBlogEntriesPath))
            {
                yield return ParseBlogEntry(file);
            }
        }

        public BlogEntry ReadBlogEntry(string id)
        {
            foreach (var file in Directory.EnumerateFiles(_configuration.PowershellBlogEntriesPath))
            {
                if (Path.GetFileNameWithoutExtension(file) == id)
                {
                    return ParseBlogEntry(file);
                }
            }
            throw new FileNotFoundException();
        }

        private BlogEntry ParseBlogEntry(string filePath)
        {
            var allLines = File.ReadAllLines(filePath);
            var titleLine = "";
            var summary = "";
            int count = 0;
            var titleLineNumber = 0;
            while (count < allLines.Length)
            {
                if (allLines[count].StartsWith("# "))
                {
                    titleLine = allLines[count].Substring(2);
                    titleLineNumber = count;
                    count++;
                    break;
                }
                count++;
            }
            while (count < allLines.Length)
            {
                if (allLines[count].StartsWith("#") == false)
                {
                    summary = summary + Environment.NewLine + allLines[count];
                }
                else
                {
                    break;
                }
                count++;
            }
            return new BlogEntry
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Title = titleLine,
                Summary = summary,
                Content = string.Join(Environment.NewLine, allLines.Skip(titleLineNumber + 1))
            };
        }
    }
}
