using System.Collections.ObjectModel;
using BarcodeDrugChecker.Enums;
using BarcodeDrugCheckerLib.DataAccess.QueryBuilder;
using BarcodeDrugCheckerLib.Model.Interface;

namespace BarcodeDrugChecker.ViewModel.UserControl
{
    public class PatientAllergyViewModel: Base.ViewModelBase
    {
        #region Property
        private DBHandler _db;
        public ObservableCollection<IPatientAllergy> PatientAllergiesList { get; private set; }
        public string DefaultImgSource
        {
            get => baseImgSrc + IndicatorIconStatus.warning.ToString() + ImgExtension;
        }
        private readonly string baseImgSrc = "/Assets/Icon/";
        private readonly string ImgExtension = ".png";
        #endregion
        #region UIProps
        private string _Visibility;
        private string _DataGridVisibility;
        private string _HasAllergiesStatus;
        private int _AllergiesCounter;
        private string _IndicatorImgSrc;
        #endregion

        #region UIEncapsulation
        public string DataGridVisibility
        {
            get => _DataGridVisibility;
            set
            {
                _DataGridVisibility = value;
                OnPropertyChanged();
            }
        }
        public string HasAllergiesStatus
        {
            get => _HasAllergiesStatus;
            private set
            {
                _HasAllergiesStatus = value;
                OnPropertyChanged();
            }
        }
        public string IndicatorImgSrc
        {
            get => _IndicatorImgSrc;
            private set
            {
                _IndicatorImgSrc = baseImgSrc + value + ImgExtension;
                OnPropertyChanged();
            }
        }
        public string Visibility
        {
            get => _Visibility;
            private set
            {
                _Visibility = value;
                OnPropertyChanged();
            }
        }
        public int AllergiesCounter
        {
            get => _AllergiesCounter;
            private set
            {
                _AllergiesCounter = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public PatientAllergyViewModel(DBHandler db)
        {
            _db = db;
            PatientAllergiesList = new ObservableCollection<IPatientAllergy>();
            AllergiesCounter = PatientAllergiesList.Count;
            ChangeImageSource(IndicatorIconStatus.ok);
            SetDataGridVisibility(Enums.Visibility.Collapsed);
            SetVisibility(Enums.Visibility.Collapsed);
        }

        #region Methods
        public async void GetPatientAllergies(string hn)
        {
            var result = await _db.GetPatientAllergy(hn);
            int Listcount;
            foreach(var item in result)
            {
                PatientAllergiesList.Add(item);
            }
            Listcount = PatientAllergiesList.Count;
            AutomateAllergiesStatusAndIconByListCount(Listcount);
            
        }
        private void SetHasAllergiesStatus(string Text)
        {
            HasAllergiesStatus = Text;
        }
        private void AutomateAllergiesStatusAndIconByListCount(int count)
        {
            SetAllergiesCounter(count);
            if (count > 0)
            {
                ChangeImageSource(IndicatorIconStatus.warning);
                SetHasAllergiesStatus(Properties.Resources.allergy_status_hasEntries);
                SetDataGridVisibility(Enums.Visibility.Visible);
            }
            else
            {
                ChangeImageSource(IndicatorIconStatus.ok);
                SetHasAllergiesStatus(Properties.Resources.allergy_status_normal);
                SetDataGridVisibility(Enums.Visibility.Collapsed);
            }
            SetVisibility(Enums.Visibility.Visible);
        }
        #endregion

        #region SetterMethods
        public void ResetAllergyControl()
        {
            ClearPatientAllergyList();
            SetAllergiesCounter(0);
            SetVisibility(Enums.Visibility.Collapsed);
            SetDataGridVisibility(Enums.Visibility.Collapsed);
        }
        private void ChangeImageSource(IndicatorIconStatus iconName)
        {
            IndicatorImgSrc = iconName.ToString();
        }
        private void SetAllergiesCounter(int counter)
        {
            AllergiesCounter = counter;
        }
        private void SetVisibility(Visibility visibility)
        {
            Visibility = visibility.ToString();
        }
        private void SetDataGridVisibility(Visibility visibility)
        {
            DataGridVisibility = visibility.ToString();
        }
        private void ClearPatientAllergyList()
        {
            PatientAllergiesList.Clear();
        }
        #endregion

    }
}
