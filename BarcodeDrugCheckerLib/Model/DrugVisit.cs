using BarcodeDrugCheckerLib.Model.Interface;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarcodeDrugCheckerLib.Model
{
    public class DrugVisit : Drug, IDrugVisit
    {
        public string VisitID { get; set; }
        public DateTime VisitDate { get; set; }
        public TimeSpan? VisitTime { get; set; }
        public DateTime RXDate { get; set; }
        public TimeSpan? RXTime { get; set; }
        private bool _BarcodeChecked;
        public bool BarcodeChecked 
        { 
            get 
            {
                return _BarcodeChecked;
            }
            set
            {
                _BarcodeChecked = value;
                OnPropertyChanged();
            } 
        }

        public DrugVisit()
        {
            BarcodeChecked = false;
        }

        public string VisitTimeFormat
        {
            get
            {
                string TimeFormat = "dd/MM/yyyy";
                DateTime CombinedDateTime = VisitDate;
                if(VisitTime != null)
                {
                    CombinedDateTime = VisitDate + (TimeSpan)VisitTime;
                    TimeFormat = "dd/MM/yyyy hh:mm:ss";
                }
                return CombinedDateTime.ToString(TimeFormat);
            }
        }

        public string RXTimeFormat
        {
            get
            {
                string TimeFormat = "dd/MM/yyyy";
                DateTime CombinedDateTime = RXDate;
                if (RXTime != null)
                {
                    CombinedDateTime = RXDate + (TimeSpan)RXTime;
                    TimeFormat = "dd/MM/yyyy hh:mm:ss";
                }
                return CombinedDateTime.ToString(TimeFormat);
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
