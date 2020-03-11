using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public class CamundaTopic
    {
        public string TopicName { get; set; }
        public int LockDuration { get; set; }
        public IEnumerable<string> Variables { get; set; }
        public string BusinessKey { get; set; }
    }

    public interface IReadOnlyCamundaTopics : IReadOnlyList<CamundaTopic>
    {
    }

    public class CamundaTopics : List<CamundaTopic>, IReadOnlyCamundaTopics
    {
    }
}