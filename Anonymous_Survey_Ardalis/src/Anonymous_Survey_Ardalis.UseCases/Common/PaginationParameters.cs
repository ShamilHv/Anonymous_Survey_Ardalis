namespace Anonymous_Survey_Ardalis.UseCases.Common;

public class PaginationParameters
{
  private const int MaxPageSize = 50;
  private int _pageSize = 10;

  public int PageNumber { get; set; } = 1;

  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
  }
}
