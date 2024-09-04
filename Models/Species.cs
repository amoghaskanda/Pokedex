using System.Text.RegularExpressions;

namespace Pokedex.Models
{
    public class Species
    {
        public string? Name { get; set; }
        public FlavorTextEntry?[] FlavorTextEntries
        {
            get { return FlavorTextEntries; }
            set { Description = Regex.Replace(value[0].FlavorText ?? string.Empty, @"[\x00-\x1F]", " "); }
        }

        public HabitatEntry? Habitat { get; set; }
        public string? Description { get; set; }
        public bool IsLegendary { get; set; } = false;
    }

    public class FlavorTextEntry
    {
        public string? FlavorText { get; set; }
    }

    public class HabitatEntry
    {
        public string? Name { get; set; }
    }
}
