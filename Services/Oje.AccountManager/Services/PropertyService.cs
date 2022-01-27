using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Services
{
    public class PropertyService : IPropertyService
    {
        readonly AccountDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public PropertyService
            (
                AccountDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult CreateUpdate(object input, int? siteSettingId, PropertyType type)
        {
            RemoveBy(type, siteSettingId);
            int fileIndex = 0;

            if (input != null && siteSettingId.ToIntReturnZiro() > 0)
            {
                var propbs = input.GetType().GetProperties();
                foreach (var prop in propbs)
                {
                    PropertyInputType inputType = PropertyInputType.Text;
                    var propValue = prop.GetValue(input);
                    if (propValue != null)
                    {
                        string value = "";
                        if (propValue.GetType() == typeof(string) || propValue.GetType() == typeof(MyHtmlString))
                            value = propValue.ToString();
                        else if ((propValue as IFormFile) != null)
                        {
                            var postedFile = propValue as IFormFile;
                            if (postedFile.Length > 0)
                            {
                                value = UploadedFileService.UploadNewFile(FileType.Property, postedFile, null, siteSettingId, null, ".jpg,.png,.jpeg,.svg", false, siteSettingId + "_" + type.ToString() + "_" + (++fileIndex));
                                inputType = PropertyInputType.File;
                            }
                        }
                        var disName = prop.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
                        if (!string.IsNullOrEmpty(value) && disName != null && !string.IsNullOrEmpty(disName.Name))
                        {
                            db.Entry(new Property()
                            {
                                Name = prop.Name,
                                Value = value,
                                SiteSettingId = siteSettingId.Value,
                                Title = disName.Name,
                                Type = type,
                                InputType = inputType
                            }).State = EntityState.Added;
                        }
                    }
                }
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public void RemoveBy(PropertyType type, int? siteSettingId)
        {
            var foundItems = db.Properties.Where(t => t.Type == type && t.SiteSettingId == siteSettingId).ToList();
            foreach (var foundItem in foundItems)
                db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public T GetBy<T>(PropertyType type, int? siteSettingId) where T : class, new()
        {
            var foundItems = db.Properties.Where(t => t.Type == type && t.SiteSettingId == siteSettingId).ToList();
            if (foundItems == null || foundItems.Count == 0)
                return null;

            T result = new();

            var tProbs = result.GetType().GetProperties();
            for (var i = 0; i < foundItems.Count; i++)
            {
                var foundProp = tProbs.Where(t => t.Name == (foundItems[i].InputType == PropertyInputType.File ? foundItems[i].Name + "_address" : foundItems[i].Name)).FirstOrDefault();
                if (foundProp != null)
                    if (foundItems[i].InputType == PropertyInputType.File)
                        foundProp.SetValue(result, GlobalConfig.FileAccessHandlerUrl + foundItems[i].Value);
                    else
                    {
                        if (foundProp.PropertyType == typeof(MyHtmlString))
                            foundProp.SetValue(result, (MyHtmlString)foundItems[i].Value);
                        else
                            foundProp.SetValue(result, foundItems[i].Value);
                    }
            }

            return result;
        }
    }
}
