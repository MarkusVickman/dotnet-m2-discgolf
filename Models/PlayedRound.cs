using System;
namespace discgolf.Models
{
    public class PlayedRound
    {
        //Egenskaper
        public required int Id { get; set; }

        public required int Fk { get; set; }
        public required int[] Basket { get; set; }

    }
}