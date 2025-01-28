using System;
namespace discgolf.Models
{
    public class DgCourses {
        //Egenskaper

        public required int Id { get; set; }
        public required String CourseName { get; set; }

        public required int[] Basket { get; set; }

        public string? Location { get; set; }

    }
}