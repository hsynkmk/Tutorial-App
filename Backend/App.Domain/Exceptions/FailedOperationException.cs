namespace App.Domain.Exceptions;

public class FailedOperationException(string Message) : Exception(Message)
{
}