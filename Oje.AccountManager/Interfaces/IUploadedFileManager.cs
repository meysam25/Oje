using Oje.AccountService.Models.DB;
using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IUploadedFileService
    {
        string UploadNewFile(FileType fileType, IFormFile userPic, long? loginUserId, int? siteSettingId, long? objectId, string extensions, bool isAccessRequired, string objectIdStr = null);
        UploadedFile GetFile(string fn, long? userId);
        int GetCountBy(long objectId, FileType fileType);
        object GetListBy(long objectId, FileType fileType, int skip, int take);
        void Delete(long? uploadFileId, int? siteSettingId, long? objectId, FileType fileType);
    }
}
