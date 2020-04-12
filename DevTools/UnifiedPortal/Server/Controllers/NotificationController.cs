using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Notification;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService notificationService;

        public NotificationController(IOperationService operationService, INotificationService notificationService) : base(operationService)
        {
            this.notificationService = notificationService;
        }

        /// <summary>
        /// Obtains information about all notifications
        /// </summary>
        /// <returns>Model of notifications data</returns>
        [ProducesResponseType(typeof(NotificationPagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpPost("list")]
        public async Task<ActionResult<NotificationPagingResponseModel>> GetNotifications([FromBody] NotificationPagingRequestModel model) => await Execute(async operation =>
        {
            var items = await notificationService.GetItems(operation, model.ToEntity());
            var response = new NotificationPagingResponseModel().ToModel(items);
            return response;
        });

        /// <summary>
        /// Creates new or updates existed notification
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageNotifications)]
        [HttpPut]
        public async Task<ActionResult> ModifyNotification([FromBody] NotificationUpdateModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity(UserId);
            await notificationService.ModifyNotification(operation, entity);
        });

        /// <summary>
        /// Removes an existed notification
        /// </summary>
        /// <param name="id">ID of a notification</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageNotifications)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserRole(int id) => await Execute(async operation => await notificationService.DeleteNotificationById(operation, id));
    }
}