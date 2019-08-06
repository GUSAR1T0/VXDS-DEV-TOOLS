using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Attributes.Camunda;
using VXDesign.Store.DevTools.Common.Entities.Enums;
using VXDesign.Store.DevTools.Common.Extensions.Enums;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda
{
    public class CamundaEndpoint
    {
        public int ActionCode { get; }
        public string CategoryName { get; }
        public string ActionName { get; }
        public HttpMethod Method { get; }
        public string Path { get; }

        private CamundaEndpoint(CamundaAction camundaAction, CamundaCategoryAttribute camundaCategoryAttribute, CamundaActionAttribute camundaActionAttribute)
        {
            ActionCode = (int) camundaAction;
            CategoryName = camundaCategoryAttribute.Name;
            ActionName = camundaActionAttribute.Name;
            Method = camundaActionAttribute.Method;
            Path = $"/{camundaCategoryAttribute.Root}{(!string.IsNullOrWhiteSpace(camundaActionAttribute.Path) ? '/' + camundaActionAttribute.Path : "")}";
        }

        private static IEnumerable<CamundaEndpoint> GetEndpoints(Func<CamundaAction, bool> predicate = null)
        {
            return
                from category in EnumExtensions.GetValues<CamundaCategory>()
                let allActions = EnumExtensions.GetValues<CamundaAction>().Where(predicate ?? (action => true))
                let allActionsAndAttributes = allActions.Select(action =>
                (
                    action,
                    attributes: action.GetAttributesOfType<CamundaActionAttribute>()
                ))
                from actionAndAttributes in allActionsAndAttributes
                from actionAttribute in actionAndAttributes.attributes
                where actionAttribute.CamundaCategory == category
                let categoryAttribute = actionAttribute.CamundaCategory.GetAttributeOfType<CamundaCategoryAttribute>()
                select new CamundaEndpoint(actionAndAttributes.action, categoryAttribute, actionAttribute);
        }

        public static IEnumerable<CamundaEndpoint> GetAll() => GetEndpoints().ToList();

        public static CamundaEndpoint GetEndpoint(CamundaAction camundaAction) => GetEndpoints(action => action == camundaAction).FirstOrDefault();
    }
}