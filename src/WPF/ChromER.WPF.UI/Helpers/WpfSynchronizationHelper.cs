using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChromER.WPF.UI
{
    internal class WpfSynchronizationHelper : ISynchronizationHelper
    {
        public async Task InvokeAsync(Action action)
            => await Application.Current.Dispatcher.InvokeAsync(action, DispatcherPriority.Background);
    }
}