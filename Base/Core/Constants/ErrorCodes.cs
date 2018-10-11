using Core.Contracts;

namespace Core.Constants
{
    public class ErrorCodes
    {
        public static ErrorCode ErrorTokenParse = new ErrorCode { Code = 1, Message = "Could not parse auth token" };
        public static ErrorCode ErrorInvalidInput(string message) { return new ErrorCode { Code = 2, Message = message }; }
        public static ErrorCode ErrorNonceThrottle = new ErrorCode { Code = 3, Message = "Hit nonce requested limit" };
        public static ErrorCode ErrorNonceInvalid = new ErrorCode { Code = 4, Message = "Invalid Nonce" };
        public static ErrorCode ErrorUserNotFound = new ErrorCode { Code = 5, Message = "User Not Found" };
        public static ErrorCode ErrorCouldNotCreate = new ErrorCode { Code = 6, Message = "Could not create resource" };
        public static ErrorCode ErrorCouldNotDelete = new ErrorCode { Code = 7, Message = "Could not delete resource" };
        public static ErrorCode ErrorCouldNotUpdate = new ErrorCode { Code = 8, Message = "Could not update resource" };
        public static ErrorCode ErrorUploadStatusCode(string message, int status) { return new ErrorCode { Code = 9, Message = message, StatusCode = status }; }
        public static ErrorCode ErrorCouldNotSendEmail = new ErrorCode { Code = 10, Message = "Could not send Email" };
    }
}
