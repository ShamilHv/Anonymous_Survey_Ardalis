using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using Ardalis.SharedKernel;
using File = Anonymous_Survey_Ardalis.Core.CommentAggregate.File;


namespace Anonymous_Survey_Ardalis.UseCases.Files.Commands.Download;

public class DownloadFileHandler(
  IRepository<File> fileRepository,
  ICurrentUserService currentUserService,
  IAdminPermissionService adminPermissionService)
  : ICommandHandler<DownloadFileCommand, Result<FileDownloadDto>>
{
  public async Task<Result<FileDownloadDto>> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
  {
    var adminId = currentUserService.GetCurrentAdminId();

    // Check if admin has permission to download this file
    var hasPermission = await adminPermissionService.CanDownloadFile(adminId, request.FileId);
    if (!hasPermission)
    {
      return Result<FileDownloadDto>.Forbidden();
    }

    // Get the file entity
    var file = await fileRepository.GetByIdAsync(request.FileId, cancellationToken);
    if (file == null)
    {
      throw new ResourceNotFoundException($"File with id {request.FileId} not found");
    }

    // Construct the absolute path to the file on the disk
    var desktopPath = "/home/shamil/Desktop";
    var relativePath = file.FilePath.TrimStart('/');
    var fullPath = Path.Combine(desktopPath, relativePath);

    if (!System.IO.File.Exists(fullPath))
    {
      throw new ResourceNotFoundException($"Physical file not found at {fullPath}");
    }

    // Get the original file name from the path
    var fileName = Path.GetFileName(fullPath);

    return new FileDownloadDto { FilePath = fullPath, FileName = fileName };
  }
}
