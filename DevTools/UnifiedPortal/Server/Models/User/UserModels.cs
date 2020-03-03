using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.User
{
    public class UserModel : PagingResponseItemModel, IPagingResponseItemModel<UserModel, UserListItem>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }
        public int? UserRoleId { get; set; }
        public string UserRole { get; set; }
        public bool IsActivated { get; set; }

        public UserModel ToModel(UserListItem entity)
        {
            Id = entity.Id;
            Email = entity.Email;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Color = entity.Color;
            UserRoleId = entity.UserRoleId;
            UserRole = entity.UserRole;
            IsActivated = entity.IsActivated;
            return this;
        }
    }

    public class UserPagingFilterModel : PagingFilterModel, IPagingFilterModel<UserPagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> UserNames { get; set; }
        public IEnumerable<string> Emails { get; set; }
        public IEnumerable<int> UserRoleIds { get; set; }
        public bool? IsActivated { get; set; }

        public UserPagingFilter ToEntity() => new UserPagingFilter
        {
            Ids = Ids,
            UserNames = UserNames,
            Emails = Emails,
            UserRoleIds = UserRoleIds,
            IsActivated = IsActivated
        };
    }

    public class UserPagingRequestModel : ServerSidePagingRequestModel<UserPagingFilterModel, UserPagingRequest>
    {
        public override UserPagingRequest ToEntity() => new UserPagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class UserPagingResponseModel : ServerSidePagingResponseModel<UserModel, UserPagingResponseModel, UserPagingResponse>
    {
        public override UserPagingResponseModel ToModel(UserPagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new UserModel().ToModel(item));
            return this;
        }
    }

    public class UserShortModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}