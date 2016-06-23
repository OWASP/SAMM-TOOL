using OwaspSAMM.DAL;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Web;


namespace OwaspSAMM.Web
{
    public class UserDataBL
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private OwaspSAMMRepository DAL = new OwaspSAMMRepository();

        public UserDataBL()
        {
        }

        /// <summary>
        /// GetUserData - Method returns the UserData object for the user in the UserData table.  userIdentityName is a string containing Domain\Account 
        /// and is usually available through User.Identity.Name property.
        /// </summary>
        /// <param name="userIdentityName">string - in form of "Domain\Account" such as provided by User.Identity.Name</param>
        /// <returns>UserData object for the user.</returns>
        public UserData GetUserData(string userIdentityName)
        {
            return DAL.GetUserData(userIdentityName);
        }

        /// <summary>
        /// InsertUserData - Method returns the UserData object for the user in the UserData table after doing an insert using the params.
        /// </summary>
        /// <param name="account">string - in form of "Domain\Account" such as provided by User.Identity.Name</param>
        /// <param name="userNTID">string - user NT ID</param>
        /// <param name="userDomain">string - user Domain</param>
        /// <returns>UserData object for the user.</returns>
        public UserData InsertUserData(string account, string userNTID = null, string userDomain = null)
        {
            UserData result = new UserData();

            if (string.IsNullOrEmpty(userNTID) && string.IsNullOrEmpty(userDomain))
            {
                var xx = account.Split('\\');
                userDomain = xx[0];
                userNTID = xx[1];
            }

            var ud = new UserData();
            ud.UserNTID = userNTID;
            ud.UserDomain = userDomain;

            if (DAL.InsertUserData(ud))
                result = ud;

            return ud;
        }

