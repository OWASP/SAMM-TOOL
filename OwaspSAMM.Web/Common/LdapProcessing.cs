using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Text;
using System.Threading.Tasks;

namespace OwaspSAMM.Web
{
    public class LdapProcessing
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string ADCommonNameProperty = "cn";
        private static string ADMemberProperty = "member";
        private static string ADUIDProperty = "uid";

        public LdapProcessing()
        {
        }

        /// <summary>
        /// FormatAndSearchForUser.  Format and Search for a User.
        /// </summary>
        /// <param name="filterName"></param>
        /// <param name="filterValue"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public SearchResult FormatAndSearchForUser(string filterName, string filterValue, List<string> loadProperties)
        {
            // Build the LDAP PAth
            string strServerDNS = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPServerDNS"];
            string strSearchBaseDN = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPUserPath"];

            string strLDAPPath = "LDAP://" + strServerDNS + "/" + strSearchBaseDN;

            // Create New Directory Entry instance
            DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, null, null, AuthenticationTypes.Anonymous);
            
            // Set the Searcher
            DirectorySearcher searcher = new DirectorySearcher(objDirEntry);

            try
            {
                

                // Only load the Member Property
                searcher.PropertiesToLoad.AddRange(loadProperties.ToArray());

                // Set the Search Filter
                searcher.Filter = filterName + "=" + filterValue;

                // Define the Result
                SearchResult result;

                // Search for One
                result = searcher.FindOne();

                // Return the Search Result
                return (result);
            }
            catch (Exception ex)
            {
                // Error in Search
                logger.Error("Fatal LDAP Error", ex);
                throw new ApplicationException("Error LDAPInterface.FormatAndSearch", ex);
            }
            finally
            {
                if (objDirEntry != null)
                {
                    objDirEntry.Close();
                }
            }
        }

        /// <summary>
        /// FormatAndSearchForUser.  Format and Search for a User using their EID (employee id)
        /// </summary>
        /// <param name="filterName"></param>
        /// <param name="filterValue"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public SearchResult FormatAndSearchForEID(string filterName, string filterValue, List<string> loadProperties)
        {
            // Load the LDAP PAth
            string strServerDNS = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPServerDNS"];
            string strSearchBaseDN = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPUserPath"];

            string strLDAPPath = "LDAP://" + strServerDNS + "/" + strSearchBaseDN;

            // Create New Directory Entry instance
            DirectoryEntry objDirEntry =new DirectoryEntry(strLDAPPath, null, null, AuthenticationTypes.Anonymous);

            try
            {
                // Set the Searcher
                DirectorySearcher searcher = new DirectorySearcher(objDirEntry);

                // Only load the Member Property
                searcher.PropertiesToLoad.AddRange(loadProperties.ToArray());

                // Set the Search Filter
                searcher.Filter = filterName + "=" + filterValue;

                // Define the Result
                SearchResult result;

                // Search for One
                result = searcher.FindOne();

                // Return the Search Result
                return (result);
            }
            catch (Exception ex)
            {
                // Error in Search
                logger.Error("Fatal LDAP Error", ex);
                throw new ApplicationException("Error LDAPInterface.FormatAndSearch", ex);
            }
            finally
            {
                if (objDirEntry != null)
                {
                    objDirEntry.Close();
                }
            }
        }

        /// <summary>
        /// GetMembers.  Get and Associate Users to the specified Groups passed in.
        /// </summary>
        /// <param name="groupNames"></param>
        /// <param name="dwGroupsMembers"></param>
        /// <returns></returns>
        public void GetMembers(List<string> groupNames, SortedList<string, List<string>> dwGroupsMembers)
        {
            // Load the LDAP PAth for Group Search
            string strServerDNS = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPServerDNS"];
            string strSearchBaseDN = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPGroupPath"];

            string strLDAPPath = "LDAP://" + strServerDNS + "/" + strSearchBaseDN;

            // Create New Directory Entry instance
            DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, null, null, AuthenticationTypes.Anonymous);
            
            // Set the Searcher
            DirectorySearcher searcher = new DirectorySearcher(objDirEntry);

            // Build the Filter
            StringBuilder searchFilter = new StringBuilder();

            // Load all the Groups passed in the Search Filter
            searchFilter.Append("(|");
            foreach (string groupName in groupNames)
            {
                searchFilter.Append("(" + ADCommonNameProperty + "=" + groupName + ")");
            }
            // Append the last End Paren
            searchFilter.Append(")");

            // Set the Search Filter
            searcher.Filter = searchFilter.ToString();

            // Only load the Member Property
            searcher.PropertiesToLoad.Add(ADMemberProperty);

            // Define the Results Collection
            SearchResultCollection results;

            try
            {
                // Find all Groups
                results = searcher.FindAll();

                // Process all the Results
                for (int i = 0; i < results.Count; i++)
                {
                    // Get the "member" Attribute for the current Result (group)
                    ResultPropertyValueCollection memberResults = results[i].Properties[ADMemberProperty];

                    // Load the Mmebers from the current Result (group)
                    LoadMembers(results[i].Path, memberResults, dwGroupsMembers);
                }

                // Dispose of the LDAP results
                results.Dispose();
            }
            catch (Exception ex)
            {
                // Error in Search
                logger.Error("Fatal LDAP Error", ex);
                throw new ApplicationException("Error LDAPInterface.GetMembers: ", ex);
            }
            finally
            {
                if (objDirEntry != null)
                {
                    objDirEntry.Close();
                }
            }
        }

        /// <summary>
        /// LoadMembers.  Load Group and associated Users.
        /// </summary>
        /// <param name="resultPath"></param>
        /// <param name="memberResults"></param>
        /// <param name="groupsMembers"></param>
        private void LoadMembers(string resultPath,ResultPropertyValueCollection memberResults,SortedList<string, List<string>> groupsMembers)
        {
            // Get the Group Name from the Result Path
            int commonNamePosition = resultPath.IndexOf(ADCommonNameProperty + "=");

            // Move to the Name
            commonNamePosition += 3;

            // Find the Next Comma
            int commonNameEndComma = resultPath.IndexOf(",", commonNamePosition);

            // Get the Common Name for the Group
            string groupName = resultPath.Substring(commonNamePosition, commonNameEndComma - commonNamePosition);

            // Group has NOT been loaded already
            if (!groupsMembers.Keys.Contains(groupName))
            {
                // Define a List to Hold the Users for the Group being Processed
                List<string> userList = new List<string>();

                // Load all the Users for the Group
                int memberCount = memberResults.Count;
                for (int i = 0; i < memberCount; i++)
                {
                    // Split the Member Name by '," and get the 'uid' value
                    List<string> memberList = memberResults[i].ToString().Split(new char[] { ',' }).ToList();
                    string uidValue = memberList.Find(mem => mem.StartsWith(ADUIDProperty));
                    string[] uidKeyValue = uidValue.Split(new char[] { '=' });

                    // uid Value is what is needed
                    userList.Add(uidKeyValue[1]);
                }

                // Add the Group Name and associated Users tot he List
                groupsMembers.Add(groupName, userList);
            }
        }
    }

}
