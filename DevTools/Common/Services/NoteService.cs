using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Note;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface INoteService
    {
        Task<NotePagingResponse> GetItems(IOperation operation, NotePagingRequest request);
        Task DeleteNoteById(IOperation operation, int id);
    }

    public class NoteService : INoteService
    {
        private readonly INoteStore noteStore;

        public NoteService(INoteStore noteStore)
        {
            this.noteStore = noteStore;
        }

        public async Task<NotePagingResponse> GetItems(IOperation operation, NotePagingRequest request)
        {
            var (total, notes) = await noteStore.Get(operation, request);
            var noteList = notes.ToList();
            var projects = (await noteStore.GetNotesProjects(operation, noteList.Select(note => note.Id))).ToList();

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

        public async Task DeleteNoteById(IOperation operation, int id)
        {
            if (!await noteStore.IsNoteExist(operation, id))
            {
                throw CommonExceptions.NoteWasNotFound(operation, id);
            }

            await noteStore.DeleteNoteById(operation, id);
        }
    }
}