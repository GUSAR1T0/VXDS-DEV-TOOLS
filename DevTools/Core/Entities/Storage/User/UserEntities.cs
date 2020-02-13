using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Entities.Storage.SSP;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.User
{
    public class UserListItem : UserEntity, IPagingResponseItemEntity
    {
        public string UserRole { get; set; }
        public bool IsActivated { get; set; }
    }

    public class UserPagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> UserNames { get; set; }
        public IEnumerable<string> Emails { get; set; }
        public IEnumerable<int> UserRoleIds { get; set; }
        public bool? IsActivated { get; set; }
    }

    public class UserPagingRequest : ServerSidePagingRequest<UserPagingFilter>
    {
    }

    public class UserPagingResponse : ServerSidePagingResponse<UserListItem>
    {
    }

    public class UserShortEntity : IDataEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}