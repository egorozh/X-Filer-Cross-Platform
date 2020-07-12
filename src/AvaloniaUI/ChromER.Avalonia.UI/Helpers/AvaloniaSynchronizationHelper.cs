using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using ChromER.Shared.ViewModels;

namespace ChromER.Avalonia.UI
{
    internal class AvaloniaSynchronizationHelper : ISynchronizationHelper
    {
        public async Task InvokeAsync(Action action)
        {
            await Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Background);
        }
    }
}