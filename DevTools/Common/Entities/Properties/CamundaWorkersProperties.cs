using System;
using VXDesign.Store.DevTools.Common.Attributes;

namespace VXDesign.Store.DevTools.Common.Entities.Properties
{
    public abstract class CamundaWorkersProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Syrinx")]
        public SyrinxProperties SyrinxProperties { get; set; }

        #region Fetch and Lock

        [PropertyField]
        public int FetchTimeout { get; set; } = (int) TimeSpan.FromSeconds(1).TotalMilliseconds;

        [PropertyField]
        public bool UsePriority { get; set; } = true;

        [PropertyField]
        public string WorkerKeyword { get; set; }

        [PropertyField(Key = "RetriesAfterFetch")]
        public int CountOfRetriesWhenFetchIsUnsuccessful { get; set; } = 5;

        [PropertyField]
        public int RetryAfterFetchTimeout { get; set; } = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;

        #endregion

        #region Failure

        [PropertyField(Key = "RetriesAfterFailure")]
        public int CountOfRetriesWhenFailuresAre { get; set; } = 3;

        [PropertyField]
        public int RetryAfterFailureTimeout { get; set; } = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;

        #endregion

        #region Extend Lock

        [PropertyField]
        public int LockDuration { get; set; } = (int) TimeSpan.FromMinutes(5).TotalMilliseconds;

        #endregion
    }
}