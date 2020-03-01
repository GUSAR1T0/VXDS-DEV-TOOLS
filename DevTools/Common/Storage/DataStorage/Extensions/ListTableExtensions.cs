using System.Collections.Generic;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions
{
    internal static class ListTableExtensions
    {
        internal static SqlMapper.ICustomQueryParameter ToIntTable(this IEnumerable<int> values) => new IntSingleColumnTableEntity(values).ToTable();
        internal static SqlMapper.ICustomQueryParameter ToStringTable(this IEnumerable<string> values) => new StringSingleColumnTableEntity(values).ToTable();
    }
}