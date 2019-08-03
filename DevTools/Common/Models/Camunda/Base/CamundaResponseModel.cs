using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Entities.Camunda;
using VXDesign.Store.DevTools.Common.Entities.HTTP;

namespace VXDesign.Store.DevTools.Common.Models.Camunda.Base
{
    public interface ICamundaResponseModel : IResponse
    {
        Dictionary<string, string> Errors { get; set; }
    }

    public class IntermediateCamundaResponseModel<TModel> : ICamundaResponseModel
    {
        public int Status { get; set; }
        public string Output { get; set; }
        public string Reason { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public TModel Model { get; set; }
    }

    public abstract class CamundaSingleResponseModel<TEntity> : IntermediateCamundaResponseModel<TEntity> where TEntity : ICamundaEntity
    {
    }

    public abstract class CamundaMultipleResponseModel<TEntity> : IntermediateCamundaResponseModel<List<TEntity>> where TEntity : ICamundaEntity
    {
    }
}