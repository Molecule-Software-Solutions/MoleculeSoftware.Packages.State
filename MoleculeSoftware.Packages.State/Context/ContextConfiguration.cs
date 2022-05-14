using Realms;

namespace MoleculeSoftware.Packages.State
{
    internal class ContextConfiguration : RealmConfiguration
    {
        public ContextConfiguration(string path)
        {
            DatabasePath = path;
            MigrationCallback = (migration, oldSchemaVersion) =>
            {
                // Place migrations here
            }; 
        }
    }
}
