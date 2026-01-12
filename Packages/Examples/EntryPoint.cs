using System.Collections.Generic;
using CDPBridges;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class EntryPoint : MonoBehaviour
{
    [SerializeField] private int currentRequestId;

    private IBridge bridge;

    private void Start()
    {
        bridge = new Bridge(logger: new UnityLogger(nameof(Bridge)));
        BridgeStartResult result = bridge.Start();
        Debug.Log($"Bridge start result: {result}");
    }

    [ContextMenu(nameof(TriggerSend))]
    public void TriggerSend()
    {
        currentRequestId++;

        var cdpEvent = new CDPEvent.Network_requestWillBeSent(
            currentRequestId,
            "some-loader",
            "https://some-fake.com/a-document",
            new Request(
                "http://check-url",
                HttpMethod.GET,
                new Dictionary<string, string>(),
                ReferrerPolicy.Origin(),
                "test payload"
            ),
            MonotonicTime.Now,
            TimeSinceEpoch.Now,
            new Initiator(Initiator.Type.other)
        );

        bridge.SendEventAsync(
            CDPEvent.FromNetwork_requestWillBeSent(cdpEvent),
            destroyCancellationToken
        ).Forget();
    }

    [ContextMenu(nameof(TriggerFinish))]
    public void TriggerFinish()
    {
        var cdpEvent = new CDPEvent.Network_responseReceived(
            currentRequestId,
            "some-loader",
            MonotonicTime.Now,
            ResourceType.Fetch,
            new NetworkResponse(
                url: "http://check-url",
                status: 200,
                statusText: "OK",
                headers: new Dictionary<string, string>(),
                mimeType: "application/json",
                charset: "utf-8",
                requestHeaders: new Dictionary<string, string>(),
                connectionReused: false,
                connectionId: 1,
                encodedDataLength: 0,
                responseTime: TimeSinceEpoch.Now,
                cacheStorageCacheName: "",
                protocol: "http/1.1",
                securityState: SecurityState.Neutral()
            )
        );

        bridge.SendEventAsync(
            CDPEvent.FromNetwork_responseReceived(cdpEvent),
            destroyCancellationToken
        ).Forget();
    }

    private void OnDestroy()
    {
        bridge.Dispose();
    }
}