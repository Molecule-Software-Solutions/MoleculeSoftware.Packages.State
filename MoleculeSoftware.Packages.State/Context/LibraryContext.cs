using Realms;

namespace MoleculeSoftware.Packages.State
{
    internal static class LibraryContext
    {
        public static string Path { get; private set; }

        public static void Init(string databasePath)
        {
            Path = databasePath; 
        }

        public static Realm GetApplicationRealm()
        {
            if(string.IsNullOrWhiteSpace(Path))
            {
                throw new System.Exception("You have not initialized the database path properly"); 
            }

            // Note - default behavior is such that, if the file does not exist, then the file will be created
            // there is no need to determine if the file exists, or to create it, ahead of time

            return Realm.GetInstance(new ContextConfiguration(Path));
        }

        /// <summary>
        /// Removes an existing database file - Run this before executing the first migration
        /// </summary>
        public static void Cleanup()
        {
            try
            {
                if (System.IO.File.Exists(Path))
                {
                    System.IO.File.Delete(Path);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message); 
                throw;
            }
        }
    }
}
