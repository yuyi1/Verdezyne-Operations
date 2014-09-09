using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace Operations.ActiveDirectory
{
    public class LdapAuthentication
    {
        private String _path;
        private String _filterAttribute;

        public LdapAuthentication(String path)
        {
            _path = path;
        }

        public bool IsAuthenticated(String domain, String username, String pwd)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                return context.ValidateCredentials(username, pwd);
            }
            #region CommentedOut
            //String domainAndUsername = domain + @"\" + username;
            //List<string> lstUsers = new List<string>();

            //DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            //try
            //{
            //    foreach (DirectoryEntry child in entry.Children)
            //    {
            //        string childPath = child.Path;
            //        PropertyCollection coll = child.Properties;

            //        if (child.SchemaClassName == "User")
            //        {
            //            using (var context = new PrincipalContext(ContextType.Domain))
            //            {
            //                return context.ValidateCredentials(username, pwd);
            //            }
            //        }
            //    }

            //    //Bind to the native AdsObject to force authentication.			

            //    //Object obj = entry.NativeObject;

            //    //DirectorySearcher search = new DirectorySearcher(entry);

            //    //search.Filter = "(SAMAccountName=" + username + ")";
            //    //search.PropertiesToLoad.Add("cn");
            //    //SearchResult result = search.FindOne();

            //    //if (null == result)
            //    //{
            //    //    return false;
            //    //}

            //    ////Update the new path to the user in the directory.
            //    //_path = result.Path;
            //    //_filterAttribute = (String)result.Properties["cn"][0];
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error authenticating user. " + ex.Message);
            //}

            //return true;
            #endregion
        }

        public String GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();

                int propertyCount = result.Properties["memberOf"].Count;

                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }
    }

}