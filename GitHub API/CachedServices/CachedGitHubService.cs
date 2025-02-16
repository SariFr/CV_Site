using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Service;
using Service.DataEntities;

namespace GitHub_API.CachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }
        private const string UserPortfolioKey = "UserPortfolioKey";
        public async Task<List<Portfolio>> GetPortfolio()
        {
            if (_memoryCache.TryGetValue(UserPortfolioKey, out List<Portfolio> Portfolio))
                return Portfolio;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));
            Portfolio= await _gitHubService.GetPortfolio();
            _memoryCache.Set(UserPortfolioKey, Portfolio,cacheOptions);
            return Portfolio;
        }

        public Task<List<Repository>> SearchRepositoriesAsync(string? userName, string? repoName, string? language)
        {
            return _gitHubService.SearchRepositoriesAsync(userName, repoName, language);    
        }
    }
}
