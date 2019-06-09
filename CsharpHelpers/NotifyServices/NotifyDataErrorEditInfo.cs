namespace CsharpHelpers.NotifyServices
{
    public abstract class NotifyDataErrorEditInfo : INotifyDataErrorEditInfo
    {
        public bool HasError { get; protected set; }
        public string ErrorMessage { get; protected set; }
        public string PropertyName { get; protected set; }
    }
}
