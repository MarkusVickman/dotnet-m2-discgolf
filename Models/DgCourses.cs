using System;
using System.ComponentModel.DataAnnotations;
namespace discgolf.Models
{
    public class DgCourses {
        //Egenskaper

        [Display(Name = "ID")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ange banans namn")]
        [Display(Name = "Banans namn")]
        public required String CourseName { get; set; }

        [Required(ErrorMessage = "Ange par på varje hål")]
        [Display(Name = "Par")]
        public required int[] Basket { get; set; }


        [Display(Name = "Kordinater")]
        public string? Location { get; set; }

    }
}