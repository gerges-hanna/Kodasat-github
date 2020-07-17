using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kodasat.Models
{
    [MetadataType(typeof(churchCustomize))]
    public partial class Church
    {

    }
    public class churchCustomize
    {
        public int Id { get; set; }
        public int CityID { get; set; }

        [Display(Name ="أسم الكنيسة")]
        public string churchName { get; set; }
        public string Status { get; set; }

        public virtual City City { get; set; }
    }
}