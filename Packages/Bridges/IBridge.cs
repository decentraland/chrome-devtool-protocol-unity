using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using REnum;

namespace CDPBridges
{
    public interface IBridge : IDisposable
    {
        /// <summary>
        /// Handler for CDP Method according to https://chromedevtools.github.io/devtools-protocol/1-3/Network/ "Methods": <br/>
        /// It's allowed to return to `null` if the given Method is not implemented by the client. <br/>
        /// <see cref="HandleMethod"/> is called from a background thread
        /// </summary>
        Func<int, CDPMethod, CDPResult?>? HandleMethod { set; }
        
        BridgeStatus Status { get; }

        BridgeStartResult Start();

        UniTask<SendResult> SendEventAsync(CDPEvent cdpEvent, CancellationToken token);
    }

    public enum BridgeStatus
    {
        Offline,
        Online,
        HasListeners
    }

    [REnum]
    [REnumFieldEmpty("Success")]
    [REnumField(typeof(BridgeStartError))]
    public partial struct BridgeStartResult
    {
    }


    [REnum]
    [REnumField(typeof(WebSocketError))]
    [REnumField(typeof(BrowserOpenError))]
    public partial struct BridgeStartError
    {
    }

    public readonly struct WebSocketError
    {
        public readonly Exception Exception;

        public WebSocketError(Exception exception)
        {
            Exception = exception;
        }
    }
}