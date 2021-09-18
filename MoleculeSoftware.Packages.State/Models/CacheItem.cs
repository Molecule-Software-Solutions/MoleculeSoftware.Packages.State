using System.ComponentModel.DataAnnotations;

namespace MoleculeSoftware.Packages.State
{
    public class CacheItem
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
