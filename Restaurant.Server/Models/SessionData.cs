using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Restaurant.Core.Data;

namespace Restaurant.Server.Models
{
    [Serializable]
    [DataContract]
    public struct SessionData
    {
        [DataMember]
        public List<Food> Cart;

        public static SessionData Get(ISession session)
        {
            string s = session.GetString("session");
            SessionData sd;
            if (session == null)
            {
                //New session
                sd = new SessionData();
                sd.Cart = new List<Food>();
            }
            else
            {
                //Load session
                sd = (SessionData)JsonConvert.DeserializeObject(s, typeof(SessionData));
            }
            return sd;
        }
    }
}
