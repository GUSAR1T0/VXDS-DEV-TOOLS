using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Entities.User
{
    public class UserListItem : UserEntity, IPagingResponseItemEntity
    {
        public int? UserRoleId { get; set; }
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