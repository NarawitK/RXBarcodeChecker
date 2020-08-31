using System.ComponentModel;
using BarcodeDrugCheckerLib.Model.Interface;
using Dapper.Contrib.Extensions;

namespace BarcodeDrugCheckerLib.Model
{
    public class Drug : IDrug
    {
        [Key]
        [DisplayName("รหัสยา")]
        public string ICode { get; set; }
        [DisplayName("ชื่อยา")]
        public string DrugName { get; set; }
        [DisplayName("ชื่อดั้งเดิม")]
        public string GenericName { get; set; }
        [DisplayName("ประเภทยา")]
        public string DosageForm { get; set; }
        [DisplayName("จำนวน")]
        public int Quantity { get; set; }
        public string DrugUnit { get; set; }
        public string DrugManualCode { get; set; }
        [DisplayName("วิธีการใช้ยาส่วนที่ 1")]
        public string Manual1 { get; set; }
        [DisplayName("วิธีการใช้ยาส่วนที่ 2")]
        public string Manual2 { get; set; }
        [DisplayName("วิธีการใช้ยาส่วนที่ 3")]
        public string Manual3 { get; set; }
        [DisplayName("วิธีการใช้ยา")]
        public string Instruction
        {
            get
            {
                return $"{Manual1} {Manual2} {Manual3}";
            }
        }
        [DisplayName("วิธีการใช้ยา")]
        public bool IsInjection
        {
            get
            {
                switch (DosageForm.ToLower())
                {
                    case "injections":
                    case "intravenous solution":
                        return true;
                    default:
                        return false;
                }
            }
        }

    }
}
