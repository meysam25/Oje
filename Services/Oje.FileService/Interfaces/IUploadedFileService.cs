using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using Oje.FileService.Models.DB;
using Oje.FileService.Models.View;

namespace Oje.FileService.Interfaces
{
    public interface IUploadedFileService
    {
        string UploadNewFile(FileType fileType, IFormFile userPic, long? loginUserId, int? siteSettingId, long? objectId, string extensions, bool isAccessRequired, string objectIdStr = null, string title = null, long? userId = null);
        UploadedFile GetFile(string fn, long? userId);
        int GetCountBy(long objectId, FileType fileType);
        int GetCountBy(long objectId, FileType fileType, int? siteSettingId);
        object GetListBy(long objectId, FileType fileType, int skip, int take);
        object GetListBy(long objectId, FileType fileType, int skip, int take, int? siteSettingId);
        void Delete(long? uploadFileId, int? siteSettingId, long? objectId, FileType fileType);
        bool IsValidImageSize(IFormFile mainImage, bool isWidthCheck, decimal relatedRateStart, decimal relatedRateEnd);
        object Delete(long? id, int? siteSettingId);
        object GetList(UploadedFileMainGrid searchInput, int? siteSettingId);
        UploadedFile GetFile(string fn);
    }
}
