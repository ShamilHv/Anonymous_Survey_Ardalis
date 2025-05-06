using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Files.Commands.Download;

public record DownloadFileCommand(int FileId) : ICommand<Result<FileDownloadDto>>;

public class FileDownloadDto
{
  public string FilePath { get; set; } = string.Empty;
  public string FileName { get; set; } = string.Empty;
}
