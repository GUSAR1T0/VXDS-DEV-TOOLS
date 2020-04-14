using System.Collections.Generic;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities;
using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions
{
    internal static class ListTableExtensions
    {
        #region Single Column Tables

        internal static SqlMapper.ICustomQueryParameter ToIntTable(this IEnumerable<int> values) => new IntSingleColumnTableEntity(values).ToTable();
        internal static SqlMapper.ICustomQueryParameter ToStringTable(this IEnumerable<string> values) => new StringSingleColumnTableEntity(values).ToTable();

        #endregion

        #region Custom Tables

        internal static SqlMapper.ICustomQueryParameter ToTable(this IEnumerable<FolderShortEntity> values) => new FoldersUpdateTableEntity(values).ToTable();

        #endregion
    }
}