using CG.Web.MegaApiClient;
using Oje.BackupService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;

namespace Oje.BackupService.Services
{
    public class MegaService : IMegaService
    {
        readonly IGoogleBackupArchiveLogService GoogleBackupArchiveLogService = null;
        public MegaService
            (
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService
            )
        {
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
        }

        public async Task Delete(string fileId)
        {
            if (!string.IsNullOrEmpty(fileId))
            {
                MegaApiClient client = new MegaApiClient();

                try
                {
                    await client.LoginAsync(GlobalConfig.Configuration["megaUser:username"], GlobalConfig.Configuration["megaUser:password"]);
                    IEnumerable<INode> nodes = await client.GetNodesAsync();
                    var foundNode = getNodeById(nodes, nodes.Where(t => t.Type == NodeType.Root).FirstOrDefault(), fileId);
                    if (foundNode != null)
                        client.Delete(foundNode, false);


                    await client.LogoutAsync();
                }
                catch (Exception ex)
                {
                    Exception exTemp = ex;
                    string message = exTemp.Message;

                    while (exTemp.InnerException != null)
                    {
                        exTemp = exTemp.InnerException;
                        message += Environment.NewLine + exTemp.Message;
                    }

                    GoogleBackupArchiveLogService.Create(message, GoogleBackupArchiveLogType.RemoveExpiredFile);
                }
                finally
                {
                    if (client != null && client.IsLoggedIn)
                        try { await client.LogoutAsync(); } catch { }
                }

            }
        }

        INode getNodeById(IEnumerable<INode> nodes, INode parent, string Id)
        {
            if (nodes == null || nodes.Count() == 0 || parent == null || string.IsNullOrEmpty(Id))
                return null;

            IEnumerable<INode> children = nodes.Where(x => x.ParentId == parent.Id);
            foreach (INode child in children)
            {
                if (child.Id == Id)
                    return child;
            }

            return null;
        }


        public async Task uploadFile(string todayFileName, IGoogleBackupArchiveService GoogleBackupArchiveService)
        {
            MegaApiClient client = new MegaApiClient();
            try
            {
                await client.LoginAsync(GlobalConfig.Configuration["megaUser:username"], GlobalConfig.Configuration["megaUser:password"]);
                IEnumerable<INode> nodes = await client.GetNodesAsync();
                INode root = nodes.Single(x => x.Type == NodeType.Root);
                INode myFile = client.UploadFile(todayFileName, root);
                GoogleBackupArchiveService.Create(myFile.Id, myFile.Size, GoogleBackupArchiveType.MEGA);
                GoogleBackupArchiveLogService.Create(BMessages.Operation_Was_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.UploadSectionMega);
                await client.LogoutAsync();
                
            }
            catch (Exception ex)
            {
                Exception exTemp = ex;
                string message = exTemp.Message;

                while (exTemp.InnerException != null)
                {
                    exTemp = exTemp.InnerException;
                    message += Environment.NewLine + exTemp.Message;
                }

                GoogleBackupArchiveLogService.Create(message, GoogleBackupArchiveLogType.UploadSectionMega);
            }
            finally
            {
                if (client != null && client.IsLoggedIn)
                    try { await client.LogoutAsync(); } catch { }
                client = null;
            }
        }
    }
}
