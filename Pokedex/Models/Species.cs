using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Pokedex.Models
{
    public class Species
    {
        public string? Name { get; set; }
        private FlavorTextEntry[]? _flavorTextEntries = null;
        private const string regexPattern = @"[\x00-\x1F]";
        public FlavorTextEntry[]? FlavorTextEntries
        {
            get => _flavorTextEntries;
            set
            {
                if (value != null)
                    Description = Regex.Replace(value[0].FlavorText ?? string.Empty, regexPattern, " ");
            }
        }
        public HabitatEntry? Habitat { get; set; }
        public string? Description { get; set; }
        public bool IsLegendary { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Species))
                return false;

            var other = (Species)obj;

            return
                string.Equals(Name, other.Name) &&
                string.Equals(Description, other.Description) &&
                IsLegendary == other.IsLegendary &&
                (Habitat?.Equals(other.Habitat) ?? other.Habitat == null);
        }
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Name);
            hash.Add(Description);
            hash.Add(IsLegendary);
            hash.Add(Habitat);
            return hash.ToHashCode();
        }
    }


    public class FlavorTextEntry
    {
        public string? FlavorText { get; set; }
    }

    public class HabitatEntry
    {
        [JsonPropertyName("name")]
        public string? HabitatName { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(HabitatEntry))
                return false;
            var other = (HabitatEntry)obj;
            return other != null && string.Equals(HabitatName, other.HabitatName);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(HabitatName);
            return hash.ToHashCode();
        }
    }

}