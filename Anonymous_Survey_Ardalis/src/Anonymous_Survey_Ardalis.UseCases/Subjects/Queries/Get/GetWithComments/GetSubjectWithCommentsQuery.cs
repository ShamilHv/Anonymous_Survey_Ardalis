using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.Get.GetWithComments;

public record GetSubjectWithCommentsQuery(int Id) : IRequest<Result<SubjectWithCommentsDto>>;
