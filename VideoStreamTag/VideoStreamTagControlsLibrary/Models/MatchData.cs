using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoStreamTagControlsLibrary.Properties;

namespace VideoStreamTagControlsLibrary.Models
{
    public class MatchData : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private bool _isSelected;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                OnUpdateSelected();
            }
        }

        public object Tag { get; set; }

        public event EventHandler UpdateSelected;

        public MatchData(int id, string name, object tag)
        {
            Id = id;
            Name = name;
            Tag = tag;
            IsSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnUpdateSelected()
        {
            UpdateSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}