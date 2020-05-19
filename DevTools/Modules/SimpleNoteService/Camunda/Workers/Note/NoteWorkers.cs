using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Camunda.Workers.Note
{
    public static class Note
    {
        [CamundaWorkerTopic("SimpleNoteService.Note.Notification")]
        public class NotificationWorker : CamundaWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.NoteId, Direction = CamundaVariableDirection.Input)]
            public int NoteId { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.UserIds, Direction = CamundaVariableDirection.Input)]
            public IEnumerable<int> UserIds { get; set; }

            private readonly IUserDataStore userDataStore;
            private readonly INoteFolderStore noteFolderStore;

            public NotificationWorker(IUserDataStore userDataStore, INoteFolderStore noteFolderStore)
            {
                this.userDataStore = userDataStore;
                this.noteFolderStore = noteFolderStore;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                logger.Debug($"[Operation: {operation.ComplexOperationId}, note: {NoteId}] Sending notifications for users").Wait();

                var note = noteFolderStore.GetNoteById(operation, NoteId).Result;
                var (_, users) = userDataStore.GetUsers(operation, new UserPagingRequest
                {
                    PageNo = 0,
                    PageSize = UserIds.Count(),
                    Filter = new UserPagingFilter
                    {
                        Ids = UserIds
                    }
                }).Result;

                foreach (var user in users)
                {
                    var message = $"[Operation: {operation.ComplexOperationId}, note: {NoteId}] Note \"{note.FolderName}\" -> \"{note.Title}\":";
                    message += $"\n- The following user: {user.FirstName}{(!string.IsNullOrWhiteSpace(user.LastName) ? $" {user.LastName}" : "")} <{user.Email}>";
                    message += $"\n- The text:\n{note.Text}";
                    logger.Info(message).Wait();
                }

                logger.Debug($"[Operation: {operation.ComplexOperationId}, note: {NoteId}] Sent notifications for users").Wait();
            }
        }
    }
}