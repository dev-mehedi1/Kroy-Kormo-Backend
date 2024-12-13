using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface
{
    public interface IGenericService<T, TKey>
    {
        Task<ResponseDto<T>> GetByIdAsync(TKey id);
        Task<ResponseDto<T>> CreateAsync(T entity);
        Task<ResponseDto<T>> UpdateAsync(TKey id, T entity);
        Task<ResponseDto<bool>> DeleteAsync(TKey id);
    }
}
