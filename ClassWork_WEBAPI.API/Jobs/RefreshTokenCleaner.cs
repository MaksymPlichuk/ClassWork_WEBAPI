using ClassWork_WEBAPI.BLL.Services;
using ClassWork_WEBAPI.DAL.Repositories;
using Quartz;

namespace ClassWork_WEBAPI.API.Jobs
{
    public class RefreshTokenCleaner : IJob
    {
        private readonly RefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenCleaner(RefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var tokens = _refreshTokenRepository.GetAll().ToList();
            foreach (var token in tokens)
            {
                if (DateTime.UtcNow - token.CreateDate >= TimeSpan.FromDays(7))
                {
                    await _refreshTokenRepository.DeleteAsync(token);
                    Console.WriteLine($"Deleted Token From {token.CreateDate}");
                }
            }
        }
    }
}
