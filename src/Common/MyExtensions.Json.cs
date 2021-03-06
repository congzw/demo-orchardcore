﻿using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common
{
    public static partial class MyExtensions
    {
        class IPAddressConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(IPAddress));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                IPAddress ip = (IPAddress)value;
                writer.WriteValue(ip.ToString());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);
                return IPAddress.Parse(token.Value<string>());
            }
        }
        class IPEndPointConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(IPEndPoint));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                IPEndPoint ep = (IPEndPoint)value;
                writer.WriteStartObject();
                writer.WritePropertyName("Address");
                serializer.Serialize(writer, ep.Address);
                writer.WritePropertyName("Port");
                writer.WriteValue(ep.Port);
                writer.WriteEndObject();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jo = JObject.Load(reader);
                IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
                int port = jo["Port"].Value<int>();
                return new IPEndPoint(address, port);
            }
        }

        /// <summary>
        /// object as json
        /// </summary>
        /// <param name="model"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJson(this object model, bool indented = false)
        {
            if (model is string)
            {
                return model.ToString();
            }
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IPAddressConverter());
            settings.Converters.Add(new IPEndPointConverter());
            settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(model, settings);
        }
    }
}
