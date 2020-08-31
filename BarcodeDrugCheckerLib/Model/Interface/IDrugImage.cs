namespace BarcodeDrugCheckerLib.Model.Interface
{
    public interface IDrugImage
    {
        string ICode { get; set; }
        string Name { get; set; }
        byte[] ImageBytes { get; set; }
    }
}
