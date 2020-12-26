using System.Collections;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Egorozh.GoogleChromeWindow.WPF
{
    public class GoogleChromeWindow : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty TabsMaxWidthProperty = DependencyProperty.Register(
            nameof(TabsMaxWidth), typeof(double), typeof(GoogleChromeWindow), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(
            nameof(CloseCommand), typeof(ICommand), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CollapseCommandProperty = DependencyProperty.Register(
            nameof(CollapseCommand), typeof(ICommand), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ExpandCommandProperty = DependencyProperty.Register(
            nameof(ExpandCommand), typeof(ICommand), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ToolBarContentProperty = DependencyProperty.Register(
            nameof(ToolBarContent), typeof(FrameworkElement), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(FrameworkElement)));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(GoogleChromeWindow), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TabItemTemplateProperty = DependencyProperty.Register(
            nameof(TabItemTemplate), typeof(DataTemplate), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ActiveTabBackgroundProperty = DependencyProperty.Register(
            nameof(ActiveTabBackground), typeof(Brush), typeof(GoogleChromeWindow),
            new PropertyMetadata(default(Brush)));

        #endregion

        #region Public Properties

        public static double SystemButtonsWidth { get; } = 44 * 3;

        public double TabsMaxWidth
        {
            get => (double) GetValue(TabsMaxWidthProperty);
            set => SetValue(TabsMaxWidthProperty, value);
        }

        public ICommand CloseCommand
        {
            get => (ICommand) GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public ICommand CollapseCommand
        {
            get => (ICommand) GetValue(CollapseCommandProperty);
            set => SetValue(CollapseCommandProperty, value);
        }

        public ICommand ExpandCommand
        {
            get => (ICommand) GetValue(ExpandCommandProperty);
            set => SetValue(ExpandCommandProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public FrameworkElement ToolBarContent
        {
            get => (FrameworkElement) GetValue(ToolBarContentProperty);
            set => SetValue(ToolBarContentProperty, value);
        }

        public object SelectedItem
        {
            get => (object) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public DataTemplate TabItemTemplate
        {
            get => (DataTemplate) GetValue(TabItemTemplateProperty);
            set => SetValue(TabItemTemplateProperty, value);
        }

        public Brush ActiveTabBackground
        {
            get => (Brush) GetValue(ActiveTabBackgroundProperty);
            set => SetValue(ActiveTabBackgroundProperty, value);
        }

        #endregion

        #region Static Constructor

        static GoogleChromeWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GoogleChromeWindow),
                new FrameworkPropertyMetadata(typeof(GoogleChromeWindow)));
        }

        #endregion

        #region Constructor

        public GoogleChromeWindow()
        {
            CloseCommand = new DelegateCommand(OnClose);
            CollapseCommand = new DelegateCommand(OnCollapse);
            ExpandCommand = new DelegateCommand(OnExpand);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var behavior = new WindowResizeFixerBehavior();
            behavior.Attach(this);

            //var blurBehavior = new BlurBehavior();
            //blurBehavior.Attach(this);

            CalcTabMaxWidth();

            SizeChanged += (sender, args) => CalcTabMaxWidth();
        }

        #endregion

        #region Private Methods

        private void OnClose(object obj)
        {
            Close();
        }

        private void OnCollapse(object obj)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnExpand(object obj)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        private void CalcTabMaxWidth()
        {
            TabsMaxWidth = ActualWidth - 24 * 3 - 115 - 34;
        }

        #endregion
    }
}