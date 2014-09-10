using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Operations.Config
{
    /// <summary>
    /// Class to manipulate sections of a config file: appSettings and connectionStrings
    /// </summary>
    public class ConfigFile
    {


        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ConfigFile() { }

        ///// <summary>
        ///// Gets the the section from the App.Config file
        ///// </summary>
        ///// <param name="section">string - the section name to retrieve</param>
        ///// <returns>NameValueCollection of section names and values</returns>
        //public static NameValueCollection GetSection(string section)
        //{
        //    return GetSection(section);
        //}
        /// <summary>
        /// Gets the the section from the App.Config file
        /// </summary>
        /// <param name="section">string - the section name to retrieve</param>
        /// <returns>NameValueCollection of section names and values</returns>
        public static NameValueCollection GetSection(string section)
        {

            NameValueCollection cool = new NameValueCollection();

            // Get the selected section.
            switch (section)
            {

                case "appSettings":

                    return GetAppSettings();
                //break;

                case "connectionStrings":
                    return GetConnectionStrings();

                default:
                    Console.WriteLine("GetSection: Unknown section (0)", section);
                    break;

            }



            return cool;
        }
        private static NameValueCollection GetAppSettings()
        {
            NameValueCollection cool = new NameValueCollection();
            //KeyValueConfigurationCollection settings = null;
            try
            {
                //if (isWeb)
                //{
                cool = System.Web.Configuration.WebConfigurationManager.AppSettings;

                //}
                //else
                //{

                //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as System.Configuration.Configuration;

                //AppSettingsSection appSettings = config.AppSettings as AppSettingsSection;
                //settings = config.AppSettings.Settings;

                //if (settings.Count > 0)
                //{
                //    // Get the KeyValueConfigurationCollection 
                //    // from the configuration.

                //    foreach (KeyValueConfigurationElement keyValueElement in settings)
                //    {
                //        cool.Add(keyValueElement.Key, keyValueElement.Value);
                //    }
                //}
                ////}


            }
            catch (ConfigurationErrorsException e)
            {
                throw new Exception(string.Format("Error in AppSettings property: {0}", e.Message));
            }
            return cool;
        }
        private static NameValueCollection GetConnectionStrings()
        {
            NameValueCollection cool = new NameValueCollection();
            try
            {
                //if (isWeb)
                //{
                    ConnectionStringsSection connectionStringsSection = System.Web.Configuration.WebConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;
                    foreach (ConnectionStringSettings setting in connectionStringsSection.ConnectionStrings)
                    {
                        cool.Add(setting.Name, setting.ConnectionString);

                    }
                //}
                //else
                //{
                //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as System.Configuration.Configuration;

                //ConnectionStringsSection conStrSection = config.ConnectionStrings as ConnectionStringsSection;

                //if (conStrSection.ConnectionStrings.Count != 0)
                //{

                //    //Console.WriteLine();
                //    //Console.WriteLine("Using ConnectionStrings property.");
                //    //Console.WriteLine("Connection strings:");

                //    // Get the collection elements.
                //    foreach (ConnectionStringSettings connection in conStrSection.ConnectionStrings)
                //    {
                //        string name = connection.Name;
                //        string provider = connection.ProviderName;
                //        string connectionString = connection.ConnectionString;

                //        //Console.WriteLine("Name:               {0}",name);
                //        //Console.WriteLine("Connection string:  {0}",connectionString);
                //        //Console.WriteLine("Provider:            {0}",provider);
                //        cool.Add(name, connectionString);
                //    }

                //}
                //}
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("Using ConnectionStrings property: {0}", e.ToString());
            }
            return cool;
        }
    }

}