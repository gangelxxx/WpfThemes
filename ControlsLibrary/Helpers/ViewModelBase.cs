using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ControlsLibrary.Helpers
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Методы

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null, Action updated = null,
            Func<bool> isUpdate = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);

            if (isUpdate != null && isUpdate.Invoke())
            {
                updated?.Invoke();
            }

            return true;
        }

        #endregion
    }
}