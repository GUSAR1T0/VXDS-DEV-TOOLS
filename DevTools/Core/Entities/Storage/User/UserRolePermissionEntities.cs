using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.SqlServer.Server;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.User
{
    internal class UserRolePermissionEntities : List<UserRolePermissionEntity>, IEnumerable<SqlDataRecord>
    {
        private const string TableTypeName = "[authentication].[UserRolePermissionItem]";

        private static SqlMetaData[] MetaData => new []
        {
            new SqlMetaData("PermissionGroupId", SqlDbType.Int),
            new SqlMetaData("Permissions", SqlDbType.BigInt)
        };

        internal UserRolePermissionEntities(IEnumerable<UserRolePermissionEntity> values) : base(values)
        {
        }

        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var record = new SqlDataRecord(MetaData);
            foreach (var value in this)
            {
                record.SetInt32(0, value.PermissionGroupId);
                record.SetInt64(1, value.Permissions);
                yield return record;
            }
        }

        internal SqlMapper.ICustomQueryParameter ToTable() => this.AsTableValuedParameter<SqlDataRecord>(TableTypeName);
    }
}