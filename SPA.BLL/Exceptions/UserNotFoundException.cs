namespace SPA.BLL.Exceptions;

public class UserNotFoundException(string message) : CustomException(message);