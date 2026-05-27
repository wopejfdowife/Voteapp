using Microsoft.EntityFrameworkCore;
using VottingApp.API.Data;
using VottingApp.API.Models;
using VottingApp.API.DTOs;

namespace VottingApp.API.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public VoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VoteOption>> GetAllOptionsAsync()
        {
            return await _context.VoteOptions.ToListAsync();
        }

        public async Task<VoteOption> GetOptionByIdAsync(int id)
        {
            return await _context.VoteOptions.FindAsync(id);
        }

        public async Task<bool> VoteAsync(int optionId, string voterName)
        {
            var option = await _context.VoteOptions.FindAsync(optionId);
            if (option == null)
                return false;

            if (await HasVotedAsync(voterName))
                return false;

            var vote = new Vote
            {
                VoteOptionId = optionId,
                VoterName = voterName,
                VotedAt = DateTime.Now
            };

            option.VoteCount++;

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<VoteResultDto>> GetResultsAsync()
        {
            var totalVotes = await GetTotalVotesAsync();
            var options = await GetAllOptionsAsync();

            return options.Select(o => new VoteResultDto
            {
                OptionId = o.Id,
                OptionName = o.Name,
                VoteCount = o.VoteCount,
                Percentage = totalVotes > 0 ? (o.VoteCount * 100.0 / totalVotes) : 0
            }).ToList();
        }

        public async Task<VoteOption> GetWinnerAsync()
        {
            return await _context.VoteOptions
                .OrderByDescending(o => o.VoteCount)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalVotesAsync()
        {
            return await _context.Votes.CountAsync();
        }

        public async Task<VoteOption> AddOptionAsync(string name, string description)
        {
            var option = new VoteOption
            {
                Name = name,
                Description = description,
                VoteCount = 0,
                CreatedAt = DateTime.Now
            };

            _context.VoteOptions.Add(option);
            await _context.SaveChangesAsync();
            return option;
        }

        public async Task<bool> DeleteOptionAsync(int id)
        {
            var option = await _context.VoteOptions.FindAsync(id);
            if (option == null)
                return false;

            _context.VoteOptions.Remove(option);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasVotedAsync(string voterName)
        {
            return await _context.Votes
                .AnyAsync(v => v.VoterName == voterName);
        }
    }
}