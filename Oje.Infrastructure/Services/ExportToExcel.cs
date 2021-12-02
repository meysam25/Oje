using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class ExportToExcel
    {
        public static byte[] Export<T>(List<T> source)
        {
            if (source != null && source.Count > 0)
            {
                var workbook = new XSSFWorkbook();
                {
                    var ws = workbook.CreateSheet("MySheet");

                    var firstRow = source[0];
                    if (firstRow != null)
                    {
                        var allProps = firstRow.GetType().GetProperties();
                        List<string> headers = new List<string>();
                        var curRow = ws.CreateRow(0);
                        for (var i = 0; i < allProps.Length; i++)
                        {

                            var disPalayProperty = allProps[i].GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault();
                            if (disPalayProperty != null && !string.IsNullOrEmpty(disPalayProperty?.Name))
                            {
                                headers.Add(allProps[i].Name);
                                curRow.CreateCell(i).SetCellValue(disPalayProperty?.Name);
                            }
                        }

                        var row = 2;
                        foreach (var item in source)
                        {
                            if (item != null)
                            {
                                var newRow = ws.CreateRow(row - 1);
                                var allPropCurItem = item.GetType().GetProperties();
                                var colIndexRow = 0;
                                for (var i = 0; i < allPropCurItem.Length; i++)
                                {
                                    if (headers.Any(t => t == allPropCurItem[i].Name))
                                    {
                                        newRow.CreateCell(colIndexRow).SetCellValue(allPropCurItem[i].GetValue(item) + "");
                                        colIndexRow++;
                                    }
                                }
                                row++;
                            }

                        }
                        using (var ms = new MemoryStream())
                        {
                            workbook.Write(ms);

                            return ms.ToArray();
                        }
                    }
                }
            }

            return null;
        }

        public static List<T> ConvertToModel<T>(IFormFile file) where T : class, new()
        {
            List<T> result = new List<T>();

            if (file != null && file.Length > 0 && file.IsValidExtension(".xlsx") == true)
            {
                try
                {
                    XSSFWorkbook hssfwb = null;
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        ms.Position = 0;
                        hssfwb = new XSSFWorkbook(ms);
                    }
                    ISheet sheet = hssfwb.GetSheet("Sheet1");
                    if (sheet.LastRowNum >= 1)
                    {
                        for (int row = 1; row <= sheet.LastRowNum; row++)
                        {
                            var newRowItem = new T();
                            var props = newRowItem.GetType().GetProperties();

                            for (var j = 0; j < props.Length; j++)
                            {
                                var prop = props[j];

                                if (sheet.GetRow(row) != null)
                                {
                                    if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                                    {
                                        prop.SetValue(newRowItem, sheet.GetRow(row).GetCell(j).GetValueExcel().ToIntReturnZiro());
                                    }
                                    else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                                    {
                                        prop.SetValue(newRowItem, sheet.GetRow(row).GetCell(j).GetValueExcel().ToLongReturnZiro());
                                    }
                                    else if (prop.PropertyType == typeof(string))
                                    {
                                        prop.SetValue(newRowItem, sheet.GetRow(row).GetCell(j).GetValueExcel());
                                    }
                                    else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                                    {
                                        prop.SetValue(newRowItem, sheet.GetRow(row).GetCell(j).GetValueExcel().ToBooleanReturnFalse());
                                    }
                                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                                    {
                                        prop.SetValue(newRowItem, sheet.GetRow(row).GetCell(j).GetValueExcel().ToDecimalZiro());
                                    }
                                    else if (prop.PropertyType == typeof(List<int>))
                                    {
                                        var ids = sheet.GetRow(row).GetCell(j).GetValueExcel();
                                        if (!string.IsNullOrEmpty(ids))
                                        {
                                            var arrIds = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            if (arrIds != null && arrIds.Length > 0)
                                            {
                                                List<int> propTemp = new List<int>();
                                                foreach (var id in arrIds)
                                                {
                                                    if (id.ToIntReturnZiro() > 0)
                                                    {
                                                        propTemp.Add(id.ToIntReturnZiro());
                                                    }
                                                }
                                                prop.SetValue(newRowItem, propTemp);
                                            }
                                        }
                                    }
                                    else if (prop.PropertyType?.BaseType == typeof(ValueType))
                                    {
                                        Type tempType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                                        var enumTemp = Enum.ToObject(tempType, sheet.GetRow(row).GetCell(j).GetValueExcel().ToIntReturnZiro());
                                        if (enumTemp != null)
                                            prop.SetValue(newRowItem, enumTemp);

                                    }
                                }
                            }
                            result.Add(newRowItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex?.InnerException);
                }

            }

            return result;
        }
    }
}
