namespace Anonymous_Survey_Ardalis.Web.Files;

public class DownloadFileRequest
{
  public const string Route = "/Files/{FileId}/download";

  public int FileId { get; set; }
}
