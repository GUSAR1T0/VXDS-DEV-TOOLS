using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IFileStore
    {
        Task<int?> Find(IOperation operation, IFile file);
        Task<int> Upload(IOperation operation, UploadedFile file);
        Task<File> Download(IOperation operation, int fileId);
        Task<IEnumerable<File>> Download(IOperation operation, IEnumerable<int> fileIds);
    }

    public class FileStore : IFileStore
    {
        public async Task<int?> Find(IOperation operation, IFile file)
        {
            return await operation.Connection.QueryFirstOrDefaultAsync<int?>(new
            {
                file.Name,
                file.SourceExtension,
                file.Hash
            }, @"
                SELECT [Id]
                FROM [base].[File]
                WHERE
                    [Name] = @Name AND
                    [Extension] = @SourceExtension AND
                    [Hash] = @Hash;
            ");
        }

        public async Task<int> Upload(IOperation operation, UploadedFile file)
        {
            return await operation.Connection.QueryFirstOrDefaultAsync<int>(new
            {
                file.Name,
                file.SourceExtension,
                file.Extension,
                Content = Encoding.Unicode.GetBytes(file.Content),
                file.Hash
            }, @"
                INSERT INTO [base].[File] ([Name], [Extension], [ExtensionId], [Content], [Hash])
                OUTPUT INSERTED.[Id]
                VALUES (@Name, @SourceExtension, @Extension, @Content, @Hash);
            ");
        }

        private const string SelectFile = @"
            SELECT
                [Id],
                [Name],
                [Extension] AS [SourceExtension],
                [ExtensionId] AS [Extension],
                [Content] AS [ByteContent],
                [Time]
            FROM [base].[File]
        ";

        public async Task<File> Download(IOperation operation, int fileId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<File>(new { Id = fileId }, $@"
                {SelectFile}
                WHERE [Id] = @Id;
            ");
        }

        public async Task<IEnumerable<File>> Download(IOperation operation, IEnumerable<int> fileIds)
        {
            return await operation.Connection.QueryAsync<File>(new { Ids = fileIds }, $@"
                {SelectFile}
                WHERE [Id] IN @Ids;
            ");
        }
    }
}