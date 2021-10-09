using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Dock.Avalonia.Controls;
using Dock.Serializer;

namespace ChromER
{
    public class MainWindow : Window
    {
        private DockControl _dockControl;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _dockControl = this.FindControl<DockControl>("DockControl");

            this.Closed += OnClosed;
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            var json = new DockSerializer(typeof(List<>)).Serialize(_dockControl.Layout);

            File.WriteAllText("layout.json", json);
        }
    }
}