        /// <summary>
        /// FindUser - This method retrieves the UserData object (UDO) from a data store and returns it to the caller.  To save some time, the 
        /// UDO is being saved in Session.  The method first looks for the UDO in Session.  If it is not in Session, it retrieves it from the 
        /// database.
        /// 
        /// If the User doesn't exist in the database, the user is added to the database.
        /// 
        /// LDAP information for the the user is updated the first time the user starts a session. 
        /// </summary>
        /// <param name="userIdentityName">string - usually the value in User.Identity.Name which should be DOMAIN\NTAccount</param>
        /// <returns>UserData - UDO for the user</returns>
        public UserData FindUser(string userIdentityName)
        {
            UserData ud = new UserData();

            try
            {
                if (HttpContext.Current.Session["UserDataObject"] == null)
                {
                    ud = GetUserData(userIdentityName);

                    // On first visit, user will not be in table, so add them.
                    if (ud == null)
                        ud = InsertUserData(userIdentityName);

                    // update the user's data from LDAP if LDAP is being used
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPEnabled"] == "true")
                    {
                        ud = LogUserLogin(ud);
                    }
                    else
                    //Otherwise, just update the last login date
                    {
                        ud = LogUserLoginDateOnly(ud);
                    }

                    HttpContext.Current.Session["UserDataObject"] = ud;
                }
                else
                {
                    ud = (UserData)HttpContext.Current.Session["UserDataObject"];

                    // Check "ud" to make sure the session user is the same as the actual user
                    var tokens = userIdentityName.Split('\\');
                    if (!(ud.UserDomain == tokens[0] && ud.UserNTID == tokens[1]))
                    {
                        ud = GetUserData(userIdentityName);
                        // update the user's LDAP information if LDAP is being used
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPEnabled"] == "true")
                        {
                            ud = LogUserLogin(ud);
                        }
                        //Otherwise, just update the last login date
                        else
                        {
                            ud = LogUserLoginDateOnly(ud);
                        }
                        HttpContext.Current.Session["UserDataObject"] = ud;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error: OwaspSAMMRepository.FindUser" + ex);
            }

            return ud;
        }


        /// <summary>
        /// LogUserLogin - This method retrieves data from LDAP and update the UserData object in the database
        /// </summary>
        /// <param name="userData">UserData - users data from UserData table</param>
        /// <param name="UpdateLastLogin">bool - default value is true</param>
        /// <returns>UserData - updated with current LDAP data</returns>
        public UserData LogUserLogin(UserData userData, bool UpdateLastLogin = true)
        {
            string ManagerDNID = null;
            string ManagerEID = null;
            string EmployeeEID = null;
            LdapProcessing LDAP = new LdapProcessing();

            string domainID = userData.UserDomain.ToString() + ":" + userData.UserNTID.ToString();

            try
            {
                // Define Properties to Return
                // These properties are defined in your local LDAP and the values are returned from the directory search

                string ADManagerEmailProperty = "manager";
                string ADOProperty = "hpOrganizationChart";
                string ABUProperty = "hpBusinessGroup";
                string ADCNProperty = "cn";
                string ADCNManagerNum = "managerEmployeeNumber";
                string ADEmailProperty = "mail";

                // Load the Properties into a list of strings 
                List<string> loadProperties = new List<string>();
                loadProperties.Add(ADManagerEmailProperty);
                loadProperties.Add(ADOProperty);
                loadProperties.Add(ABUProperty);
                loadProperties.Add(ADCNProperty);
                loadProperties.Add(ADCNManagerNum);
                loadProperties.Add(ADEmailProperty);

                // Search for the User
                SearchResult result = LDAP.FormatAndSearchForUser("ntuserdomainid", domainID, loadProperties);

                // Found the User
                if (result != null)
                {
                    // Parse name into first/last
                    string[] names = result.Properties[ADCNProperty][0].ToString().Split(' ');
                    string firstName = null;
                    string lastName = null;

                    foreach (string name in names)
                    {
                        if (firstName != null)
                        {
                            lastName = lastName + name + " ";
                        }
                        else
                        {
                            firstName = name;
                        }
                    }

                    if (firstName != null)
                    {
                        userData.FirstName = firstName.Trim();
                    }
                    else
                    {
                        userData.FirstName = "";
                    }

                    if (lastName != null)
                    {
                        userData.LastName = lastName.Trim();
                    }
                    else
                    {
                        userData.LastName = "";
                    }


                    // Get Manager, Organization, Business Unit
                    ManagerDNID = result.Properties[ADManagerEmailProperty][0].ToString();
                    userData.OrgName = result.Properties[ADOProperty][0].ToString();
                    userData.BusinessUnit = result.Properties[ABUProperty][0].ToString();

                    userData.ManagerEID = result.Properties[ADCNManagerNum][0].ToString();
                    ManagerEID = result.Properties[ADCNManagerNum][0].ToString();
                    EmployeeEID = result.Properties[ADEmailProperty][0].ToString();
                }
                else
                {
                    // User Not Found
                    logger.ErrorFormat("User {0} Not Found in LDAP", domainID);
                    throw new ApplicationException("User Not Found in LDAP: " + domainID);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new ApplicationException("Error LDAPInterface.GetUserWithDomainIdentity: " + Environment.NewLine + "User: " + domainID, ex);
            }

            // If a Manager EID (Employee ID) exists, then look up the manager in LDAP
            if (ManagerEID != null)
            {
                try
                {
                    // Define Properties to Return
                    string ADManagerIDProperty = "ntuserdomainid";
                    string ADMgrCNProperty = "cn";

                    // Load the Properties to Load
                    List<string> loadProperties = new List<string>();
                    loadProperties.Add(ADManagerIDProperty);
                    loadProperties.Add(ADMgrCNProperty);

                    // Search for the Manager info using Employee Number
                    SearchResult result = LDAP.FormatAndSearchForEID("employeeNumber", ManagerEID, loadProperties);

                    // Found the User
                    if (result != null)
                    {

                        string[] names = result.Properties[ADMgrCNProperty][0].ToString().Split(' ');
                        string MGRfirstName = null;
                        string MGRlastName = null;

                        foreach (string name in names)
                        {
                            if (MGRfirstName != null)
                            {
                                MGRlastName = MGRlastName + name + " ";
                            }
                            else
                            {
                                MGRfirstName = name;
                            }
                        }

                        if (MGRfirstName != null)
                        {
                            userData.MgrFirstName = MGRfirstName.Trim();
                        }
                        else
                        {
                            userData.MgrFirstName = "";
                        }

                        if (MGRlastName != null)
                        {
                            userData.MgrLastName = MGRlastName.Trim();
                        }
                        else
                        {
                            userData.MgrLastName = "";
                        }

                        userData.ManagerID = result.Properties[ADManagerIDProperty][0].ToString().Replace(@":", @"\").ToUpper();
                    }
                    else
                    {
                        // User Not Found
                        throw new ApplicationException("Manager Not Found in AD: " + ManagerEID);
                    }


                }
                catch (Exception ex)
                {
                    logger.Error(ex);

                    // Error in Search
                    throw new ApplicationException("Error LDAPInterface.FormatAndSearchForEmail: " + Environment.NewLine + "Mgr Email: " + ManagerEID, ex);
                }


            }


            // Now check to see if the user belongs to either the Admin Group or the Business Unit Owner LDAP groups
            bool GCSOwaspSAMMUser = false;

            List<string> SAMMGroups = new List<string>();
            SAMMGroups.Add(System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPAdminGroup"]);

            SortedList<string, List<string>> tempSAMMGroupsMembers = new SortedList<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            // Get the Members
            LDAP.GetMembers(SAMMGroups, tempSAMMGroupsMembers);

            // Process all the Groups for the Role
            foreach (string SAMMGroup in SAMMGroups)
            {
                // Find the Group in the Group/Members List
                List<string> members = tempSAMMGroupsMembers[SAMMGroup];

                // Interrogate the list for this group to see if the current users email is in it
                foreach (string member in members)
                {
                    // If users email is found set Boolean to true
                    if (member == EmployeeEID)
                    {
                        GCSOwaspSAMMUser = true;
                    }
                }
            }


            // Business Unit Owner LDAP group
            bool GCSSAMMBURep = false;

            SAMMGroups = new List<string>();
            SAMMGroups.Add(System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPBUOwnerGroup"]);

            tempSAMMGroupsMembers = new SortedList<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            // Get the Members
            LDAP.GetMembers(SAMMGroups, tempSAMMGroupsMembers);

            // Process all the Groups for the Role
            foreach (string SAMMGroup in SAMMGroups)
            {
                // Find the Group in the Group/Members List
                List<string> members = tempSAMMGroupsMembers[SAMMGroup];

                // Interrogate the list for this group to see if the current users email is in it
                foreach (string member in members)
                {
                    // If users email is found set Boolean to true
                    if (member == EmployeeEID)
                    {
                        GCSSAMMBURep = true;
                    }
                }
            }

            userData.Manager = DAL.IsUserAManager(userData.UserID);
            userData.BUOwner = GCSSAMMBURep;
            userData.Administrator = GCSOwaspSAMMUser;

            if (UpdateLastLogin)
            {
                userData.LastLoginDate = DateTime.Now;
            }

            var success = DAL.UpdateUserData(userData);

            return userData;
        }

        /// <summary>
        /// UserinLDAP - This method check LDAP for the existance of a user
        /// </summary>
        /// <param name="userData">UserData - users data from UserData table</param>
        /// <returns>bool - true if user is found in LDAP, false if user is not found in LDAP</returns>
        public bool UserinLDAP(UserData userData)
        {

            LdapProcessing LDAP = new LdapProcessing();

            string domainID = userData.UserDomain.ToString() + ":" + userData.UserNTID.ToString();

            try
            {
                // Define Properties to Return

                string ADEmailProperty = "mail";

                // Load the Properties to Load
                List<string> loadProperties = new List<string>();

                loadProperties.Add(ADEmailProperty);

                // Search for the User
                SearchResult result = LDAP.FormatAndSearchForUser("ntuserdomainid", domainID, loadProperties);

                // Found the User
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex);

                // Error in Search
                throw new ApplicationException("Error LDAPInterface.UserinLDAP: " + Environment.NewLine + "User: " + domainID, ex);
            }

        }

        /// <summary>
        /// LogUserLoginDateOnly - This method updates the LastLoginDate of the UserData object in the database for NonLDAP implementations
        /// </summary>
        /// <param name="userData">UserData - users data from UserData table</param>
        /// <returns>UserData - updated with current LastLoginDate data</returns>
        public UserData LogUserLoginDateOnly(UserData userData)
        {

            userData.LastLoginDate = DateTime.Now;


            var success = DAL.UpdateUserData(userData);

            return userData;
        }
    }
}
