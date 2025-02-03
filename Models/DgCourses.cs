using System;
using System.ComponentModel.DataAnnotations;
namespace discgolf.Models
{

    //Klass för discgolfbanor
    public class DgCourses
    {

        //Banans id (optional)
        [Display(Name = "ID")]
        public int Id { get; set; }

        //Banans namn är ett krav
        [Required(ErrorMessage = "Ange banans namn")]
        [Display(Name = "Banans namn")]
        public required String CourseName { get; set; }

        //Denna är lite speciell då längden på denna int array bestämmer antalet korgar på banan.
        //Så på så sätt innehåller den båda antalkorgar och par för varje korg.
        [Required(ErrorMessage = "Ange par på varje hål")]
        [Display(Name = "Par")]
        public required int[] Basket { get; set; }

        //Valfritt att lägga till kordinater för banan men det är inte testat på denna webbplats.
        [Display(Name = "Kordinater")]
        public string? Location { get; set; }
    }
}