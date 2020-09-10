using System;
using System.Collections.ObjectModel;
using BarcodeDrugChecker.Commands;
using BarcodeDrugChecker.ViewModel.UserControl;
using BarcodeDrugChecker.ViewModel.Base;
using BarcodeDrugCheckerLib.DataAccess.QueryBuilder;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using BarcodeDrugCheckerLib.Model.Interface;
using System.Configuration;
using BarcodeDrugChecker.Enums;

namespace BarcodeDrugChecker.ViewModel
{
    public class MainWindowModel : ViewModelBase
    {
        #region SubVM
        public DrugInteractionViewModel DrugInteractionViewModel { get; set; }
        public PatientAllergyViewModel PatientAllergyViewModel { get; set; }
        #endregion
        #region Property
        private readonly SolidColorBrush _PassColor = Brushes.Green;
        private readonly SolidColorBrush _FailColor = Brushes.Red;
        private readonly string _Visible = "Visible";
        private readonly string _Collapsed = "Collapsed";
        private readonly string _CounterText;
        private BitmapImage _dImg;
        public DBHandler _dbHandler { get; private set; }
        #endregion
        #region DataProps
        private string _CurrentPatientHN;
        private IPatient _Patient;
        private string _CurrentICode;
        public ObservableCollection<IDrugVisit> DrugRetrievalList { get; set; }
        private IDrugImage _DrugImage;

        private IDrugVisit _SelectedDrug;
        public IDrugVisit SelectedDrug
        {
            get => _SelectedDrug;
            set
            {
                _SelectedDrug = value;
                OnPropertyChanged();
                if (value != null)
                    OnDataGridSelectedItem();
            }
        }

        public BitmapImage DImg
        {
            get => _dImg;
            set
            {
                _dImg = value;
                OnPropertyChanged();
            }
        }

        public string CurrentPatientHN
        {
            get => _CurrentPatientHN;
            set
            {
                _CurrentPatientHN = value;
                OnPropertyChanged();
            }
        }
        
        public IPatient Patient
        {
            get => _Patient;
            set
            {
                _Patient = value;
                OnPropertyChanged();
            }
        }

        public string CurrentICode
        {
            get =>_CurrentICode;
            set
            {
                _CurrentICode = value;
                OnPropertyChanged();
            }
        }

