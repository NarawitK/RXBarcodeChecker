using System.Collections.Generic;

namespace BarcodeDrugCheckerLib.Model.Interface
{
    public interface IPatient
    {
        int HN { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        List<Drug> MedError { get; set; }
    }
}
