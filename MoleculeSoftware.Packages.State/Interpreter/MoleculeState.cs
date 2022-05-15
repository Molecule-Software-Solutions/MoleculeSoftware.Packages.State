using System;
using System.Collections.Generic;
using System.Linq;

namespace MoleculeSoftware.Packages.State
{
    public class MoleculeState
    {
        private string m_DatabasePath; 
        private bool m_Initialized; 

        public string DatabaseOperation(string operationString)
        {
            if (!m_Initialized)
                throw new Exception("The caching database has not been initialized. You must call the Init()/Init(databaseFileName) method before performing any database operations"); 
            return ParseOperationString(operationString);
        }


        /// <summary>
        /// Initializes the database and ensures that any migrations are performed
        /// NOTE: Deletes all previous instances of the caching database matching the <see cref="databaseFileName"/>
        /// </summary>
        /// <param name="databasePath"></param>
        public void Init(string databasePath)
        {
            m_DatabasePath = databasePath;
            LibraryContext.Init(m_DatabasePath); 
            LibraryContext.Cleanup(); 
            m_Initialized = true; 
        }

        // Parses the operation string and performs commands based upon the command structure
        private string ParseOperationString(string operationString)
        {
            // Guard
            if (operationString.Length == 0)
                return "#NONE#";

            try
            {
                string[] parseData = operationString.Split("#>");
                switch (parseData[0])
                {
                    case "STORE":
                        {
                            if (StoreValue(parseData[1], parseData[2]))
                            {
                                return "#OK#";
                            }
                            else return "#NONE#";
                        }
                    case "RETRIEVE":
                        {
                            return RetrieveValue(parseData[1]); 
                        }
                    case "UPDATE":
                        {
                            if (UpdateData(parseData[1], parseData[2]))
                            {
                                return "#OK#";
                            }
                            return "#NONE#"; 
                        }
                    case "DELETE":
                        {
                            if (DeleteValue(parseData[1]))
                            {
                                return "#OK#";
                            }
                            else return "#NONE#"; 
                        }
                    case "PURGE":
                        {
                            if (parseData[1] == "1")
                            {
                                if (PurgeData())
                                {
                                    return "#OK#";
                                }
                                else return "#NONE#";
                            }
                            else return "#NONE#"; 
                        }
                    default:
                        return "#NONE#";
                }
            }
            catch (Exception)
            {
                return "#NONE#";
                throw; 
            }
        }

        private bool UpdateData(string key, string value)
        {
            var context = LibraryContext.GetApplicationRealm(); 

            try
            {
                var searchResult = context.All<CacheItem>().FirstOrDefault(c => c.Key == key);
                if (searchResult is null)
                    return false;
                context.Write(() =>
                {
                    searchResult.Value = value;
                }); 
                return true; 
            }
            catch (Exception)
            {
                return false;
                throw; 
            }
        }

        private bool PurgeData()
        {
            var context = LibraryContext.GetApplicationRealm();

            try
            {
                List<CacheItem> itemsList = new List<CacheItem>(context.All<CacheItem>());
                context.Write(() =>
                {
                    foreach (CacheItem item in itemsList)
                    {
                        context.Remove(item);
                    }
                }); 
                return true;

            }
            catch (Exception)
            {
                return false;
                throw; 
            }
        }

        private bool DeleteValue(string key)
        {
            // Guard
            if(string.IsNullOrEmpty(key))
            {
                return false; 
            }

            var context = LibraryContext.GetApplicationRealm();

            try
            {
                var result = context.All<CacheItem>().FirstOrDefault(c => c.Key == key);
                if (result is null)
                    return false;

                context.Write(() =>
                {
                    context.Remove(result);
                }); 
                return true;
            }
            catch (Exception)
            {
                return false;
                throw; 
            }
        }

        private string RetrieveValue(string key)
        {
            // Guard
            if(string.IsNullOrEmpty(key))
            {
                return "#NONE#"; 
            }

            var context = LibraryContext.GetApplicationRealm();

            try
            {
                var result = context.All<CacheItem>().FirstOrDefault(c => c.Key == key);
                if (result is null)
                    return "#NONE#";
                else return result.Value; 
            }
            catch (Exception) 
            { 
                return "#NONE#";
                throw; 
            }
        }

        private bool StoreValue(string key, string value)
        {
            // Guard
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;
            var context = LibraryContext.GetApplicationRealm(); 

            try
            {
                context.Write(() =>
                {
                    context.Add(new CacheItem()
                    {
                        Key = key,
                        Value = value
                    });
                });
                return true;
            }
            catch (Exception)
            {
                return false;
                throw; 
            }
        }
    }
}
