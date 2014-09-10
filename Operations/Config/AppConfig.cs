namespace Operations.Config
{
    /// <summary>
    /// The class which parses the app.config file.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public const string MAIN_CONNECTION_STRING = "MainConnectionString";
        /// <summary>
        /// 
        /// </summary>
        public const string CONNECTION_STRINGS = "connectionStrings";
        /// <summary>
        /// 
        /// </summary>
        public const string APP_SETTINGS = "appSettings";
        /// <summary>
        /// 
        /// </summary>
        public const string SPREADSHEET_WATCH_FOLDER = "spreadsheet.watch.folder";
        /// <summary>
        /// 
        /// </summary>
        public const string JOB_WATCH_FOLDER = "job.watch.folder";
        /// <summary>
        /// 
        /// </summary>
        public const string RCODE_WORKING_FOLDER = "rcode.working.folder";
        /// <summary>
        /// 
        /// </summary>
        public const string RCODE_COMMAND = "rcode.command";
        /// <summary>
        /// 
        /// </summary>
        public const string MOBILEFORMS_OUTPUT_FOLDER = "MobileForms.output.folder";


        private PropertiesParser _cfg = null;
        private PropertiesParser _connectionStrings = null;

        /// <summary>
        /// 
        /// </summary>
        public PropertiesParser Cfg
        {
            get { return _cfg; }
            set { _cfg = value; }
        }

        /// <summary>
        /// The Connections Strings for the App
        /// </summary>
        public PropertiesParser ConnectionStrings
        {
            get { return _connectionStrings; }
            set { _connectionStrings = value; }
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public AppConfig() 
        {

        }
        /// <summary>
        /// Initializes the ConnectionStrings and Cfg from the app.config file
        /// </summary>
        public void InitFromAppSettings()
        {            
            if (_connectionStrings == null)
            {
                _connectionStrings = new PropertiesParser(ConfigFile.GetSection(CONNECTION_STRINGS));
            }
            if (_cfg == null)
            {
                _cfg = new PropertiesParser(ConfigFile.GetSection(APP_SETTINGS));
            }
        }
    }
}
