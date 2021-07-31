using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace ChromER.SDK
{
    [PropertyChanged.DoNotNotify]
    public class ChromerWindowBase : Window, IStyleable
    {   
        #region Private Fields

        private const string PartTitleBar = "PART_TitleBar";
        private Grid? _titleBar;

        #endregion

        #region IStyleable

        Type IStyleable.StyleKey => typeof(ChromerWindowBase);

        #endregion

        #region Dependency Properties

        public static readonly StyledProperty<object> ToolBarContentProperty =
            AvaloniaProperty.Register<ChromerWindowBase, object>(nameof(ToolBarContent));

        public static readonly StyledProperty<ICommand> CloseCommandProperty = AvaloniaProperty.Register
            <ChromerWindowBase, ICommand>(nameof(CloseCommand));

        public static readonly StyledProperty<ICommand> CollapseCommandProperty = AvaloniaProperty.Register
            <ChromerWindowBase, ICommand>(nameof(CollapseCommand));

        public static readonly StyledProperty<ICommand> ExpandCommandProperty = AvaloniaProperty.Register
            <ChromerWindowBase, ICommand>(nameof(ExpandCommand));

        #endregion

        #region Public Properties

        public object ToolBarContent
        {
            get => GetValue(ToolBarContentProperty);
            set => SetValue(ToolBarContentProperty, value);
        }

        public ICommand CloseCommand
        {
            get => GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public ICommand CollapseCommand
        {
            get => GetValue(CollapseCommandProperty);
            set => SetValue(CollapseCommandProperty, value);
        }

        public ICommand ExpandCommand
        {
            get => GetValue(ExpandCommandProperty);
            set => SetValue(ExpandCommandProperty, value);
        }

        #endregion

        #region Constructor

        public ChromerWindowBase()
        {
            CloseCommand = new DelegateCommand(OnClose);
            CollapseCommand = new DelegateCommand(OnCollapse);
            ExpandCommand = new DelegateCommand(OnExpand);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;
        }

        #endregion

        #region Protected Methods

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _titleBar = e.NameScope.Get<Grid>(PartTitleBar);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (Equals(e.Source, _titleBar))
            {
                BeginMoveDrag(e);
            }
        }

        #endregion

        #region Private Methods

        private void OnClose()
        {
            Close();
        }

        private void OnCollapse()
        {
            WindowState = WindowState.Minimized;
        }

        private void OnExpand()
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        #endregion
    }
}