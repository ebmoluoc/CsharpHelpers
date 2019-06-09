using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CsharpHelpers.NotifyServices
{

    [Serializable]
    public abstract class NotifyModel : INotifyPropertyChanged, INotifyDataErrorInfo, INotifyDataErrorEdit
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? ""));
        }

        protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            property = value;
            RaisePropertyChanged(propertyName);
        }

        #endregion INotifyPropertyChanged


        #region INotifyDataErrorInfo

        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
                propertyName = "";

            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName ?? ""));
        }

        public void SetError(INotifyDataErrorEditInfo errorEditInfo)
        {
            var errorMessage = errorEditInfo.ErrorMessage;
            var propertyName = errorEditInfo.PropertyName ?? "";

            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Cannot be null, empty or only white-space characters.", nameof(errorEditInfo.ErrorMessage));

            if (errorEditInfo.HasError)
            {
                if (!_errors.ContainsKey(propertyName))
                    _errors[propertyName] = new List<string>();

                if (!_errors[propertyName].Contains(errorMessage))
                {
                    _errors[propertyName].Add(errorMessage);
                    RaiseErrorsChanged(propertyName);
                }
            }
            else
            {
                if (_errors.ContainsKey(propertyName) && _errors[propertyName].Contains(errorMessage))
                {
                    _errors[propertyName].Remove(errorMessage);

                    if (_errors[propertyName].Count == 0)
                        _errors.Remove(propertyName);

                    RaiseErrorsChanged(propertyName);
                }
            }
        }

        #endregion INotifyDataErrorInfo

    }

}
