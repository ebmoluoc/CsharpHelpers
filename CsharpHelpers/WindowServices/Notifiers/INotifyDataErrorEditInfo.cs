namespace CsharpHelpers.WindowServices
{
    public interface INotifyDataErrorEditInfo
    {
        bool HasError { get; }
        string ErrorMessage { get; }
        string PropertyName { get; }
    }
}
