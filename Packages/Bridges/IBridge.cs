using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using REnum;

namespace CDPBridges
{
    public interface IBridge : IDisposable
    {
        Action<int, CDPMethod>? OnMethodInvoked { set; }
        
        BridgeStatus Status { get; }

        BridgeStartResult Start();

        UniTask<SendResult> SendEventAsync(CDPEvent cdpEvent, CancellationToken token);
        void SendResponse(int requestId, CDPResult result, CancellationToken ct);
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