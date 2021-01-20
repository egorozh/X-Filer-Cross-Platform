﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChromER.WPF.UI
{
    internal class RectSelectListBox : ListBox
    {
        #region Private Fields

        private RectSelectLogic<ListBoxItem> _selectLogic;

        #endregion

        public static readonly DependencyProperty HasSelectItemsProperty = DependencyProperty.Register(
            "HasSelectItems", typeof(bool), typeof(RectSelectListBox), new PropertyMetadata(default(bool)));

        public bool HasSelectItems
        {
            get => (bool) GetValue(HasSelectItemsProperty);
            set => SetValue(HasSelectItemsProperty, value);
        }

        #region Static Constructor

        static RectSelectListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectSelectListBox),
                new FrameworkPropertyMetadata(typeof(RectSelectListBox)));
        }

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _selectLogic = new RectSelectLogic<ListBoxItem>(this, GetTemplateChild("PART_Canvas") as Canvas,
                i => i.IsSelected = true, i => i.IsSelected = false);
        }

        #endregion

        #region Protected Methods

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            HasSelectItems = SelectedItems.Count > 0;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _selectLogic.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            _selectLogic.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            _selectLogic.OnMouseLeftButtonUp(e);
        }

        #endregion
    }
}