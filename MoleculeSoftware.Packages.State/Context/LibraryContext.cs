using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MoleculeSoftware.Packages.State
{
    internal class LibraryContext : DbContext
    {
        public string DataSource { get; init; }

        public LibraryContext(string dataSource)
        {
            if(!string.IsNullOrWhiteSpace(dataSource))
            {
                DataSource = dataSource;
                return;
            }
            DataSource = System.IO.Path.Join(System.IO.Path.GetTempPath(), "MoleculeStateData.db"); 
        }

        // Database Sets
        internal DbSet<CacheItem> CacheItems { get; set; }

        // Context

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqliteConnectionStringBuilder csbuilder = new SqliteConnectionStringBuilder();
            csbuilder.DataSource = DataSource;
            csbuilder.ForeignKeys = true; 
            optionsBuilder.UseSqlite(csbuilder.ConnectionString); 
        }

        /// <summary>
        /// Removes an existing database file - Run this before executing the first migration
        /// </summary>
        public void Cleanup()
        {
            try
            {
                if (System.IO.File.Exists(DataSource))
                {
                    System.IO.File.Delete(DataSource);
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
