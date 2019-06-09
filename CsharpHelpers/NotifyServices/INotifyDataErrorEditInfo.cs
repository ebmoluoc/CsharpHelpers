namespace CsharpHelpers.NotifyServices
{
    public interface INotifyDataErrorEditInfo
    {
        bool HasError { get; }
        string ErrorMessage { get; }
        string PropertyName { get; }
    }
}
