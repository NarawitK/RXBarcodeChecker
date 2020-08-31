using BarcodeDrugCheckerLib.DataAccess.QueryBuilder;
using BarcodeDrugChecker.ViewModel;
using BarcodeDrugCheckerLib.DataAccess.Interface;
using System.Security;
using System.Windows;

namespace BarcodeDrugChecker.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class DBSetting : Window, IHavePassword
    {
        public SecureString Password => tb_password.SecurePassword;
        public DBSetting()
        {
            InitializeComponent();
            DataContext = new SettingViewModel(new DBHandler());
        }
    }
}
