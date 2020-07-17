using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kodasat.Models
{
    [MetadataType(typeof(customizeLogin))]
    public partial class  Login
    {

    }

    public class customizeLogin
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="أسم الكاهن")]
        public string Name { get; set; }
        public string Type { get; set; }

        [Display(Name ="Church")]
        public int ChurchesID { get; set; }
    }

}