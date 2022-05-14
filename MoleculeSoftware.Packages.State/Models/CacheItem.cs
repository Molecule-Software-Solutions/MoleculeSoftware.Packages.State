using Realms;
using MongoDB.Bson; 

namespace MoleculeSoftware.Packages.State
{
    public class CacheItem : RealmObject
    {
        [PrimaryKey]
        public int ID { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
