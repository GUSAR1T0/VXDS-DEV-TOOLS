using System.Collections.Generic;
using Dapper;
using VXDesign.Store.DevTools.Core.Entities.Storage;

namespace VXDesign.Store.DevTools.Core.Extensions.Storage
{
    internal static class ListTableExtensions
    {
        internal static SqlMapper.ICustomQueryParameter ToIntTable(this IEnumerable<int> values) => new IntSingleColumnTableEntity(values).ToTable();
        internal static SqlMapper.ICustomQueryParameter ToStringTable(this IEnumerable<string> values) => new StringSingleColumnTableEntity(values).ToTable();
    }
}