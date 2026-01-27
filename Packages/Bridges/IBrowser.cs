using System;
using REnum;

namespace CDPBridges
{
    public interface IBrowser
    {
        BrowserOpenResult OpenUrl(string url);
    }


    [REnum]
    [REnumFieldEmpty("Success")]
    [REnumField(typeof(BrowserOpenError))]
    public partial struct BrowserOpenResult
    {
    }


    [REnum]
    [REnumFieldEmpty("ErrorChromeNotInstalled")]
    [REnumField(typeof(Exception))]
    public partial struct BrowserOpenError
    {
    }
}