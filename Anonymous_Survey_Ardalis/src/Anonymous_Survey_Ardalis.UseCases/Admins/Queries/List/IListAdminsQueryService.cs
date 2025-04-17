namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.List;

public interface IListAdminsQueryService
{
  Task<IEnumerable<AdminDto>> ListAsync();
}
