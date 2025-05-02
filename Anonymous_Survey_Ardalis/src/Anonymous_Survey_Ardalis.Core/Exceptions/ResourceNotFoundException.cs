namespace Anonymous_Survey_Ardalis.Core.Exceptions;

public class ResourceNotFoundException : Exception
{
  public ResourceNotFoundException(string resourceName)
    : base($"Resourse with given value {resourceName} was not found.")
  {
  }
}
