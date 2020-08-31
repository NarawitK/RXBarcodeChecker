namespace BarcodeDrugCheckerLib.Model.Interface
{
    public interface IDrug
    {
        string ICode { get; set; }
        string DrugName { get; set; }
        string GenericName { get; set; }
        int Quantity { get; set; }
        string Instruction { get; }
    }
}
