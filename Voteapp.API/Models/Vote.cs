using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VottingApp.API.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int VoteOptionId { get; set; }

        [Required]
        public string VoterName { get; set; }

        public DateTime VotedAt { get; set; }

        [JsonIgnore]
        public virtual VoteOption VoteOption { get; set; }
    }
}