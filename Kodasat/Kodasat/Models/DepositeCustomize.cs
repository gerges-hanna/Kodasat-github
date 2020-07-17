using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kodasat.Models
{
    [MetadataType(typeof(DepositeCustomize))]
    public partial class Deposited
    {
    }
    public class DepositeCustomize
    {
        public int Id { get; set; }

        [Display(Name ="National ID")]
        [Required(ErrorMessage ="يرجي ادخال 14 رقم ")]
        [MinLength(14, ErrorMessage = "عدد الأرقام أصغر من 14 رقم")]
        [MaxLength(14,ErrorMessage = "عدد الأرقام اكبر من 14 رقم ")]
        
        public string NationalID { get; set; }

        [Required(ErrorMessage ="يرجى أدخال الأسم الرباعى")]
        [DataType(DataType.Text)]
        
        public string fullName { get; set; }
        public int kodasID { get; set; }
        public System.DateTime AttendedOn { get; set; }
        public Nullable<int> FatherID { get; set; }

        [Display(Name ="Church")]
        public Nullable<int> churchesID { get; set; }
        public Nullable<int> NumberFriends { get; set; }

        public virtual Church Church { get; set; }
        public virtual Login Login { get; set; }
        public virtual Koda Koda { get; set; }
    }
}