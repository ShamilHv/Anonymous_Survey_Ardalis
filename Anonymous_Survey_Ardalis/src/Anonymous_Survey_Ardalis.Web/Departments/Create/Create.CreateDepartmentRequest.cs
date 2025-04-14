using System.ComponentModel.DataAnnotations;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class CreateDepartmentRequest{
  public const string Route = "/Departments";

  [Required] public string DepartmentName { get; set; }= string.Empty;

}
