namespace Pokedex.Models
{
    public class Pokemon
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Habitat { get; set; }
        public bool? IsLegendary { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Pokemon))
                return false;
            var other = (Pokemon)obj;
            return
                string.Equals(Name, other.Name) &&
                string.Equals(Description, other.Description) &&
                string.Equals(Habitat, other.Habitat) &&
                string.Equals(IsLegendary, other.IsLegendary);
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
}
