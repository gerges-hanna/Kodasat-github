using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kodasat.CustomValidation;

namespace Kodasat.Models
{
    [MetadataType(typeof(kodasCustomize))]
    public partial class Koda
    {
        
    }
    public class kodasCustomize
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:D}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "يرجى تحديد تاريخ القداس")]
        [Display(Name = "التاريخ")]
        public System.DateTime Date { get; set; }

        [Display(Name = "الساعة")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage ="يرجى تحديد معاد القداس")]
        public Nullable<System.TimeSpan> Time { get; set; }

        [Display(Name ="عدد الأشخاص فى القداس")]
        [Required(ErrorMessage ="يرجى تحديد عدد الأشخاص المسموح لهم بالدخول")]
        //[DepositeLessCountAvailable(PeopleCount, PeopleDeposited)]
        public int PeopleCount { get; set; }

        [Display(Name ="تم حجز")]
        public int PeopleDeposited { get; set; }

        [Display(Name ="Church")]
        public int ChurchesID { get; set; }

        [Display(Name ="الكاهن")]
        public int fatherID { get; set; }

        [Display(Name = "الكنيسه")]
        public virtual Church Church { get; set; }
    }
}