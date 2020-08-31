using System;
using System.ComponentModel;

namespace BarcodeDrugCheckerLib.Model.Interface
{
    public interface IDrugVisit : IDrug, INotifyPropertyChanged
    {
        string VisitID { get; set; }
        DateTime VisitDate { get; set; }
        TimeSpan? VisitTime { get; set; }
        DateTime RXDate { get; set; }
        TimeSpan? RXTime { get; set; }
        string VisitTimeFormat { get; }
        string RXTimeFormat { get; }
        bool BarcodeChecked { get; set; }
    }
}
