using Sob_Need_Server.Models.Products;
using Models.Products;

namespace Sob_Need_Server.Interface
{
    public interface IRankingService
    {
        decimal CalculateScore(Product product);
        Task UpdateProductScoreAsync(Guid productId);
        Task UpdateAllScoresAsync();
    }
}
