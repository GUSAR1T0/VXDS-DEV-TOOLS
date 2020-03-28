using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
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
        [PortalAuthentication]
        [HttpPost("list")]
        public async Task<ActionResult<NotificationPagingResponseModel>> GetNotifications([FromBody] NotificationPagingRequestModel model) => await Execute(async operation =>
        {
            var items = await notificationService.GetItems(operation, model.ToEntity());
            var response = new NotificationPagingResponseModel().ToModel(items);
            return response;
        });
    }
}