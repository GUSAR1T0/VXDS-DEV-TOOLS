using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Authentication;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Controllers
{
    [Route("api/folder")]
    public class NoteFolderController : BaseApiController
    {
        private readonly INoteFolderService noteFolderService;

        public NoteFolderController(IOperationService operationService, INoteFolderService noteFolderService) : base(operationService)
        {
            this.noteFolderService = noteFolderService;
        }

        #region Folders

        /// <summary>
        /// Obtains all folders
        /// </summary>
        /// <returns>Model of folders</returns>
        [ProducesResponseType(typeof(FolderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpGet("list")]
        public async Task<ActionResult<FolderModel>> GetFolders() => await Execute(async operation =>
        {
            var folders = await noteFolderService.GetFolders(operation);
            return folders.ToModel();
        });

        /// <summary>
        /// Updates folders positions
        /// </summary>
        /// <param name="model">Model to update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPut("positions")]
        public async Task<ActionResult> UpdateFolderPositions([FromBody] FolderShortModel model) => await Execute(async operation =>
        {
            var node = model.ToEntity();
            await noteFolderService.UpdateFolderPositions(operation, node);
        });

        /// <summary>
        /// Obtains an existed folder name
        /// </summary>
        /// <param name="folderId">ID of a folder</param>
        /// <returns>Name of folder</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpGet("{folderId}")]
        public async Task<ActionResult<string>> GetFolderName(int folderId) => await Execute(async operation => await noteFolderService.GetFolderName(operation, folderId));

        /// <summary>
        /// Creates new folder
        /// </summary>
        /// <param name="folderId">ID of a parent folder</param>
        /// <returns>ID of new folder</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPost("{folderId}/new")]
        public async Task<ActionResult<int>> CreateFolder(int folderId) => await Execute(async operation => await noteFolderService.CreateFolder(operation, folderId));

        /// <summary>
        /// Updates an existed folder
        /// </summary>
        /// <param name="folderId">ID of a folder</param>
        /// <param name="name">New name of a folder</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPut("{folderId}")]
        public async Task<ActionResult> UpdateFolder(int folderId, [FromQuery(Name = "n")] string name) => await Execute(async operation =>
        {
            await noteFolderService.UpdateFolder(operation, folderId, name);
        });

        /// <summary>
        /// Removes an existed folder
        /// </summary>
        /// <param name="folderId">ID of a folder</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpDelete("{folderId}")]
        public async Task<ActionResult> DeleteFolder(int folderId) => await Execute(async operation => await noteFolderService.DeleteFolder(operation, folderId));

        /// <summary>
        /// Obtains counts of affected folders and notes
        /// </summary>
        /// <param name="folderId">ID of a folder</param>
        /// <returns>Counts of affected items</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpGet("{folderId}/affected/count")]
        public async Task<ActionResult<AffectedNoteFolderModel>> GetAffectedFoldersAndNotes(int folderId) => await Execute(async operation =>
        {
            var entity = await noteFolderService.GetAffectedFoldersAndNotes(operation, folderId);
            return entity.ToModel();
        });

        #endregion

        #region Notes

        /// <summary>
        /// Obtains information about all notes
        /// </summary>
        /// <param name="folderId">ID of a folder</param>
        /// <param name="model">Request to get notes</param>
        /// <returns>Model of operations with logs data</returns>
        [ProducesResponseType(typeof(NotePagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPost("{folderId}/note/list")]
        public async Task<ActionResult<NotePagingResponseModel>> GetNotes(int folderId, [FromBody] NotePagingRequestModel model) => await Execute(async operation =>
        {
            var items = await noteFolderService.GetNotes(operation, model.ToEntity(folderId));
            var response = new NotePagingResponseModel().ToModel(items);
            return response;
        });

        /// <summary>
        /// Obtains an existed note
        /// </summary>
        /// <param name="folderId">ID of a note folder</param>
        /// <param name="noteId">ID of a note</param>
        /// <returns>Model of note</returns>
        [ProducesResponseType(typeof(NoteModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpGet("{folderId}/note/{noteId}")]
        public async Task<ActionResult<NoteExtendedModel>> GetNote(int folderId, int noteId) => await Execute(async operation =>
        {
            var entity = await noteFolderService.GetNoteById(operation, folderId, noteId);
            return entity.ToModel();
        });

        /// <summary>
        /// Changes some folder for an existed note
        /// </summary>
        /// <param name="folderId">ID of a note folder</param>
        /// <param name="noteId">ID of a note</param>
        /// <param name="newFolderId">ID of new folder for the note</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(typeof(NoteModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPut("{folderId}/note/{noteId}/replace")]
        public async Task<ActionResult> ChangeNoteFolder(int folderId, int noteId, [FromQuery(Name = "f")] int newFolderId) => await Execute(async operation =>
        {
            await noteFolderService.ChangeNoteFolder(operation, folderId, noteId, newFolderId);
        });

        /// <summary>
        /// Creates new note
        /// </summary>
        /// <param name="folderId">ID of a folder</param>
        /// <param name="model">Model of note</param>
        /// <returns>ID of new note</returns>
        [ProducesResponseType(typeof(NoteModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPost("{folderId}/note")]
        public async Task<ActionResult<int>> CreateNote(int folderId, [FromBody] NoteUpdateModel model) => await Execute(async operation =>
        {
            return await noteFolderService.CreateNote(operation, folderId, UserId ?? 0, model.ToEntity());
        });

        /// <summary>
        /// Updates an existed note
        /// </summary>
        /// <param name="folderId">ID of a note folder</param>
        /// <param name="noteId">ID of a note</param>
        /// <param name="model">Model of note</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(typeof(NoteModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPut("{folderId}/note/{noteId}")]
        public async Task<ActionResult<int>> UpdateNote(int folderId, int noteId, [FromBody] NoteUpdateModel model) => await Execute(async operation =>
        {
            await noteFolderService.UpdateNote(operation, folderId, noteId, model.ToEntity());
        });

        /// <summary>
        /// Removes an existed note
        /// </summary>
        /// <param name="folderId">ID of a note folder</param>
        /// <param name="noteId">ID of a note</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpDelete("{folderId}/note/{noteId}")]
        public async Task<ActionResult> DeleteNote(int folderId, int noteId) => await Execute(async operation => await noteFolderService.DeleteNoteById(operation, folderId, noteId));

        /// <summary>
        /// Sends notifications about an existed note
        /// </summary>
        /// <param name="folderId">ID of a note folder</param>
        /// <param name="noteId">ID of a note</param>
        /// <param name="userIds">List of user IDs</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToModule)]
        [HttpPut("{folderId}/note/{noteId}/notify")]
        public async Task<ActionResult> SendNotificationsAboutNote(int folderId, int noteId, [FromQuery(Name = "u")] IEnumerable<int> userIds) => await Execute(async operation =>
        {
            await noteFolderService.SendNotificationsAboutNote(operation, folderId, noteId, userIds);
        });

        #endregion
    }
}