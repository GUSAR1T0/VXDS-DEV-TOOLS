using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface INoteFolderService
    {
        #region Folders

        Task<FolderNode> GetFolders(IOperation operation);
        Task UpdateFolderPositions(IOperation operation, FolderShortNode node);
        Task<string> GetFolderName(IOperation operation, int folderId);
        Task<int> CreateFolder(IOperation operation, int folderId);
        Task UpdateFolder(IOperation operation, int folderId, string name);
        Task DeleteFolder(IOperation operation, int folderId);
        Task<AffectedNoteFolderEntity> GetAffectedFoldersAndNotes(IOperation operation, int folderId);

        #endregion

        #region Notes

        Task<NotePagingResponse> GetNotes(IOperation operation, NotePagingRequest request);
        Task<NoteExtendedEntity> GetNoteById(IOperation operation, int folderId, int noteId);
        Task ChangeNoteFolder(IOperation operation, int folderId, int noteId, int newFolderId);
        Task<int> CreateNote(IOperation operation, int folderId, int userId, NoteUpdateEntity entity);
        Task UpdateNote(IOperation operation, int folderId, int noteId, NoteUpdateEntity entity);
        Task DeleteNoteById(IOperation operation, int folderId, int noteId);

        #endregion
    }

    public class NoteFolderService : INoteFolderService
    {
        private readonly INoteFolderStore noteFolderStore;

        public NoteFolderService(INoteFolderStore noteFolderStore)
        {
            this.noteFolderStore = noteFolderStore;
        }

        #region Folders

        public async Task<FolderNode> GetFolders(IOperation operation)
        {
            var folders = (await noteFolderStore.GetFolders(operation)).ToList();
            return FillNode(folders, folders.FirstOrDefault(folder => folder.Id == 0));
        }

        private static FolderNode FillNode(IReadOnlyCollection<FolderEntity> folders, FolderEntity entity) => new FolderNode
        {
            Entity = entity,
            Nodes = entity.NodeList.Select(nodeId => FillNode(folders, folders.First(folder => folder.Id == nodeId)))
        };

        public async Task UpdateFolderPositions(IOperation operation, FolderShortNode node)
        {
            var folderWithChild = new Dictionary<int, IEnumerable<int>>();
            ExtractNode(folderWithChild, node);
            var folders = folderWithChild.Select(pair => new FolderShortEntity
            {
                Id = pair.Key,
                NodeList = pair.Value
            });
            await noteFolderStore.UpdateFolderPositions(operation, folders);
        }

        private static void ExtractNode(IDictionary<int, IEnumerable<int>> folderWithChild, FolderShortNode node)
        {
            var ids = new List<int>();
            foreach (var childNode in node.Nodes)
            {
                ExtractNode(folderWithChild, childNode);
                ids.Add(childNode.Id);
            }

            folderWithChild.Add(node.Id, ids);
        }

        public async Task<string> GetFolderName(IOperation operation, int folderId)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            return await noteFolderStore.GetFolderName(operation, folderId);
        }

        public async Task<int> CreateFolder(IOperation operation, int folderId)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            return await noteFolderStore.CreateFolder(operation, folderId);
        }

        public async Task UpdateFolder(IOperation operation, int folderId, string name)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            await noteFolderStore.UpdateFolder(operation, folderId, name);
        }

        public async Task DeleteFolder(IOperation operation, int folderId)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            var folders = (await noteFolderStore.GetFolders(operation)).ToList();
            var parent = folders.First(folder => folder.NodeList.Contains(folderId)).Id;
            var child = new List<int>();
            FindChild(folders, folders.First(folder => folder.Id == folderId), ref child);

            await noteFolderStore.DeleteFolder(operation, parent, folderId, child);
        }

        private static void FindChild(IReadOnlyCollection<FolderEntity> folders, FolderShortEntity entity, ref List<int> child)
        {
            foreach (var nodeId in entity.NodeList)
            {
                FindChild(folders, folders.First(folder => folder.Id == nodeId), ref child);
            }

            child.Add(entity.Id);
        }

        public async Task<AffectedNoteFolderEntity> GetAffectedFoldersAndNotes(IOperation operation, int folderId)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            var folders = (await noteFolderStore.GetFolders(operation)).ToList();
            var child = new List<int>();
            FindChild(folders, folders.First(folder => folder.Id == folderId), ref child);

            return new AffectedNoteFolderEntity
            {
                FoldersCount = child.Count > 0 ? child.Count - 1 : 0,
                NotesCount = await noteFolderStore.GetFolderNoteCount(operation, child)
            };
        }

        #endregion

        #region Notes

        public async Task<NotePagingResponse> GetNotes(IOperation operation, NotePagingRequest request)
        {
            var (total, notes) = await noteFolderStore.GetNotes(operation, request);
            var noteList = notes.ToList();
            var projects = (await noteFolderStore.GetNotesProjects(operation, noteList.Select(note => note.Id))).ToList();

            foreach (var note in noteList)
            {
                note.Projects = projects.Where(project => project.NoteId == note.Id).ToList();
            }

            return new NotePagingResponse
            {
                Total = total,
                Items = noteList
            };
        }

        public async Task<NoteExtendedEntity> GetNoteById(IOperation operation, int folderId, int noteId)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            if (!await noteFolderStore.IsNoteExist(operation, folderId, noteId))
            {
                throw CommonExceptions.NoteWasNotFound(operation, noteId);
            }

            var note = await noteFolderStore.GetNoteById(operation, noteId);
            note.Projects = await noteFolderStore.GetNotesProjects(operation, new[] { note.Id });
            return note;
        }

        public async Task ChangeNoteFolder(IOperation operation, int folderId, int noteId, int newFolderId)
        {
            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            if (!await noteFolderStore.IsNoteExist(operation, folderId, noteId))
            {
                throw CommonExceptions.NoteWasNotFound(operation, noteId);
            }

            if (!await noteFolderStore.IsFolderExist(operation, newFolderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, newFolderId);
            }

            await noteFolderStore.ChangeNoteFolder(operation, noteId, newFolderId);
        }

        public async Task<int> CreateNote(IOperation operation, int folderId, int userId, NoteUpdateEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                throw CommonExceptions.NoteTitleIsEmpty(operation);
            }

            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            return await noteFolderStore.CreateNote(operation, folderId, userId, entity);
        }

        public async Task UpdateNote(IOperation operation, int folderId, int noteId, NoteUpdateEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                throw CommonExceptions.NoteTitleIsEmpty(operation);
            }

            if (!await noteFolderStore.IsFolderExist(operation, folderId))
            {
                throw CommonExceptions.FolderWasNotFound(operation, folderId);
            }

            if (!await noteFolderStore.IsNoteExist(operation, folderId, noteId))
            {
                throw CommonExceptions.NoteWasNotFound(operation, noteId);
            }

            await noteFolderStore.UpdateNote(operation, noteId, entity);
        }

        public async Task DeleteNoteById(IOperation operation, int folderId, int noteId)
        {
            if (!await noteFolderStore.IsNoteExist(operation, folderId, noteId))
            {
                throw CommonExceptions.NoteWasNotFound(operation, noteId);
            }

            await noteFolderStore.DeleteNoteById(operation, noteId);
        }

        #endregion
    }
}