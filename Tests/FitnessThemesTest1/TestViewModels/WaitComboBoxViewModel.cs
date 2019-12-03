using System.Collections.Generic;
using FitnessThemesTest1.Providers;
using ThemesLib.InStat.Helpers;
using ThemesLib.InStat.Helpers.VirtualizingCollection;

namespace FitnessThemesTest1.TestViewModels
{
    public class WaitComboBoxViewModel : ViewModelBase
    {
        public WaitComboBoxViewModel()
        {
            IVirtualListProvider<Customer> customerProvider = new DataProvider2();
            _autoComboSource = new AsyncVirtualizingCollection<Customer>(customerProvider, 100, 300 * 1000);

            // SelectedIndexForWaitComboBox = 1287;
            // SelectedIndexForWaitComboBox = -1;

//            SelectedItemString = "sergey.babich@instatfootball.com";


        }

        public string SelectedItemString
        {
            get => _selectedItemString;
            set => SetProperty(ref _selectedItemString, value);
        }

        private WaitCombo2DataProvider _waitCombo2DataProvider;

        public WaitCombo2DataProvider GetWaitCombo2DataProvider()
        {
            return _waitCombo2DataProvider;
        }

        public AsyncVirtualizingCollection<Customer> _autoComboSource;
        private List<string> _listComboBox;
        private int _selectedIndexForWaitComboBox;
        private int _selectedIndexForWaitComboBox2;
        private Customer _selectedItemForWaitComboBox;
        private string _selectedItemString;

        public AsyncVirtualizingCollection<Customer> AutoComboSource
        {
            get => _autoComboSource;
        }

        public List<string> ListComboBox
        {
            get => _listComboBox;
        }

        public int SelectedIndexForWaitComboBox
        {
            get { return _selectedIndexForWaitComboBox; }
            set
            {
                if (!Equals(_selectedIndexForWaitComboBox, value))
                {
                    _selectedIndexForWaitComboBox = value;
                    OnPropertyChanged(nameof(SelectedIndexForWaitComboBox));
                }
            }
        }

        public Customer SelectedItemForWaitComboBox
        {
            get { return _selectedItemForWaitComboBox; }
            set
            {
                if (!Equals(_selectedItemForWaitComboBox, value))
                {
                    _selectedItemForWaitComboBox = value;
                    OnPropertyChanged(nameof(SelectedItemForWaitComboBox));
                }
            }
        }
    }
}