using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Note;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface INoteService
    {
        Task<NotePagingResponse> GetItems(IOperation operation, NotePagingRequest request);
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
            return new NotePagingResponse
            {
                Total = total,
                Items = notes
            };
        }
    }
}