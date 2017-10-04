using fastJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPSimulator.DataGenerator
{
    class Sensor_01 : IGenerator
    {
        private static List<KeyValuePair<string, List<KeyValuePair<string, List<string>>>>> ProViderCustomerDevice = new List<KeyValuePair<string, List<KeyValuePair<string, List<string>>>>>()
        {
            new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>(
                "LORA/Generic", new List<KeyValuePair<string, List<string>>>() {
                    new KeyValuePair<string, List<string>>("100004550", new List<string>() { "000DB5311385354A", "000DB5311385354B" }),
                    new KeyValuePair<string, List<string>>("100004551", new List<string>() { "000DB5311385354C", "000DB5311385354D", "000DB5311385354E" }),
                    new KeyValuePair<string, List<string>>("100004552", new List<string>() { "000DB5311385354F" })
                }),
            new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>(
                "LORA/Custom", new List<KeyValuePair<string, List<string>>>() {
                    new KeyValuePair<string, List<string>>("100004553", new List<string>() { "000DB5311385354G", "000DB5311385354H", "000DB5311385354I", "000DB5311385354J" })
                })
        };
        private static Int64 fCountUp = 1000;
        private static Int64 fCountDown = 90;
        
        public string Generate()
        {
            var utcNow = DateTime.Now;
            var random = new Random(utcNow.Millisecond);

            var pcd = ProViderCustomerDevice[random.Next(ProViderCustomerDevice.Count)];
            var cd = pcd.Value[random.Next(pcd.Value.Count)];
            
            var jsonObject = new Dictionary<string, object>();

            jsonObject["Time"] = utcNow.ToString(@"yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
            jsonObject["DevEUI"] = cd.Value[random.Next(cd.Value.Count)];
            jsonObject["FPort"] = (1 + random.Next(3)).ToString();
            jsonObject["FCntUp"] = (fCountUp++).ToString();
            jsonObject["ADRbit"] = random.Next(4).ToString();
            jsonObject["MType"] = random.Next(8).ToString();
            jsonObject["FCntDn"] = (random.Next(10) == 0) ? (fCountDown++).ToString() : fCountDown.ToString();
            jsonObject["payload_hex"] = "008264021dc34a0852541f";
            jsonObject["mic_hex"] = "990ea472";

            jsonObject["Lrcid"] = "00000125";
            jsonObject["LrrRSSI"] = "-52.000000";
            jsonObject["LrrSNR"] = "7.000000";
            jsonObject["SpFact"] = "7";
            jsonObject["SubBand"] = "G0";
            jsonObject["Channel"] = "LC1";

            jsonObject["DevLrrCnt"] = "1";
            jsonObject["Lrrid"] = "080600C2";
            jsonObject["Late"] = "0";
            jsonObject["LrrLAT"] = (35 + (random.Next(1000000) / (double)1000000)).ToString();
            jsonObject["LrrLON"] = (139 + (random.Next(1000000) / (double)1000000)).ToString();

            var lrr = new Dictionary<string, object>();

            lrr["Lrrid"] = "080600C2";
            lrr["Chain"] = "0";
            lrr["LrrRSSI"] = "-52.000000";
            lrr["LrrSNR"] = "7.000000";
            lrr["LrrESP"] = "-52.790096";

            jsonObject["Lrrs"] = new Dictionary<string, object>() { { "Lrr", lrr } };

            jsonObject["CustomerID"] = cd.Key;
            jsonObject["CustomerData"] = "{\"alr\":{\"pro\":\"" + pcd.Key + "\",\"ver\":\"1\"}}";
            jsonObject["ModelCfg"] = "0";
            jsonObject["DevAddr"] = "1385354F";

            var payload = new Dictionary<string, object>();

            payload["deviceType"] = 0;
            payload["deviceTypeTxt"] = "Reserved";
            payload["gpsFixStatus"] = 2;
            payload["gpsFixStatusTxt"] = "3D fixed";
            payload["reportType"] = 2;
            payload["reportTypeTxt"] = "Periodic mode report";
            payload["batteryCapacity"] = random.Next(100);

            var lat = random.Next(50000000) / (double)1000000;
            var lon = random.Next(140000000) / (double)1000000;
            
            payload["latitude"] = lat;
            payload["longitude"] = lon;
            payload["latitudeTxt"] = lat.ToString();
            payload["longitudeTxt"] = lon.ToString();
            payload["gpsAvailable"] = random.Next(10) == 0 ? false : true;
            payload["decoderAvailable"] = random.Next(10) == 0 ? false : true;
            jsonObject["payload"] = payload;
            
            return JSON.ToJSON(jsonObject);
        }
    }
}
