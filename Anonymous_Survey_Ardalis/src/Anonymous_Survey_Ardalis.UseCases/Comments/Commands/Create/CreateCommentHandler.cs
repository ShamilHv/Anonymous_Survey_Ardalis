using System.Net;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.ContributorAggregate;
using Anonymous_Survey_Ardalis.UseCases.Contributors.Create;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using File = Anonymous_Survey_Ardalis.Core.CommentAggregate.File;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create;

public class CreateCommentHandler(IRepository<Comment> _repository, IRepository<File> _fileRepository)
  : ICommandHandler<CreateCommentCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
  {
    var newComment = new Comment(request.subjectId, request.commentText);
    if (request.file != null)
    {
       UploadFileAsync(request.file);
    }
    await _repository.AddAsync(newComment, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);
   
    return await Task.FromResult(new Result<int>(newComment.Id));
  }
  
  public async void UploadFileAsync(IFormFile file)
  {
    string desktopPath = "/home/shamil/Desktop";
    string uploadsFolder = Path.Combine(desktopPath, "uploads");

    if (!Directory.Exists(uploadsFolder))
    {
      Directory.CreateDirectory(uploadsFolder);
    }

    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
    string filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    var fileEntity = new File
    {
      FilePath = $"/uploads/{fileName}",
      UploadedAt = DateTime.UtcNow
    };

    File addedFile = await _fileRepository.AddAsync(fileEntity);
  }
}
