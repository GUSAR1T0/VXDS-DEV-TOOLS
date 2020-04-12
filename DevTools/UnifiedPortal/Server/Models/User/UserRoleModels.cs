using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.SSP;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.User
{
    public class UserRoleShortInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRolePermissionModel
    {
        public int PermissionGroupId { get; set; }
        public IEnumerable<long> Permissions { get; set; }
    }

    public class UserRoleFullInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserRolePermissionModel> Permissions { get; set; }
    }

    public class UserRoleListItemModel : PagingResponseItemModel, IPagingResponseItemModel<UserRoleListItemModel, UserRoleListItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserRolePermissionModel> Permissions { get; set; }
        public int UserCount { get; set; }
 
        public UserRoleListItemModel ToModel(UserRoleListItem entity) => entity.ToModel();
    }

    public class UserRolePagingFilterModel : PagingFilterModel, IPagingFilterModel<UserRolePagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> UserRoleNames { get; set; }

        public UserRolePagingFilter ToEntity() => new UserRolePagingFilter
        {
            Ids = Ids,
            UserRoleNames = UserRoleNames
        };
    }

    public class UserRolePagingRequestModel : ServerSidePagingRequestModel<UserRolePagingFilterModel, UserRolePagingRequest>
    {
        public override UserRolePagingRequest ToEntity() => new UserRolePagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class UserRolePagingResponseModel : ServerSidePagingResponseModel<UserRoleListItemModel, UserRolePagingResponseModel, UserRolePagingResponse>
    {
        public override UserRolePagingResponseModel ToModel(UserRolePagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new UserRoleListItemModel().ToModel(item));
            return this;
        }
    }
}