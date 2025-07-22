using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;

namespace Sob_Need_Server.Interface.Location;

public interface IStateServices : IGenericService<StateDto, int>
{
    Task<ResponseDto<IEnumerable<StateDto>>> GetAllAsync(StateFilterDto Filter);

}
