using System.Collections.Generic;
using Newtonsoft.Json;

namespace WepinSDK.Types{

    public class Attributes {
        public string defaultLanguage { get; set; }
        public string defaultCurrency { get; set; }    
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class AccountList
    {
        public List<Accounts> accounts { get; set; }

        public static AccountList FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AccountList>(json);
        }
    }
    
    public class Accounts {

            public string network { get; set; }
            public string address { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

