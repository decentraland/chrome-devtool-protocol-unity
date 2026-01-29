using System;
using Newtonsoft.Json;
using REnum;

namespace CDPBridges
{
    /// <summary>
    /// T should be "object", not a stringified JSON
    /// </summary>
    [Serializable]
    public struct CDPResponseRaw<T> where T : struct
    {
        public int id;
        public T result;

        public CDPResponseRaw(int id, T result)
        {
            this.id = id;
            this.result = result;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public readonly struct CDPResponse
    {
        public readonly int Id;
        public readonly CDPResult Result;

        public CDPResponse(int id, CDPResult result)
        {
            Id = id;
            Result = result;
        }

        public string ToJson()
        {
            // JsonCovert requires "object" anyway so we can't avoid boxing
            var responseRaw = Result.Match(Id,
                static (id, body) => (object)new CDPResponseRaw<CDPResult.GetResponseBody>(id, body),
                static _ => new CDPResponseRaw<CDPResult.Empty>()
            );

            return JsonConvert.SerializeObject(responseRaw);
        }

        public override string ToString()
        {
            return $"({nameof(CDPResponse)} {{ id: {Id}, method: {Result.ToString()} }})";
        }
    }

    [REnum]
    [REnumFieldEmpty("Network_enable")]
    [REnumField(typeof(GetResponseBody))]
    public partial struct CDPResult
    {
        [Serializable]
        public struct Empty
        {
        }
        
        [Serializable]
        public struct GetResponseBody
        {
            public string body;
            public bool base64Encoded;

            public GetResponseBody(string body, bool base64Encoded)
            {
                this.body = body;
                this.base64Encoded = base64Encoded;
            }
        }
    }
}