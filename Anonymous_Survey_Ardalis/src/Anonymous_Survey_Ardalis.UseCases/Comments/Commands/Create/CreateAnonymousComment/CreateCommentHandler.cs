using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.Get.GetWithComments;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using File = Anonymous_Survey_Ardalis.Core.CommentAggregate.File;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create;

public class CreateCommentHandler(IRepository<Comment> _repository, IRepository<Subject> subjectRepository, IRepository<File> _fileRepository)
  : ICommandHandler<CreateCommentCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
  {
    
    var spec = new SubjectByIdSpec(request.SubjectId);
    var subject = await subjectRepository.FirstOrDefaultAsync(spec, cancellationToken);
    if (subject == null)
    {
      throw new ResourceNotFoundException($"Subject with id {request.SubjectId}");    }
    var newComment = new Comment(request.SubjectId, request.CommentText);

    if (request.File != null)
    {
      var fileEntity = await UploadFileAsync(request.File, cancellationToken);
      await _fileRepository.AddAsync(fileEntity, cancellationToken);
      await _fileRepository.SaveChangesAsync(cancellationToken);

      newComment.FileId = fileEntity.FileId;
    }

    await _repository.AddAsync(newComment, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return newComment.CommentIdentifier;
  }

  private async Task<File> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
  {
    var desktopPath = "/home/shamil/Desktop";
    var uploadsFolder = Path.Combine(desktopPath, "uploads");

    if (!Directory.Exists(uploadsFolder))
    {
      Directory.CreateDirectory(uploadsFolder);
    }

    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await file.CopyToAsync(stream, cancellationToken);
    }

    return new File { FilePath = $"/uploads/{fileName}", UploadedAt = DateTime.UtcNow };
  }
  // public async Task<Result<int>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
  // {
  //   var newComment = new Comment(request.SubjectId, request.CommentText);
  //   if (request.File != null)
  //   {
  //     UploadFileAsync(request.File);
  //   }
  //
  //   await _repository.AddAsync(newComment, cancellationToken);
  //   await _repository.SaveChangesAsync(cancellationToken);
  //
  //   return await Task.FromResult(new Result<int>(newComment.Id));
  // }
  //
  // public async void UploadFileAsync(IFormFile file)
  // {
  //   var desktopPath = "/home/shamil/Desktop";
  //   var uploadsFolder = Path.Combine(desktopPath, "uploads");
  //
  //   if (!Directory.Exists(uploadsFolder))
  //   {
  //     Directory.CreateDirectory(uploadsFolder);
  //   }
  //
  //   var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
  //   var filePath = Path.Combine(uploadsFolder, fileName);
  //
  //   using (var stream = new FileStream(filePath, FileMode.Create))
  //   {
  //     await file.CopyToAsync(stream);
  //   }
  //
  //   var fileEntity = new File { FilePath = $"/uploads/{fileName}", UploadedAt = DateTime.UtcNow };
  //
  //   var addedFile = await _fileRepository.AddAsync(fileEntity);
  // }
}
