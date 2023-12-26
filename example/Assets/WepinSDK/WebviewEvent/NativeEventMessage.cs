using System;
using Newtonsoft.Json;

[Serializable]
public class NativeEventMessage
{
    public class RequestData
    {
        public RequestHeader header { get; set; }
        public RequestBody body { get; set; }
    }

    public class RequestHeader
    {
        public string request_from { get; set; }
        public string request_to { get; set; }
        public long id { get; set; }
    }

    public class RequestBody
    {
        public string command { get; set; }
        public dynamic parameter { get; set; }
    }

    public class SetTokenParameter
    {
        public string token { get; set; }
        public string provider { get; set; }
    }

}

