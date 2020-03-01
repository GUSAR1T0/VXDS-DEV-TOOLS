namespace VXDesign.Store.DevTools.Common.Core.HTTP
{
    public interface IResponse
    {
        int Status { get; set; }
        string Output { get; set; }
        string Reason { get; set; }
    }
}