using System;
using System.ComponentModel.DataAnnotations;
namespace discgolf.Models
{
    public class DgCourses {
        //Egenskaper

        [Display(Name = "ID")]
        public required int Id { get; set; }
        
        [Display(Name = "Banans namn")]
        public required String CourseName { get; set; }


        [Display(Name = "Par")]
        public required int[] Basket { get; set; }


        [Display(Name = "Kordinater")]
        public string? Location { get; set; }

    }
}