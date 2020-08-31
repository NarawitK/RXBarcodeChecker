using BarcodeDrugChecker.ViewModel;
using BarcodeDrugChecker.ViewModel.UserControl;
using BarcodeDrugCheckerLib.DataAccess.QueryBuilder;
using System.Text.RegularExpressions;
using System.Windows;

namespace BarcodeDrugChecker.View
{
    /// <summary>
    /// ViewModel logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowModel vm = new MainWindowModel(new PatientAllergyViewModel(new DBHandler()), new DrugInteractionViewModel(new DBHandler()), new DBHandler());
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void DBSettingButton_Click(object sender, RoutedEventArgs e)
        {
            DBSetting settingWindow = new DBSetting();
            settingWindow.ShowDialog();
            bool? pageResult = settingWindow.DialogResult;
            if(pageResult == false)
            {
                vm._dbHandler.ReloadConnection();
            }
        }

        private void OnlyNumeric_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
