using Microsoft.AspNetCore.Mvc;
using VottingApp.API.Repositories;
using VottingApp.API.DTOs;

namespace VottingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteRepository _repository;

        public VoteController(IVoteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetOptions()
        {
            var options = await _repository.GetAllOptionsAsync();
            return Ok(options);
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetResults()
        {
            var results = await _repository.GetResultsAsync();
            return Ok(results);
        }

        [HttpGet("winner")]
        public async Task<IActionResult> GetWinner()
        {
            var winner = await _repository.GetWinnerAsync();
            if (winner == null)
                return Ok(new { Message = "No votes yet" });

            var totalVotes = await _repository.GetTotalVotesAsync();
            return Ok(new
            {
                Winner = winner,
                TotalVotes = totalVotes,
                Percentage = totalVotes > 0 ? (winner.VoteCount * 100.0 / totalVotes) : 0
            });
        }

        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromBody] VoteDto voteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repository.VoteAsync(voteDto.VoteOptionId, voteDto.VoterName);

            if (result)
                return Ok(new { Success = true, Message = "Vote recorded successfully" });

            return BadRequest(new { Success = false, Message = "You have already voted or option doesn't exist" });
        }

        [HttpPost("option")]
        public async Task<IActionResult> AddOption([FromBody] CreateOptionDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.Name))
                return BadRequest(new { Message = "Name is required" });

            var option = await _repository.AddOptionAsync(createDto.Name, createDto.Description ?? "");
            return Ok(new { Success = true, Option = option });
        }

        [HttpDelete("option/{id}")]
        public async Task<IActionResult> DeleteOption(int id)
        {
            var result = await _repository.DeleteOptionAsync(id);

            if (result)
                return Ok(new { Success = true, Message = "Option deleted" });

            return NotFound(new { Success = false, Message = "Option not found" });
        }

        [HttpGet("check-vote/{voterName}")]
        public async Task<IActionResult> CheckVote(string voterName)
        {
            var hasVoted = await _repository.HasVotedAsync(voterName);
            return Ok(new { HasVoted = hasVoted });
        }

        [HttpGet("total-votes")]
        public async Task<IActionResult> GetTotalVotes()
        {
            var total = await _repository.GetTotalVotesAsync();
            return Ok(new { TotalVotes = total });
        }
    }
}