using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Exceptions
{
    public class BException : Exception
    {
        public ApiResultErrorCode Code;
        public BMessages BMessages;
        public long UserId = 0;
        public BException(string Message) :
            base(Message)
        {
            Code = ApiResultErrorCode.ValidationError;
        }
        public BException(string Message, ApiResultErrorCode Code, long UserId = 0) :
            base(Message)
        {
            this.Code = Code;
            this.UserId = UserId;
        }
        public BException(string Message, ApiResultErrorCode Code, BMessages BMessages, long UserId = 0) :
            base(Message)
        {
            this.Code = Code;
            this.BMessages = BMessages;
            this.UserId = UserId;
        }

        public static BException GenerateNewException(BMessages message, ApiResultErrorCode code, long UserId = 0)
        {
            return new BException(message.GetAttribute<DisplayAttribute>()?.Name, code, message, UserId);
        }

        public static BException GenerateNewException(BMessages message)
        {
            return new BException(message.GetAttribute<DisplayAttribute>()?.Name, ApiResultErrorCode.ValidationError, message);
        }

        public static BException GenerateNewException(string message, long UserId = 0)
        {
            return new BException(message, ApiResultErrorCode.ValidationError, UserId);
        }
    }
}
