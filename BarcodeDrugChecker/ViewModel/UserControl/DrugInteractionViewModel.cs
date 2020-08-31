using System.Linq;
using BarcodeDrugChecker.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BarcodeDrugCheckerLib.DataAccess.QueryBuilder;
using BarcodeDrugCheckerLib.Model.Interface;
using BarcodeDrugChecker.Commands;

namespace BarcodeDrugChecker.ViewModel.UserControl
{
    public class DrugInteractionViewModel : Base.ViewModelBase
    {
        #region Property
        private readonly DBHandler _db;
        private List<IDrugInteraction> AllDrugInteractions;
        public ObservableCollection<IDrugInteraction> PatientDrugInteractionList { get; private set; }
        private readonly string baseImgSrc = "/Assets/Icon/";
        private readonly string ImgExtension = ".png";
        #endregion

        #region UIProps
        private string _DIDataGridVisibility;
        private string _Visibility;
        private string _DIStatus;
        private string _IndicatorImgSrc;
        private int _DrugInteractionCounter;
        private string _DGNoteVisibility;
        private string _DGNoteBtnTxt = Properties.Resources.di_defaultNoteBtntxt;
        #endregion

        #region UIEncapsulation
        public string DGNoteBtnTxt
        {
            get => _DGNoteBtnTxt;
            private set
            {
                _DGNoteBtnTxt = value;
                OnPropertyChanged();
            }
        }
        public string DGNoteVisibility
        {
            get => _DGNoteVisibility;
            private set
            {
                _DGNoteVisibility = value;
                OnPropertyChanged();
            }
        }
        public string DIDataGridVisibility
        {
            get => _DIDataGridVisibility;
            private set
            {
                _DIDataGridVisibility = value;
                OnPropertyChanged();
            }
        }
        public string DIStatus
        {
            get => _DIStatus;
            private set
            {
                _DIStatus = value;
                OnPropertyChanged();
            }
        }
        public int DrugInteractionCounter
        {
            get => _DrugInteractionCounter;
            private set
            {
                _DrugInteractionCounter = value;
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
        #endregion

        public DrugInteractionViewModel(DBHandler db)
        {
            _db = db;
            AllDrugInteractions = new List<IDrugInteraction>();
            PatientDrugInteractionList = new ObservableCollection<IDrugInteraction>();
            DrugInteractionCounter = PatientDrugInteractionList.Count;
            SetVisibility(Enums.Visibility.Collapsed);
            SetDataGridVisibility(Enums.Visibility.Collapsed);
            SetNoteVisibility(Enums.Visibility.Collapsed);
            ChangeImageSource(IndicatorIconStatus.ok);
            OnToggleDatagridNoteButtonCommand = new RelayCommand(OnToggleDatagridNoteButton);
        }

        public async void GetPatientDI(IList<IDrugVisit> RetrieveDrugList)
        {
            if(AllDrugInteractions.Count == 0)
            {
                var result = await _db.GetDrugInteractionList();
                foreach (var item in result)
                    AllDrugInteractions.Add(item);
            }
            int listCount = FilterDrugInteractions(AllDrugInteractions, RetrieveDrugList);
            AutomateUIByListCount(listCount);
        }

        private int FilterDrugInteractions(List<IDrugInteraction> DIList, IList<IDrugVisit> RetrieveDrugList)
        {
            //Only FirstDrugName in this list.
            var distinctedFirstDrugName = DIList.GroupBy(x => x.FirstDrug).Select(y => y.Key).ToList();

            foreach(var firstDrugName in distinctedFirstDrugName)
            {
                if (RetrieveDrugList.Any(i => i.GenericName == firstDrugName))
                {
                    foreach(var item in DIList)
                    {
                        if(item.FirstDrug == firstDrugName)
                        {
                            foreach (var drugitem in RetrieveDrugList)
                            {
                                if (drugitem.GenericName == item.SecondDrug)
                                {
                                    PatientDrugInteractionList.Add(item);
                                    //break;
                                }
                            }
                        }
                    }
                }
                DIList.RemoveAll(x => x.FirstDrug == firstDrugName);
            }
            return PatientDrugInteractionList.Count;
        }

        private void AutomateUIByListCount(int count)
        {
            if (count > 0)
            {
                SetDrugInteractionCounter(count);
                SetDIStatus(Properties.Resources.di_status_hasEntries);
                ChangeImageSource(IndicatorIconStatus.warning);
                SetDataGridVisibility(Enums.Visibility.Visible);
            }
            else
            {
                SetDrugInteractionCounter(count);
                SetDIStatus(Properties.Resources.di_status_normal);
                ChangeImageSource(IndicatorIconStatus.ok);
                SetDataGridVisibility(Enums.Visibility.Collapsed);
            }
            SetVisibility(Enums.Visibility.Visible);
        }

        public RelayCommand OnToggleDatagridNoteButtonCommand { get; }
        private void OnToggleDatagridNoteButton()
        {
            switch (DGNoteVisibility)
            {
                case "Collapsed":
                    DGNoteBtnTxt = Properties.Resources.di_closeNoteBtnTxt;
                    DGNoteVisibility = Enums.Visibility.Visible.ToString();
                    break;
                case "Visible":
                default:
                    DGNoteBtnTxt = Properties.Resources.di_openNoteBtnTxt;
                    DGNoteVisibility = Enums.Visibility.Collapsed.ToString();
                    break;
            }
        }

        public void ResetDIControl()
        {
            SetVisibility(Enums.Visibility.Collapsed);
            SetDataGridVisibility(Enums.Visibility.Collapsed);
            ClearPatientDIList();
            //Not necessary
            SetDrugInteractionCounter(0);
        }
        private void ChangeImageSource(IndicatorIconStatus iconName)
        {
            IndicatorImgSrc = iconName.ToString();
        }
        private void SetDIStatus(string ResourceText)
        {
            DIStatus = ResourceText;
        }
        private void SetDrugInteractionCounter(int counter)
        {
            DrugInteractionCounter = counter;
        }
        private void SetVisibility(Visibility visibility)
        {
            Visibility = visibility.ToString();
        }
        private void SetDataGridVisibility(Visibility visibility)
        {
            DIDataGridVisibility = visibility.ToString();
        }
        private void SetNoteVisibility(Visibility visibility)
        {
            DGNoteVisibility = visibility.ToString();
        }
        private void ClearPatientDIList()
        {
            PatientDrugInteractionList.Clear();
        }
    }
}
