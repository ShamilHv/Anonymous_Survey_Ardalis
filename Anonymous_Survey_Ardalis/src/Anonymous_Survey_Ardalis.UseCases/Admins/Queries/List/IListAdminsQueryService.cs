using Anonymous_Survey_Ardalis.Core.AdminAggregate;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.List;

public interface IListAdminsQueryService
{
  Task<IEnumerable<AdminDto>> ListAsync();

}
