using REnum;

namespace CDPBridges
{
    [REnum]
    [REnumField(typeof(Text))]
    [REnumField(typeof(Binary))]
    public partial struct WebSocketMessage
    {
        public readonly struct Text
        {
            public readonly string Message;

            public Text(string message)
            {
                Message = message;
            }

            public override string ToString()
            {
                return $"({nameof(Text)} {{ message: {Message} }} )";
            }
        }


        public readonly struct Binary
        {
            public readonly byte[] Data;

            public Binary(byte[] data)
            {
                Data = data;
            }

            public override string ToString()
            {
                return $"({nameof(Binary)} {{ dataLength: {Data.Length} }} )";
            }
        }
    }
}