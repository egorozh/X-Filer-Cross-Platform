using System;
using System.Threading.Tasks;

namespace ChromER
{
    public interface ISynchronizationHelper
    {
        Task InvokeAsync(Action action);
    }
}