using BarcodeDrugCheckerLib.Model.Interface;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BarcodeDrugCheckerLib.Model
{
    [Table("patient")]
    public class Patient : IPatient
    {
        [Key]
        public int HN { get; set; }
        public string Initials { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Drug> MedError { get; set; }

        public string FullName 
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
    }
}
