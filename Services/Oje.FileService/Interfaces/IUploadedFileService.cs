using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oje.FileService.Models.DB;

namespace Oje.FileService.Interfaces
{
    public interface IUploadedFileService
    {
        string UploadNewFile(FileType fileType, IFormFile userPic, long? loginUserId, int? siteSettingId, long? objectId, string extensions, bool isAccessRequired, string objectIdStr = null);
        UploadedFile GetFile(string fn, long? userId, List<long> allChildUserId);
        int GetCountBy(long objectId, FileType fileType);
        object GetListBy(long objectId, FileType fileType, int skip, int take);
        void Delete(long? uploadFileId, int? siteSettingId, long? objectId, FileType fileType);
        bool IsValidImageSize(IFormFile mainImage, bool isWidthCheck, decimal relatedRateStart, decimal relatedRateEnd);
    }
}