        public IDrugImage DrugImage
        {
            get => _DrugImage;  
            set
            {
                _DrugImage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region UIProps
        //Loading Text
        public string LoadingText { get; private set; } = Properties.Resources.txt_loading;
        //Loading TextVisibility
        private string _LoadingTextVisibility;
        public string LoadingTextVisibility
        {
            get => _LoadingTextVisibility;
            private set
            {
                _LoadingTextVisibility = value;
                OnPropertyChanged();
            }
        }

        //Font Color
        private SolidColorBrush _PatientMessageColor;
        private SolidColorBrush _ICodeMessageColor;
        private SolidColorBrush _CounterMessageColor = Brushes.Green;

        //Checker Text
        private string _PatientSearchChecker;
        private string _DrugListChecker;
        private string _DrugImageChecker;
        //Visibility
        private string _PatientStatusMessageVisibility;
        private string _PatientInfoVisibility;
        private string _ICodeDataGridVisibility;
        private string _ICodeStatusMessageVisibility;
        private string _DrugImageVisibility;
        private string _NoImageTextVisibility;
        //Focus Management
        private bool _ShouldPatientHNFocus;
        private bool _ShouldICodeInputFocus;
        //Enabler
        private bool _IsICodeInputEnable;
        //Drug Remaining Counter
        public ushort DrugRemainCounter { get; private set; }
        private string _DrugRemainCounterText;
        //IconSource
        private string BaseImgSource = "/Assets/Icon/";
        private string ImgExtension = ".png";
        private string _IndicatorIconImgSource { get; set; }

        public SolidColorBrush CounterMessageColor
        {
            get => _CounterMessageColor;
            set
            {
                _CounterMessageColor = value;
                OnPropertyChanged();
            }
        }

        public string IndicatorIconImgSource 
        {
            get => _IndicatorIconImgSource;
            set
            {
                _IndicatorIconImgSource = $"{BaseImgSource}{value}{ImgExtension}";
                OnPropertyChanged();
            }
        }

        public string DrugRemainCounterText
        {
            get => _DrugRemainCounterText;
            set 
            {
                _DrugRemainCounterText = value;
                OnPropertyChanged();
            } 
        }
        
        public SolidColorBrush PatientMessageColor
        {
            get
            {
                return _PatientMessageColor;
            }
            set
            {
                _PatientMessageColor = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ICodeMessageColor 
        { 
            get => _ICodeMessageColor;
            set
            {
                _ICodeMessageColor = value;
                OnPropertyChanged();
            }
        }

        public string PatientStatusMessageVisibility
        {
            get
            {
                return _PatientStatusMessageVisibility;
            }
            set
            {
                _PatientStatusMessageVisibility = value;
                OnPropertyChanged();
            }
        }

        public string PatientInfoVisibility 
        { 
            get => _PatientInfoVisibility;
            set 
            {
                _PatientInfoVisibility = value;
                OnPropertyChanged();
            }  
        }

        public string ICodeDataGridVisibility
        {
            get => _ICodeDataGridVisibility;
            set
            {
                _ICodeDataGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public string ICodeStatusMessageVisibility
        {
            get => _ICodeStatusMessageVisibility;
            set
            {
                _ICodeStatusMessageVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool IsICodeInputEnable
        {
            get => _IsICodeInputEnable;
            set
            {
                _IsICodeInputEnable = value;
                OnPropertyChanged();
            }
        }

        public string DrugImageVisibility
        {
            get => _DrugImageVisibility;
            set
            {
                _DrugImageVisibility = value;
                OnPropertyChanged();
            }
        }

        public string PatientSearchChecker
        {
            get
            {
                return _PatientSearchChecker;
            }
            set
            {
                _PatientSearchChecker = value;
                OnPropertyChanged();
            }
        }

        public string DrugListChecker
        {
            get => _DrugListChecker;
            set
            {
                _DrugListChecker = value;
                OnPropertyChanged();
            }
        }

        public string DrugImageChecker
        {
            get => _DrugImageChecker;
            set
            {
                _DrugImageChecker = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldPatientHNFocus
        {
            get => _ShouldPatientHNFocus;
            set
            {
                _ShouldPatientHNFocus = value;
                OnPropertyChanged();
            }
        }
        public bool ShouldICodeInputFocus
        {
            get => _ShouldICodeInputFocus;
            set
            {
                _ShouldICodeInputFocus = value;
                OnPropertyChanged();
            }
        }
        public string NoImageTextVisibility 
        {
            get => _NoImageTextVisibility;
            set
            {
                _NoImageTextVisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion
        
        public MainWindowModel(PatientAllergyViewModel patientAllergyViewModel, DrugInteractionViewModel drugInteractionViewModel, DBHandler db)
        {
            _CounterText = Properties.Resources.txt_remDrug;
            DrugImageChecker = Properties.Resources.txt_noDImg;
            IndicatorIconImgSource = Enums.IndicatorIconStatus.ok.ToString();
            NoImageTextVisibility = _Visible;
            ShouldPatientHNFocus = true;
            ShouldICodeInputFocus = IsICodeInputEnable = false;
            LoadingTextVisibility = PatientStatusMessageVisibility = PatientInfoVisibility = ICodeDataGridVisibility = ICodeStatusMessageVisibility = DrugImageVisibility = _Collapsed;

            _dbHandler = db;

            PatientAllergyViewModel = patientAllergyViewModel;
            DrugInteractionViewModel = drugInteractionViewModel;

            DrugRetrievalList = new ObservableCollection<IDrugVisit>();
            FindPatientCommand = new RelayCommand(FindPatient, FindPatientCommand_CanExec);
            ICodeInputCommand = new RelayCommand(ICodeInput, ICodeInputCommand_CanExec);
            ResetMainWindowCommand = new RelayCommand(ResetMainWindow);
        }

        private bool IsDrugImageShow()
        {
            if (_DrugImageVisibility == _Visible && _NoImageTextVisibility == _Collapsed)
                return true;
            else
                return false;
        }
        private void ShowDrugImage()
        {
            if (_DrugImageVisibility == _Collapsed && _NoImageTextVisibility == _Visible)
            {
                DrugImageVisibility = _Visible;
                NoImageTextVisibility = _Collapsed;
            }
        }
        private void HideDrugImage()
        {
            if (_DrugImageVisibility == _Visible && _NoImageTextVisibility == _Collapsed)
            {
                if(DImg != null)
                    DImg = null;
                NoImageTextVisibility = _Visible;
                DrugImageVisibility = _Collapsed;
            }
        }

        #region Methods
        public RelayCommand FindPatientCommand { get; }
        private async void FindPatient()
        {
            ChangeLoadingVisibility(Visibility.Visible);
            SearchFlag flag = SearchFlag.EmptyList;
            string message = CurrentPatientHN;
            try
            {
                if (Patient != null)
                {
                    Patient = null;
                    ClearSubControl();
                }
                if (CurrentPatientHN != null && CurrentPatientHN.Length >= 9)
                {
                    IPatient tempPatientData = await _dbHandler.GetPatient(CurrentPatientHN);
                    if (tempPatientData == null)
                    {
                        flag = SearchFlag.NotFound;
                        CurrentPatientHN = null;
                        FocusHNTextBox();
                        PatientStatusMessageVisibility = _Visible;
                    }
                    else
                    {
                        Patient = tempPatientData;
                        flag = SearchFlag.Found;
                        GetDrugList();
                        if (IsDrugImageShow())
                        {
                            HideDrugImage();
                        }
                        IsICodeInputEnable = true;
                        ToggleTextboxFocus();
                        PatientInfoVisibility = _Visible;
                    }
                }
                else
                {
                    flag = SearchFlag.InputEmpty;
                    CurrentPatientHN = null;
                    FocusHNTextBox();
                    PatientStatusMessageVisibility = _Visible;
                }
            }

            catch(Exception ex)
            {
                flag = SearchFlag.Error;
                message = ex.Message;
                CurrentPatientHN = null;
                FocusHNTextBox();
                PatientStatusMessageVisibility = _Visible;
            }
            finally
            {
                PatientSearchMessage(flag, message);
                ChangeLoadingVisibility(Visibility.Collapsed);
            }
        }
        public bool FindPatientCommand_CanExec()
        {
            if (!string.IsNullOrEmpty(CurrentPatientHN) && CurrentPatientHN.Length >= 9)
                return true;
            else
                return false;
        }

        public async void GetDrugList() 
        {
            try
            {
                if (DrugRetrievalList.Count > 0)
                    DrugRetrievalList.Clear();
                var FetchedDrug = await _dbHandler.GetDrugListfromPatient(CurrentPatientHN);
                foreach (var item in FetchedDrug)
                {
                    if(item.Quantity > 0) //Why do we need this crap ?
                        DrugRetrievalList.Add(item);
                }
                PatientAllergyViewModel.GetPatientAllergies(CurrentPatientHN);
                DrugInteractionViewModel.GetPatientDI(DrugRetrievalList);
                DrugRemainCounter = (ushort)DrugRetrievalList.Count;
                DrugRemainCounterText = $"{_CounterText}{DrugRemainCounter}";
                ChangeDrugCounterStyle(DrugRemainCounter);
                ICodeDataGridVisibility = _Visible;
            }
            catch (Exception ex)
            {
                ICodeStatusMessage(SearchFlag.Error, ex.Message);
            }
        }

        public RelayCommand ICodeInputCommand { get; }
        public void ICodeInput()
        {
            SearchFlag flag = SearchFlag.EmptyList;
            string message = null;
            try
            {
                if(CurrentICode != null)
                {
                    if (DrugRetrievalList.Count > 0)
                    {
                        if (DrugListChecker != null)
                        {
                            DrugListChecker = null;
                        }
                        for (int i = 0; i < DrugRetrievalList.Count; i++)
                        {
                            if (CurrentICode == DrugRetrievalList[i].ICode && !DrugRetrievalList[i].BarcodeChecked)
                            {
                                flag = SearchFlag.Found;
                                DrugRetrievalList[i].BarcodeChecked = !DrugRetrievalList[i].BarcodeChecked;
                                DrugRemainCounterText = $"{_CounterText}{--DrugRemainCounter}";
                                //FetchDrugImg(CurrentICode);
                                message = $"{ DrugRetrievalList[i].ICode} - { DrugRetrievalList[i].DrugName}.";
                                //SelectedIndex = i;
                                SelectedDrug = DrugRetrievalList[i];
                                break;
                            }
                            else
                                flag = SearchFlag.NotFound;
                        }
                    }
                }
                else
                    flag = SearchFlag.InputEmpty;
                ICodeStatusMessageVisibility = _Visible;
            }
            catch(Exception ex)
            {
                flag = SearchFlag.Error;
                message = ex.Message;
            }
            finally
            {
                if (DrugRetrievalList.Count > 0 && DrugRemainCounter < 1)
                {
                    ChangeDrugCounterStyle(DrugRemainCounter);
                    ICodeStatusMessage(flag, message);
                    GetFocusAndClearHNTextBox();
                }
                else
                {
                    ICodeStatusMessage(flag, message);
                    CurrentICode = null;
                    FocusICodeTextBox();
                }
            }
        }
        public bool ICodeInputCommand_CanExec()
        {
            if (!string.IsNullOrEmpty(CurrentICode) && CurrentICode.Length >= 7)
                return true;
            else
                return false;
        }
        public async void FetchDrugImg(string icode)
        {
            try
            {
                IDrugImage imgFetched = await _dbHandler.GetDrugImage(icode);
                if (imgFetched != null)
                {
                    DrugImage = imgFetched;
                    DImg = ConvertBytesToBitmapImage(DrugImage.ImageBytes);
                    ShowDrugImage();
                    DrugImage.ImageBytes = null;
                }
                else
                {
                    DImg = null;
                    DrugImageChecker = Properties.Resources.txt_noDImg;
                    if (IsDrugImageShow())
                        HideDrugImage();
                }
            }
            catch(Exception ex)
            {
                DImg = null;
                DrugImageChecker = $"{Properties.Resources.msg_DImgError}\n{ex.Message}";
                HideDrugImage();
            }
        }

        private BitmapImage ConvertBytesToBitmapImage(byte[] ImgBytes)
        {
            using (MemoryStream ms = new MemoryStream(ImgBytes))
            {
                BitmapImage bmImg = new BitmapImage();
                bmImg.BeginInit();
                bmImg.CacheOption = BitmapCacheOption.OnLoad;
                bmImg.DecodePixelHeight = int.Parse(ConfigurationManager.AppSettings["drug-img-height"]); ;
                bmImg.DecodePixelWidth = int.Parse(ConfigurationManager.AppSettings["drug-img-width"]);
                bmImg.StreamSource = ms;
                bmImg.EndInit();
                bmImg.Freeze();
                return bmImg;
            }
        }
        #endregion

        #region UICommand
        private void PatientSearchMessage(SearchFlag flag, string message = null)
        {
            switch (flag)
            {
                case SearchFlag.Found:
                    PatientMessageColor = _PassColor;
                    PatientSearchChecker = $"{Properties.Resources.msg_patientFound}: {message}";
                    break;
                case SearchFlag.NotFound:
                    PatientMessageColor = _FailColor;
                    PatientSearchChecker = $"{Properties.Resources.msg_patientNotFound}: {message}";
                    break;
                case SearchFlag.Error:
                    PatientMessageColor = _FailColor;
                    PatientSearchChecker = $"{Properties.Resources.msg_patientError}:\n{message}";
                    break;
                case SearchFlag.InputEmpty:
                    PatientMessageColor = _FailColor;
                    PatientSearchChecker = Properties.Resources.msg_hnNotConform;
                    break;
                default:
                    PatientMessageColor = _FailColor;
                    PatientSearchChecker = null;
                    break;
            }
        }
        private void ICodeStatusMessage(SearchFlag flag, string message = null)
        {
            switch (flag)
            {
                case SearchFlag.Found:
                    IndicatorIconImgSource = Enums.IndicatorIconStatus.ok.ToString();
                    ICodeMessageColor = _PassColor;
                    DrugListChecker = $"{message} {Properties.Resources.msg_icodeFound}";
                    break;
                case SearchFlag.NotFound:
                    IndicatorIconImgSource = Enums.IndicatorIconStatus.error.ToString();
                    ICodeMessageColor = _FailColor;
                    DrugListChecker = Properties.Resources.msg_icodeNotFound;
                    break;
                case SearchFlag.InputEmpty:
                    IndicatorIconImgSource = Enums.IndicatorIconStatus.error.ToString();
                    ICodeMessageColor = _FailColor;
                    DrugListChecker = Properties.Resources.msg_icodeEmpty;
                    break;
                case SearchFlag.Error:
                    IndicatorIconImgSource = Enums.IndicatorIconStatus.error.ToString();
                    ICodeMessageColor = _FailColor;
                    DrugListChecker = $"{Properties.Resources.msg_icodeError}: {message}";
                    break;
                default:
                    IndicatorIconImgSource += Enums.IndicatorIconStatus.error.ToString();
                    ICodeMessageColor = _FailColor;
                    DrugListChecker = Properties.Resources.msg_icodeListEmpty;
                    break;
            }
        }

        private void ChangeDrugCounterStyle(ushort drugCounter)
        {
            if (drugCounter > 0)
                CounterMessageColor = _FailColor;
            else
                CounterMessageColor = _PassColor;
        }

        public RelayCommand ResetMainWindowCommand { get; }
        public void ResetMainWindow()
        {
            //Basic Operation
            DrugRemainCounterText = CurrentPatientHN = PatientSearchChecker = CurrentICode = DrugListChecker = null;
            DrugImage = null; 
            Patient = null;
            DrugRemainCounter = 0;
            DrugRetrievalList.Clear();

            //Sub-Control Clear Method
            PatientAllergyViewModel.ResetAllergyControl();
            DrugInteractionViewModel.ResetDIControl();

            NoImageTextVisibility = _Visible;
            LoadingTextVisibility = PatientStatusMessageVisibility = PatientInfoVisibility = ICodeDataGridVisibility = ICodeStatusMessageVisibility = DrugImageVisibility =  _Collapsed;
            ToggleTextboxFocus();
            IsICodeInputEnable = false;
        }

        private void OnDataGridSelectedItem()
        {
            FetchDrugImg(SelectedDrug.ICode);
        }

        private void ChangeLoadingVisibility(Visibility visibility)
        {
            LoadingTextVisibility = visibility.ToString();
        }

        private void ClearSubControl()
        {
            PatientAllergyViewModel.ResetAllergyControl();
            DrugInteractionViewModel.ResetDIControl();
        }
        private void GetFocusAndClearHNTextBox()
        {
            CurrentICode = CurrentPatientHN = null;
            ToggleTextboxFocus();
        }
        private void ToggleTextboxFocus()
        {
            ShouldPatientHNFocus = !ShouldPatientHNFocus;
            ShouldICodeInputFocus = !ShouldICodeInputFocus;
        }
        private void FocusICodeTextBox()
        {
            ShouldICodeInputFocus = true;
        }
        private void FocusHNTextBox()
        {
            ShouldPatientHNFocus = true;
        }
        
        #endregion
        private enum SearchFlag
        {
            Found,
            NotFound,
            EmptyList,
            InputEmpty,
            Error
        }
    }
}