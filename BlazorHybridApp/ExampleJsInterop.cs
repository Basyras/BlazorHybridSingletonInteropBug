using Microsoft.JSInterop;

namespace BlazorHybridApp;

public class ExampleJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./exampleJsInterop.js").AsTask());

    public async ValueTask<string> Prompt(string message)
    {
        IJSObjectReference module = await moduleTask.Value.ConfigureAwait(false);
        return await module.InvokeAsync<string>("showPrompt", message).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            IJSObjectReference module = await moduleTask.Value.ConfigureAwait(false);
            await module.DisposeAsync().ConfigureAwait(false);
        }

        GC.SuppressFinalize(this);
    }
}