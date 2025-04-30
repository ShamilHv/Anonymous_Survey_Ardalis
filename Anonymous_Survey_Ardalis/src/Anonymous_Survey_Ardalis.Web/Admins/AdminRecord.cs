using Anonymous_Survey_Ardalis.Core.AdminAggregate;

namespace Anonymous_Survey_Ardalis.Web.Admins;

public record AdminRecord(int AdminId, string AdminName, string AdminEmail, int? SubjectId, DateTime CreatedAt, AdminRole Role);
