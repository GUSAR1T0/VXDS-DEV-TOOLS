using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.SqlServer.Server;

namespace VXDesign.Store.DevTools.Core.Entities.Storage
{
    public abstract class SingleColumnTableEntity<TEntity> : List<TEntity>, IEnumerable<SqlDataRecord>
    {
        protected const string ColumnName = "Value";

        protected abstract string TableTypeName { get; }
        protected abstract SqlMetaData MetaData { get; }

        protected SingleColumnTableEntity(IEnumerable<TEntity> values) : base(values)
        {
        }

        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var record = new SqlDataRecord(MetaData);
            foreach (var value in this)
            {
                Apply(record, value);
                yield return record;
            }
        }

        protected abstract void Apply(SqlDataRecord record, TEntity value);

        internal SqlMapper.ICustomQueryParameter ToTable() => this.AsTableValuedParameter<SqlDataRecord>(TableTypeName);
    }

    public class IntSingleColumnTableEntity : SingleColumnTableEntity<int>
    {
        protected override string TableTypeName => "[base].[IntValue]";
        protected override SqlMetaData MetaData => new SqlMetaData(ColumnName, SqlDbType.Int);

        public IntSingleColumnTableEntity(IEnumerable<int> values) : base(values)
        {
        }

        protected override void Apply(SqlDataRecord record, int value) => record.SetInt32(0, value);
    }
    
    public class StringSingleColumnTableEntity : SingleColumnTableEntity<string>
    {
        protected override string TableTypeName => "[base].[StringValue]";
        protected override SqlMetaData MetaData => new SqlMetaData(ColumnName, SqlDbType.NVarChar, -1);

        public StringSingleColumnTableEntity(IEnumerable<string> values) : base(values)
        {
        }

        protected override void Apply(SqlDataRecord record, string value) => record.SetString(0, value);
    }
}