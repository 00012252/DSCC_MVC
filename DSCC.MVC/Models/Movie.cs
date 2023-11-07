using System.ComponentModel.DataAnnotations;

namespace DSCC.MVC.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        [MinLength(5)]
        public string Title { get; set; } = string.Empty;
        [MinLength(15)]
        public string Description { get; set; } = string.Empty;
        [MinLength(3)]
        public string Genre { get; set; } = string.Empty;

    }
}
