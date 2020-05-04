using System;
using Microsoft.AspNetCore.Http;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Entities;
using VXDesign.Store.DevTools.Common.Core.Entities.File;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class ModelExtensions
    {
        public static string FormatDateTime(this DateTime? dateTime, string format = null)
        {
            return dateTime != null && !string.IsNullOrWhiteSpace(format) ? dateTime.Value.ToString(format) : string.Empty;
        }

        public static string FormatDateTime(this DateTime dateTime, string format = null)
        {
            return !string.IsNullOrWhiteSpace(format) ? dateTime.ToString(format) : string.Empty;
        }

        public static RangeFilter<T> ToEntity<T>(this RangeFilterModel<T> model) => model != null ? new RangeFilter<T>
        {
            Min = model.Min,
            Max = model.Max
        } : null;

        public static UploadedFile ToEntity(this IFormFile file) => UploadedFile.Transform(file);

        public static FileModel ToModel(this File file) => new FileModel
        {
            Name = file.Name,
            Extension = file.Extension,
            Content = file.Content,
            Time = file.Time.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
        };
    }
}