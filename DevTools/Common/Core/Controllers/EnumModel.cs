using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Common.Core.Controllers
{
    public class EnumModel
    {
        public string Value { get; set; }
        public string Name { get; set; }

        public static IEnumerable<EnumModel> GetEnumModelValues<T>() where T : Enum => EnumExtensions.GetValues<T>().Select(@enum => new EnumModel
        {
            Value = @enum.ToString("D"),
            Name = @enum.GetDescription()
        });
    }
}