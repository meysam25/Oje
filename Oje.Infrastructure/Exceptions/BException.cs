using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Exceptions
{
    public class BException : Exception
    {
        public ApiResultErrorCode Code;
        public BMessages BMessages;
        public BException(string Message) :
            base(Message)
        {
            Code = ApiResultErrorCode.ValidationError;
        }
        public BException(string Message, ApiResultErrorCode Code) :
            base(Message)
        {
            this.Code = Code;
        }
        public BException(string Message, ApiResultErrorCode Code, BMessages BMessages) :
            base(Message)
        {
            this.Code = Code;
            this.BMessages = BMessages;
        }

        public static BException GenerateNewException(BMessages message, ApiResultErrorCode code)
        {
            return new BException(message.GetAttribute<DisplayAttribute>()?.Name, code);
        }

        public static BException GenerateNewException(BMessages message)
        {
            return new BException(message.GetAttribute<DisplayAttribute>()?.Name, ApiResultErrorCode.ValidationError);
        }

        public static BException GenerateNewException(string message)
        {
            return new BException(message, ApiResultErrorCode.ValidationError);
        }

        public static Exception GenerateNewException(object dublicate_Email, ApiResultErrorCode validationError)
        {
            throw new NotImplementedException();
        }
    }
}
