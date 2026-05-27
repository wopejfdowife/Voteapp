using System.ComponentModel.DataAnnotations;

namespace VottingApp.API.Models
{
    public class VoteOption
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int VoteCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}