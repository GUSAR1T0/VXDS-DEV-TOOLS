using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Authentication;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.Note;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : BaseApiController
    {
        private readonly INoteService noteService;

        public NoteController(IOperationService operationService, INoteService noteService) : base(operationService)
        {
            this.noteService = noteService;
        }

        /// <summary>
        /// Obtains information about all notes
        /// </summary>
        /// <returns>Model of operations with logs data</returns>
        [ProducesResponseType(typeof(NotePagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPost("list")]
        public async Task<ActionResult<NotePagingResponseModel>> GetNotes([FromBody] NotePagingRequestModel model) => await Execute(async operation =>
        {
            var items = await noteService.GetItems(operation, model.ToEntity());
            var response = new NotePagingResponseModel().ToModel(items);
            return response;
        });

        /// <summary>
        /// Removes an existed note
        /// </summary>
        /// <param name="id">ID of a note</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id) => await Execute(async operation => await noteService.DeleteNoteById(operation, id));
    }
}