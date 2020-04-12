using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Note
{
    public class NoteEntity : IDataEntity, IPagingResponseItemEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EditTime { get; set; }
        public IEnumerable<NoteProjectEntity> Projects { get; set; }
    }

    public class NotePagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Titles { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> ProjectIds { get; set; }
        public RangeFilter<DateTime> EditTimeRange { get; set; }
    }

    public class NotePagingRequest : ServerSidePagingRequest<NotePagingFilter>
    {
    }

    public class NotePagingResponse : ServerSidePagingResponse<NoteEntity>
    {
    }
}