namespace VottingApp.API.DTOs
{
    public class VoteDto
    {
        public int VoteOptionId { get; set; }
        public string VoterName { get; set; }
    }

    public class CreateOptionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class VoteResultDto
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }
}