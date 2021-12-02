using Oje.AccountManager.Models.DB;
using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Interfaces
{
    public interface IUploadedFileManager
    {
        string UploadNewFile(FileType fileType, IFormFile userPic, long? loginUserId, int? siteSettingId, long? objectId, string extensions, bool isAccessRequired);
        UploadedFile GetFile(string fn, long? userId);
    }
}
