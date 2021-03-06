using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public interface ICamundaResponse : IResponse
    {
        object Errors { get; set; }
    }

    public class IntermediateCamundaResponse<TObject> : ICamundaResponse
    {
        public int Status { get; set; }
        public string Output { get; set; }
        public string Reason { get; set; }
        public object Errors { get; set; }
        public TObject Response { get; set; }
    }

    public abstract class CamundaEmptyResponse : IntermediateCamundaResponse<EmptyResult>
    {
    }

    public abstract class CamundaSingleResponse<TEntity> : IntermediateCamundaResponse<TEntity> where TEntity : ICamundaEntity
    {
    }

    public abstract class CamundaMultipleResponse<TEntity> : IntermediateCamundaResponse<List<TEntity>> where TEntity : ICamundaEntity
    {
    }
}