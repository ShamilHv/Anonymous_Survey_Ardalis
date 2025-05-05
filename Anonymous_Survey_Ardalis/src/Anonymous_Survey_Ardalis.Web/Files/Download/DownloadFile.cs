using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.UseCases.Files.Commands.Download;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Anonymous_Survey_Ardalis.Web.Files;

// Fixed class name (from DownlaodFile to DownloadFile)
public class DownloadFile(IMediator _mediator) : Endpoint<DownloadFileRequest, IActionResult>
{
  public override void Configure()
  {
    Get(DownloadFileRequest.Route);
    Roles("SuperAdmin", "DepartmentAdmin", "SubjectAdmin");
    Summary(s =>
    {
      s.Summary = "Download a file attachment";
      s.Description = "Downloads a file attachment based on the file ID and the admin's permission level";
      s.ExampleRequest = new DownloadFileRequest { FileId = 1 };
    });
  }

  public override async Task HandleAsync(
    DownloadFileRequest request,
    CancellationToken cancellationToken)
  {
    try
    {
      var result = await _mediator.Send(new DownloadFileCommand(request.FileId), cancellationToken);

      if (result.Status == ResultStatus.Forbidden)
      {
        await SendForbiddenAsync(cancellationToken);
        return;
      }

      if (result.IsSuccess && result.Value != null)
      {
        var fileBytes = await System.IO.File.ReadAllBytesAsync(result.Value.FilePath, cancellationToken);
        var contentType = GetContentType(result.Value.FilePath);
        
        // Return the physical file with proper Content-Type and download filename
        await SendBytesAsync(
            bytes: fileBytes,
            contentType: contentType,
            fileName: result.Value.FileName,
            cancellation: cancellationToken
        );
      }
      else
      {
        ThrowError(result.Errors.FirstOrDefault() ?? "Failed to download file");
        await SendErrorsAsync();
      }
    }
    catch (ResourceNotFoundException ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(404, cancellationToken);
    }
    catch (Exception ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(500, cancellationToken);
    }
  }

  // Helper method to determine the MIME type of a file
  private string GetContentType(string filePath)
  {
    var extension = Path.GetExtension(filePath).ToLowerInvariant();
    return extension switch
    {
      ".pdf" => "application/pdf",
      ".doc" => "application/msword",
      ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
      ".xls" => "application/vnd.ms-excel",
      ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      ".png" => "image/png",
      ".jpg" => "image/jpeg",
      ".jpeg" => "image/jpeg",
      ".gif" => "image/gif",
      ".txt" => "text/plain",
      _ => "application/octet-stream" // Default binary file type
    };
  }
}
