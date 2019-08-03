namespace VXDesign.Store.DevTools.Common.Entities.HTTP
{
    public interface IResponse
    {
        int Status { get; set; }
        string Output { get; set; }
        string Reason { get; set; }
    }
}