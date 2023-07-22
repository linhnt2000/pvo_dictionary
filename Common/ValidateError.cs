using System;

namespace thu6_pvo_dictionary.Common
{
    public class ValidateError : Exception
    {
        public int ErrorCode { get; set; }
        public ValidateError(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
