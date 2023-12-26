using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WepinSDK.Types;

[Serializable]
public class WebviewRequest
{
    public Header header;

    public Body body;

    [Serializable]
    public class Header
    {
        public string request_from;
        public string request_to;
        public string id;
    }

    [Serializable]
    public class Body
    {
        public string command;
        public object parameter;
    }
}



[Serializable]
public class Account
{
    public string network;
    public string address;
}

[Serializable]
public class AccountList
{
    public List<Account> accounts;
}

public class WebviewResponse
{
    public ResponseHeader header { get; set; }
    public ResponseBody body { get; set; }

    public WebviewResponse(ResponseHeader header, ResponseBody body)
    {
        this.header = header;
        this.body = body;
    }
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class ResponseHeader
{
    public string id { get; set; }
    public string response_from { get; set; }
    public string response_to { get; set; }

    public ResponseHeader(string id, string response_from, string response_to)
    {
        this.id = id;
        this.response_from = response_from;
        this.response_to = response_to;
    }
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class ResponseBody
{
    public string command { get; set; }
    public string state { get; set; }
    public dynamic data { get; set; }

    public ResponseBody(string command, string state, dynamic data)
    {
        this.command = command;
        this.state = state;
        this.data = data;
    }
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
//[Serializable]
public class ResponseReadyToWidget
{
    public string appKey { get; set; }
    public Attributes attributes { get; set; }
    public string domain { get; set; }
    public int platform { get; set; }
    public string version { get; set; }

    public ResponseReadyToWidget(string appKey, Attributes attributes, string domain, int platform, string version)
    {
        this.appKey = appKey;
        this.attributes = attributes;
        this.domain = domain;
        this.platform = platform;
        this.version = version;
    }
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}