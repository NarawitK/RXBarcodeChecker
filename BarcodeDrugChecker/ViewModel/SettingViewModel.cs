using BarcodeDrugChecker.Commands;
using BarcodeDrugCheckerLib.DataAccess.Interface;
using BarcodeDrugCheckerLib.DataAccess.QueryBuilder;
using MySqlConnector;
using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media;

namespace BarcodeDrugChecker.ViewModel
{
    public class SettingViewModel : Base.ViewModelBase
    {
        #region Properties
        private DBHandler _db;
        private string _ActiveDB;
        private string _DBStage;
        private bool IsFormInit = false;
        private string _DBServer;
        private uint _DBPort;
        private string _DBUsername;
        private SolidColorBrush _TestResultForegroundColor;
        private string _DBTestConnText;
        private string _TestResultVisibility = "Collapsed";

        public string TestResultVisibility
        {
            get => _TestResultVisibility;
            private set
            {
                _TestResultVisibility = value;
                OnPropertyChanged();
            }
        }
        public string DBTestConnText 
        {
            get => _DBTestConnText; 
            private set
            {
                _DBTestConnText = value;
                OnPropertyChanged();
            } 
        }
        public SolidColorBrush TestResultForegroundColor
        {
            get => _TestResultForegroundColor;
            private set
            {
                _TestResultForegroundColor = value;
                OnPropertyChanged();
            }
        }

        public string[] DBComboBoxArray { get; private set; } = new string[1] { "mysql"};
        public string[] DBStageComboBoxArray { get; private set; } = new string[3] { "development", "staging","production" };

        public string ActiveDB
        {
            get => _ActiveDB;
            set
            {
                _ActiveDB = value;
                OnPropertyChanged();
                if (IsFormInit)
                    OnDBComboBoxChanged();
            }  
        }

        public string DBStage
        {
            get => _DBStage;
            set 
            {
                _DBStage = value;
                OnPropertyChanged();
                if (IsFormInit)
                    OnDBComboBoxChanged();
            } 
        }

        public string DBServer
        {
            get => _DBServer;
            set
            {
                _DBServer = value;
                OnPropertyChanged();
            }
        }

        public uint DBPort
        {
            get => _DBPort;
            set
            {
                _DBPort = value;
                OnPropertyChanged();
            }
        }

        public string DBUsername
        {
            get => _DBUsername;
            set
            {
                _DBUsername = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public SettingViewModel(DBHandler db)
        {
            _db = db;
            LoadLastConfiguration();
            ApplyConfigurationCommand = new ParamCommand<object>(ApplyConfiguration);
            TestOpenConnectionTriggerCommand = new RelayCommand(TestOpenConnectionTrigger);
        }

        private void LoadLastConfiguration()
        {
            string connstr = GetConnString(LoadCurrentConnectionStringsKey());
            SetUIFromConnectionString(connstr);
            IsFormInit = !IsFormInit;
        }

        private void OnDBComboBoxChanged()
        {
            string connstr = GetConnString(GetCurrentComboBoxConnectionStringsKey());
            SetUIFromConnectionString(connstr);
        }

        public ParamCommand<object> ApplyConfigurationCommand { get; }
        private void ApplyConfiguration(object param)
        {
            try
            {
                Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string constring = ConstructConStringFromUI(param);
                if (constring != null)
                {
                    ConnectionStringsSection cfgGetSectionConStr = (ConnectionStringsSection)cfg.GetSection("connectionStrings");
                    AppSettingsSection cfgGetSectionAppSetting = (AppSettingsSection)cfg.GetSection("appSettings");
                    cfgGetSectionConStr.ConnectionStrings[GetCurrentComboBoxConnectionStringsKey()].ConnectionString = constring;
                    cfgGetSectionAppSetting.Settings["use-db"].Value = ActiveDB;
                    cfgGetSectionAppSetting.Settings["data-stage"].Value = DBStage;
                    cfg.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("connectionStrings");
                    ConfigurationManager.RefreshSection("appSettings");
                    SetResultUI(Properties.Resources.dbsetting_applyPass, Brushes.Green, "Visible");
                }
                else
                    SetResultUI(Properties.Resources.db_setting_applyFailed, Brushes.PaleVioletRed, "Visible");
            }
            catch(Exception ex)
            {
                SetResultUI(ex.Message, Brushes.Red, "Visible");
            }
        }

        private string ConstructConStringFromUI(object param)
        {
            var passwordContainer = param as IHavePassword;
            if (passwordContainer != null)
            {
                var securestring = passwordContainer.Password;
                var unsecure_password = ConvertToUnsecureString(securestring);
                var csb = new MySqlConnectionStringBuilder()
                {
                    Server = DBServer,
                    Port = DBPort,
                    UserID = DBUsername,
                    Database = "kps",
                    Password = unsecure_password
                };
                /*
                csb.Server = DBServer;
                csb.Port = DBPort;
                csb.UserID = DBUsername;
                csb.Password = unsecure_password;
                */
                return csb.ConnectionString;
            }
            else
                return null;
        }

        private string GetConnString(string constrKey)
        {
            return ConfigurationManager.ConnectionStrings[constrKey].ConnectionString;
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public RelayCommand TestOpenConnectionTriggerCommand { get; }
        private void TestOpenConnectionTrigger()
        {
            bool res = TestOpenConnection();
            SetResultUIOnTestStatus(res);
        }

        private bool TestOpenConnection()
        {
            _db = new DBHandler();
            return _db.TestOpenConnection();
        }

        private void SetResultUIOnTestStatus(bool status)
        {
            if (status)
            {
                DBTestConnText = Properties.Resources.dbsetting_testpass;
                TestResultForegroundColor = Brushes.Green;
                TestResultVisibility = "Visible";
            }
            else
            {
                DBTestConnText = Properties.Resources.dbsetting_testfail;
                TestResultForegroundColor = Brushes.PaleVioletRed;
                TestResultVisibility = "Visible";
            }
        }
        private void SetResultUI(string res, SolidColorBrush foreColor, string visibility)
        {
            DBTestConnText = res;
            TestResultForegroundColor = foreColor;
            TestResultVisibility = visibility;
        }

        private void SetUIFromConnectionString(string connstr)
        {
            var csb = new MySqlConnectionStringBuilder(connstr);
            DBServer = csb.Server;
            DBPort = csb.Port;
            DBUsername = csb.UserID;
        }

        private string GetCurrentComboBoxConnectionStringsKey()
        {
            string FullDBNameKey = $"{ActiveDB}-{DBStage}";
            return FullDBNameKey;
        }

        private string LoadCurrentConnectionStringsKey()
        {
            ActiveDB = ConfigurationManager.AppSettings["use-db"];
            DBStage = ConfigurationManager.AppSettings["data-stage"];
            string FullDBNameKey = $"{ActiveDB}-{DBStage}";
            return FullDBNameKey;
        }    
    }
}
