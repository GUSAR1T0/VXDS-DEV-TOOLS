using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Notification;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface INotificationService
    {
        Task<NotificationPagingResponse> GetItems(IOperation operation, NotificationPagingRequest request);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationStore notificationStore;

        public NotificationService(INotificationStore notificationStore)
        {
            this.notificationStore = notificationStore;
        }

        public async Task<NotificationPagingResponse> GetItems(IOperation operation, NotificationPagingRequest request)
        {
            var (total, notifications) = await notificationStore.Get(operation, request);
            return new NotificationPagingResponse
            {
                Total = total,
                Items = notifications
            };
        }
    }
}