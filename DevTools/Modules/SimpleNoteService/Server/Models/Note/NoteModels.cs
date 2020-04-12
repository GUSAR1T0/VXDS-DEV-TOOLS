using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.SSP;
using VXDesign.Store.DevTools.Common.Core.Entities.Note;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.Note
{
    public class NoteModel : PagingResponseItemModel, IPagingResponseItemModel<NoteModel, NoteEntity>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EditTime { get; set; }
        public IEnumerable<NoteProjectModel> Projects { get; set; }

        public NoteModel ToModel(NoteEntity entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Text = entity.Text;
            UserId = entity.UserId;
            Color = entity.Color;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            EditTime = entity.EditTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek);
            Projects = entity.Projects.ToModel();
            return this;
        }
    }

    public class NotePagingFilterModel : PagingFilterModel, IPagingFilterModel<NotePagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Titles { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> ProjectIds { get; set; }
        public RangeFilterModel<DateTime> EditTimeRange { get; set; }

        public NotePagingFilter ToEntity() => new NotePagingFilter
        {
            Ids = Ids,
            Titles = Titles,
            UserIds = UserIds,
            ProjectIds = ProjectIds,
            EditTimeRange = EditTimeRange.ToEntity()
        };
    }

    public class NotePagingRequestModel : ServerSidePagingRequestModel<NotePagingFilterModel, NotePagingRequest>
    {
        public override NotePagingRequest ToEntity() => new NotePagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class NotePagingResponseModel : ServerSidePagingResponseModel<NoteModel, NotePagingResponseModel, NotePagingResponse>
    {
        public override NotePagingResponseModel ToModel(NotePagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new NoteModel().ToModel(item));
            return this;
        }
    }
}