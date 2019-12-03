#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using ThemesLib.InStat.Helpers;

#endregion

namespace ThemesLib.InStat.Controls
{
    public class ColorComboBox : Control
    {
        #region Static

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(nameof(Selected), typeof(int), typeof(ColorComboBox),
                new FrameworkPropertyMetadata(propertyChangedCallback: SelectedPropertyChangedCallback));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Brush), typeof(ColorComboBox),
                new FrameworkPropertyMetadata(propertyChangedCallback: SelectedColorPropertyChangedCallback));

        public static readonly DependencyProperty ToggleButtonUpColorProperty =
            DependencyProperty.Register(nameof(ToggleButtonUpColor), typeof(Brush), typeof(ColorComboBox),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static readonly DependencyProperty ToggleButtonDownColorProperty =
            DependencyProperty.Register(nameof(ToggleButtonDownColor), typeof(Brush), typeof(ColorComboBox),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public static readonly DependencyProperty BackgroundPopupColorProperty =
            DependencyProperty.Register(nameof(BackgroundPopupColor), typeof(Brush), typeof(ColorComboBox),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region Private fields

        private readonly List<Button> _buttons = new List<Button>();

        private readonly List<StackPanel> _partComboBoxItemStackPanelBaseList = new List<StackPanel>();

        private readonly ISymColors _symColors;

        private ComboBox _comboBox;

        private Ellipse _ellipse;

        private int _newIdx = 0;

        private int _oldIdx = 0;

        private ToggleButton _toggleButton;

        #endregion

        #region Construct

        static ColorComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorComboBox), new FrameworkPropertyMetadata(typeof(ColorComboBox)));
        }

        public ColorComboBox()
        {
            _symColors = new SymColors();

            Loaded += OnLoaded;
        }

        #endregion

        #region Public

        public override void OnApplyTemplate()
        {
            GetParts();

            _comboBox.ItemContainerGenerator.StatusChanged += ItemContainerGeneratorFirstInitOnStatusChanged;
            _comboBox.ItemsSource = new ObservableCollection<string> {"0", "1", "2", "3"};


            Redraw();
        }

        public int Selected
        {
            get => (int) GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public Brush SelectedColor
        {
            get => (Brush) GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public Brush ToggleButtonUpColor
        {
            get => (Brush) GetValue(ToggleButtonUpColorProperty);
            set => SetValue(ToggleButtonUpColorProperty, value);
        }

        public Brush ToggleButtonDownColor
        {
            get => (Brush) GetValue(ToggleButtonDownColorProperty);
            set => SetValue(ToggleButtonDownColorProperty, value);
        }

        public Brush BackgroundPopupColor
        {
            get => (Brush)GetValue(BackgroundPopupColorProperty);
            set => SetValue(BackgroundPopupColorProperty, value);
        }

        #endregion

        #region Private methods

        private static void SelectedColorPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorComboBox colorComboBox && e.NewValue is Brush newBrush)
            {
                for (var colorIdx = 0; colorIdx < colorComboBox._symColors.Count; colorIdx++)
                {
                    var solidColor = (SolidColorBrush) colorComboBox._symColors[colorIdx];
                    if (solidColor.Color == ((SolidColorBrush) newBrush).Color && colorComboBox._newIdx != colorIdx)
                    {
                        colorComboBox._newIdx = colorIdx;
                        colorComboBox.Redraw();
                        return;
                    }
                }
            }
        }

        private static void SelectedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorComboBox colorComboBox && e.NewValue is int newIdx && colorComboBox._newIdx != newIdx)
            {
                colorComboBox._newIdx = newIdx;
                colorComboBox.Redraw();
            }
        }

        private static void SelectButton(Button button)
        {
            button.ApplyTemplate();
            var grid = (Grid) button.Template.FindName("PART_GridMain", button);
            VisualStateManager.GoToElementState(grid, "Selected", false);
        }

        private static void UnSelectButton(Button button)
        {
            button.ApplyTemplate();
            var grid = (Grid) button.Template.FindName("PART_GridMain", button);
            VisualStateManager.GoToElementState(grid, "UnSelected", false);
        }

        private void Redraw()
        {
            if (_comboBox == null || _ellipse == null || _toggleButton == null) return;

            if (Selected != this._newIdx)
            {
                Selected = this._newIdx;
            }

            _ellipse.Fill = _symColors[_newIdx];
            ChangeSelectedColorButtons(_newIdx, _oldIdx);
            _oldIdx = _newIdx;
        }

        private void ChangeSelectedColorButtons(int newIdx, int oldIdx)
        {
            if (newIdx == oldIdx) return;
            if (_buttons == null || _buttons.Count == 0) return;
            if (_symColors == null || _symColors.Count == 0) return;

            if (_buttons.Count > newIdx)
            {
                Button button = _buttons[newIdx];
                SelectButton(button);
            }

            if (oldIdx >= 0 && _symColors.Count > oldIdx && _buttons.Count > oldIdx)
            {
                Button button = _buttons[oldIdx];
                UnSelectButton(button);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style) FindResource(new ComponentResourceKey(typeof(ColorComboBox), "KControlColorComboBox"));
        }

        private void GetParts()
        {
            _comboBox = this.FindNameEx<ComboBox>("PART_ComboBox");
            _comboBox.ApplyTemplate();

            _ellipse = _comboBox.FindNameEx<Ellipse>("PART_EllipseContent");
            _toggleButton = _comboBox.FindNameEx<ToggleButton>("PART_ToggleButton");
        }

        private void ItemContainerGeneratorFirstInitOnStatusChanged(object sender, EventArgs e)
        {
            if (sender is ItemContainerGenerator containerGenerator &&
                containerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                _comboBox.ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorFirstInitOnStatusChanged;

                foreach (var item in _comboBox.ItemContainerGenerator.Items)
                {
                    var comboBoxItem = (ComboBoxItem) _comboBox.ItemContainerGenerator.ContainerFromItem(item);
                    comboBoxItem.ApplyTemplate();

                    var stackPanel = comboBoxItem.FindNameEx<StackPanel>("PART_ComboBoxItemStackPanelBase");

                    _partComboBoxItemStackPanelBaseList.Add(stackPanel);
                }
            }

            if (_partComboBoxItemStackPanelBaseList.Count <= 0) return;

//            var buttonNotHaveColorStyle = (Style) FindResource("KNotHaveColorButtonStyle");
            var buttonColorStyle = (Style) FindResource("KColorButtonStyle");

            const int buttonCount = 4;

            for (int s = 0; s < _partComboBoxItemStackPanelBaseList.Count; s++)
            {
                for (var k = 0; k < buttonCount; k++)
                {
                    int sti = s * buttonCount + k;

                    if (sti >= _symColors.Count)
                        return;

                    var button = new Button
                    {
                        Foreground = _symColors[sti],
//                        Style = sti == 0 ? buttonNotHaveColorStyle : buttonColorStyle,
                        Style = buttonColorStyle,
                        Margin = new Thickness(2),
                        Tag = new ColorComboButtonTag(sti, s, this)
                    };

                    button.Click += ButtonOnClick;

                    _buttons.Add(button);

                    if (sti == Selected)
                    {
                        SelectButton(button);
                    }

                    _partComboBoxItemStackPanelBaseList[s].Children.Add(button);
                }
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ColorComboButtonTag tag && tag.Parent is ColorComboBox colorComboBox)
            {
                Selected = tag.IdxButton;
                SelectedColor = _symColors[tag.IdxButton];
                colorComboBox._toggleButton.IsChecked = false;
            }
        }

        #endregion
    }
}