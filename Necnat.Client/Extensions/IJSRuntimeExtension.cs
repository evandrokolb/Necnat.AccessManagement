using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Necnat.Client.Extensions
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask InitializerInactivityTimer<T>(this IJSRuntime js,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("initializerInactivityTimer", dotNetObjectReference);
        }
    }
}
