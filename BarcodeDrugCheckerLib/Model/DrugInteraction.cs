using BarcodeDrugCheckerLib.Model.Interface;
namespace BarcodeDrugCheckerLib.Model
{
    public class DrugInteraction : IDrugInteraction
    {
        public string FirstDrug { get; set; }
        public string SecondDrug { get; set; }
        public string Note { get; set; }
    }
}
