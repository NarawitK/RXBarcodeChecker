using BarcodeDrugCheckerLib.Model.Interface;
namespace BarcodeDrugCheckerLib.Model
{
    public class PatientAllergy : IPatientAllergy
    {
        public string HN { get; set; }
        public string Agent { get; set; }
        public string Symptom { get; set; }
    }
}