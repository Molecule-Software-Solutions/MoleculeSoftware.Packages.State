using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoleculeSoftware.Packages.State
{
    public class MoleculeState
    {
        public string DatabaseOperation(string operationString)
        {
            return ParseOperationString(operationString);
        }

        /// <summary>
        /// Initializes the database and deletes any existing state database files
        /// </summary>
        public void Init()
        {
            LibraryContext context = new LibraryContext();
            context.Cleanup(); 
            context.Database.Migrate();
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
            }
        }

        private bool UpdateData(string key, string value)
        {
            using (LibraryContext context = new LibraryContext())
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var searchResult = context.CacheItems.FirstOrDefault(c => c.Key == key);
                        if (searchResult is null)
                            return false;
                        searchResult.Value = value;
                        context.Entry(searchResult).State = EntityState.Modified;
                        context.SaveChanges();
                        transaction.Commit();
                        return true; 
                    }
                    catch (Exception)
                    {
                        return false; 
                    }
                }
            }
        }

        private bool PurgeData()
        {
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        List<CacheItem> itemsList = new List<CacheItem>(context.CacheItems);
                        foreach (CacheItem item in itemsList)
                        {
                            context.Remove(item);
                        }
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DeleteValue(string key)
        {
            // Guard
            if(string.IsNullOrEmpty(key))
            {
                return false; 
            }

            using (LibraryContext context = new LibraryContext())
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var result = context.CacheItems.FirstOrDefault(c => c.Key == key);
                        if (result is null)
                            return false;

                        context.Remove(result);
                        context.SaveChanges();
                        transaction.Commit(); 
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private string RetrieveValue(string key)
        {
            // Guard
            if(string.IsNullOrEmpty(key))
            {
                return "#NONE#"; 
            }

            using (LibraryContext context = new LibraryContext())
            {
                var result = context.CacheItems.FirstOrDefault(c => c.Key == key);
                if (result is null)
                    return "#NONE#";
                else return result.Value; 
            }
        }

        private bool StoreValue(string key, string value)
        {
            // Guard
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false; 

            using (LibraryContext context = new LibraryContext())
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Add(new CacheItem()
                        {
                            Key = key,
                            Value = value
                        });
                        context.SaveChanges(); 
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false; 
                    }
                }
            }
        }
    }
}
