namespace BarcodeDrugCheckerLib.Model.Interface
{
    public interface IDrugInteraction
    {
        string FirstDrug { get; set; }
        string SecondDrug { get; set; }
        string Note { get; set; }
    }
}
