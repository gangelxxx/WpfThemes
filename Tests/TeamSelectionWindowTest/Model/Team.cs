using ControlsLibrary.VirtualizingCollection;

namespace TeamSelectionWindowTest.Model
{
    public class Team : IVirtualItem
    {
        private string _upperString;
        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                if (!string.IsNullOrEmpty(value))    
                {
                    _name = value;
                    _upperString = value.ToUpper();
                }
            }
        }

        public Team(string name)
        {
            Name = name;
        }


        public override string ToString()
        {
            return _name;
        }

        public string UpperString => _upperString;
    }
}