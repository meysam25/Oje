using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IUploadedFileService
    {
        object GetBy(long? id);
        GridResultVM<UploadedFileMainGridResultVM> GetList(UploadedFileMainGrid searchInput);
    }
}
