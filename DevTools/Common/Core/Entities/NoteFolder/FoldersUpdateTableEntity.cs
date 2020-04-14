using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.SqlServer.Server;

namespace VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder
{
    public class FoldersUpdateTableEntity : List<FolderShortEntity>, IEnumerable<SqlDataRecord>, ITableValuedEntity
    {
        private const string TableTypeName = "[simple-note-service].[FoldersUpdateValue]";

        public FoldersUpdateTableEntity(IEnumerable<FolderShortEntity> values) : base(values)
        {
        }

        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var record = new SqlDataRecord(
                new SqlMetaData("Id", SqlDbType.Int),
                new SqlMetaData("Nodes", SqlDbType.NVarChar, -1)
            );
            foreach (var value in this)
            {
                record.SetInt32(0, value.Id);
                if (!string.IsNullOrWhiteSpace(value.Nodes))
                {
                    record.SetString(1, value.Nodes);
                }
                else
                {
                    record.SetDBNull(1);
                }

                yield return record;
            }
        }

        public SqlMapper.ICustomQueryParameter ToTable() => this.AsTableValuedParameter<SqlDataRecord>(TableTypeName);
    }
}