using VottingApp.API.Models;
using VottingApp.API.DTOs;

namespace VottingApp.API.Repositories
{
    public interface IVoteRepository
    {
        Task<List<VoteOption>> GetAllOptionsAsync();
        Task<VoteOption> GetOptionByIdAsync(int id);
        Task<bool> VoteAsync(int optionId, string voterName);
        Task<List<VoteResultDto>> GetResultsAsync();
        Task<VoteOption> GetWinnerAsync();
        Task<int> GetTotalVotesAsync();
        Task<VoteOption> AddOptionAsync(string name, string description);
        Task<bool> DeleteOptionAsync(int id);
        Task<bool> HasVotedAsync(string voterName);
    }
}