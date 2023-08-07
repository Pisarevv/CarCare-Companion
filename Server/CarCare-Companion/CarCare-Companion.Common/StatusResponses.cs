namespace CarCare_Companion.Common;
public static class StatusResponses
{
    public const string GenericError = "An error occurred while processing your request. Please try again later.";

    public const string NoPermission = "You do not have the necessary permissions to access this resource.";

    public const string TokenExpired = "Token expired";

    public const string InvalidPassword = "Invalid password";

    public const string InvalidUser = "User does not exist";

    public const string InvalidData = "Invalid data provided.";

    public const string InvalidCredentials = "Login or password don't match";

    public const string MissingOrInvalidFields = "Invalid input fields";

    public const string UserEmailAlreadyExists = "A user with the same email already exists";

    public const string BadRequest = "Bad request";

    public const string FileSizeTooBig = "The size of the file is too big";

    public const string ResourceNotFound = "The resource does not exist";

    public const string Success = "Success";

    public const string AlreadyAdmin = "The user is already an administrator";



}
