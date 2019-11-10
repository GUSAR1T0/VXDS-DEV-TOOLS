namespace VXDesign.Store.DevTools.Core.Entities.HTTP
{
    public interface IResponse
    {
        int Status { get; set; }
        string Output { get; set; }
        string Reason { get; set; }
    }
}