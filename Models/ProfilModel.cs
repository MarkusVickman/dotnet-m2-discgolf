using System;
using System.ComponentModel.DataAnnotations;
namespace discgolf.Models
{

  //klass med modell för användarprofiler
  public class ProfileModel
  {

    //namn är ett krav
    [Required(ErrorMessage = "Ange ditt namn")]
    [Display(Name = "Namn")]
    public required String Name { get; set; }

    //valfritt att ange pdga nummer då inte alla spelare har det
    [Display(Name = "PDGA nummer")]
    public int? Pdga { get; set; }

    //Email är ett krav
    [EmailAddress]
    [Required(ErrorMessage = "Ange din Emailadress")]
    [Display(Name = "Epostadress")]
    public required string Email { get; set; }

    //Url till bildlänk för din visningsbild är valfritt
    [Url]
    [Display(Name = "URL till visningsbild")]
    public string? Url { get; set; }

    //Valfritt att ange telefonnummer
    [Phone]
    [Display(Name = "Telefonnummer")]
    public string? Phonenr { get; set; }

  }
}