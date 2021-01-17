using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using TimeZoneConverter;

namespace GBMProyect
{
  
    public class GBM
    {
        private string User;
        private string Password;
        private string ClientID;
        private string ContractID;
        private string BearerKey = "eyJraWQiOiJSUXRsSzlNbUtJT1JTNjA4alVpazBPZktNeHlEXC81UEUzenp6SGNqNVlJYz0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJjODIxYmI4MS0xZGMwLTQzYzYtOWI0ZC1mYzYwNDllOTJkNzgiLCJldmVudF9pZCI6IjQ2ODVhZjU4LWFkY2QtNDQ1MS04MDExLTU2MGVlYjZkOTRjNiIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE2MDk1NDIyMDUsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX0JLdTdxQW9odSIsImV4cCI6MTYwOTU0NTgwNSwiaWF0IjoxNjA5NTQyMjA1LCJqdGkiOiIyNjIxYWE1Mi0xOGUxLTQzYjEtOTU1Ny1lNTk2NDFkZDY3MmUiLCJjbGllbnRfaWQiOiI3cmszZ3FiaDRsb2NkY2dibXZkanEwaGRlNSIsInVzZXJuYW1lIjoicmF1bC5zb3NhLmNvcnRlc3xnbWFpbC5jb20ifQ.cA1gd29iAJoy10CxQcIhQSnhu3Kv-AU_ZqPgXQZrEgrm23a0czHk3-x5fCQhM85Zh8oseb8cmcUnb0WQCNgzt5Wa5I0eUsboLlMh5ULY8APG4y3eRUELjA0uAtcHiGLyyfdpnQddykBO86wheYb5J3_W33CBtZkDqNarmE_UwVL6JLxYbJsXU4S6mbw_VG2-akSTQLrOEo1HwffzGnvrGTuVYMFDyFSz1jTxhOZR7wkdm6AMhC85O8qAfjN701fFEckTBuMBoii_hKFbDTpBWI8OrX-XiraHRRzk4UQwPt2E_AyTSFqWFeDObyJ2gDztO1maCErl0w9Yztpb5cV7jw";
        public enum OrderTypes { Sell = 8, Buy = 1 };
        public enum InstrumentTypes { SIC = 0, IPC = 2 };
        public enum OrderState { Done = 7, Canceled = 5, Rejected = 9, Pending = 2 };
        public GBM(string UserMail, string password, string clientID, string contractID)
        {
            User = UserMail;
            Password = password;
            ClientID = clientID;
            ContractID = contractID;
        }
        public void Autenthicate()
        {
            const string WEBSERVICE_URL = "https://auth.gbm.com/api/v1/session/user";
            try
            {
                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";

                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "identity");
                myHttpWebRequest.Headers.Add("Device-Type", "Mi A2 Lite");
                myHttpWebRequest.Headers.Add("Os-version", "Android 29");
                myHttpWebRequest.Headers.Add("App-version", "60");
                myHttpWebRequest.Headers.Add("Platform", "android");


                string postData = "{\"clientid\":\"" + this.ClientID + "\",\"user\":\"" + this.User + "\",\"password\":\"" + this.Password + "\"}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
               

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                   
                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<LoginResponse>(Html);
                   
                    BearerKey = JsonData.accessToken;

                    responseStream.Close();
                }

                WebResponse.Close();

            }
            catch (System.Net.WebException UnautorizedException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Movers GetNationalMarketMovers()
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Market/GetLowRiseIssuesByBenchmark";
            try
            {

             
                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = 13;
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " +this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                string postData = "{\"request\":1}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);

                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
              
                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<List<Ticker>>(Html);
                    var Losers = JsonData.GetRange(0, 10);
                    var Winers = JsonData.GetRange(10, 10);
                    var Movers_ = JsonData.GetRange(20, 10);
                    var MoverString = Movers_.Select(item => item.issueId).ToList();
                    //dynamic data = JObject.Parse(Html);
                    responseStream.Close();
                    var Moc = new Movers();
                    Moc.Lossers = Losers;
                    Moc.Winers = Winers;
                    Moc.Volume = Movers_;


                    return Moc;
                }

                WebResponse.Close();
            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        public class Movers
        {
            public List<Ticker> Winers;
            public List<Ticker> Lossers;
            public List<Ticker> Volume;
        }
        public Movers GetGlobalMarketMovers()
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Market/GetLowRiseIssuesByMarket";
            try
            {
             
                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                string postData = "{\"instrumentType\":2,\"isOnLine\":true}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<List<Ticker>>(Html);
                    var Losers = JsonData.GetRange(0, 10);
                    var Winers = JsonData.GetRange(10, 10);
                    var Movers_ = JsonData.GetRange(20, 10);
                    var MoverString = Movers_.Select(item => item.issueId).ToList();
                    var Moc = new Movers();
                    Moc.Lossers = Losers;
                    Moc.Winers = Winers;
                    Moc.Volume = Movers_;
                    responseStream.Close();
                    return Moc;
                }

                WebResponse.Close();


            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        //ToDo: Update to return historical data of ticker
        private void GetTickerPrice()
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Market/GetIssueMarketDetail/RNO%20N";
            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.Method = "GET";
                // myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<List<Ticker>>(Html);
                    //dynamic data = JObject.Parse(Html);
                    responseStream.Close();
                }

                WebResponse.Close();
          
            }
            catch (System.Net.WebException UnautorizedException)
            {
                // this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void Securities()
        {
            const string WEBSERVICE_URL = "https://api.gbm.com/v1/markets/BMV/securities/RNO%20N/level-one";
            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);

                myHttpWebRequest.Headers.Add("Mobile-platform", "android");
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Headers.Add("Accept-Encoding", "identity");
                myHttpWebRequest.Headers.Add("Device-Type", "Mi A2 Lite");
                myHttpWebRequest.Headers.Add("Os-version", "Android 29");
                myHttpWebRequest.Headers.Add("App-version", "60");
                myHttpWebRequest.Headers.Add("Platform", "android");

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<List<Ticker>>(Html);
                    //dynamic data = JObject.Parse(Html);
                    responseStream.Close();
                }

                WebResponse.Close();
             
            }
            catch (System.Net.WebException UnautorizedException)
            {
                // this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public contractBuyingPower GetContractBuyingPower()
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Operation/GetContractBuyingPower";

            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                string postData = "{\"request\":\"" + ContractID + "\"}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
              
                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<contractBuyingPower>(Html);


                    responseStream.Close();
                    return JsonData;
                }

                WebResponse.Close();

            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        public bool CancelOrder(long orderID)
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Operation/CancelOrder";
            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                string postData = "{\"electronicOrderId\":" + orderID.ToString() + ",\"vigencia\":false,\"isPreDispatchOrder\":false}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
                // Console.WriteLine("The value of 'ContentLength' property after sending the data is {0}", myHttpWebRequest.ContentLength);

                // Close the Stream object.

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    //var JsonData = JsonConvert.DeserializeObject<{ string Response; }>(Html);
                    var definition = new { response = false };

                    // string json1 = @"{'Name':'James'}";
                    var customer1 = JsonConvert.DeserializeAnonymousType(Html, definition);
                    //dynamic data = JObject.Parse(Html);
                    responseStream.Close();
                    return customer1.response;
                }

                WebResponse.Close();


            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }


        public OrderResponse GenerateOrder(string Ticker, int quantity, OrderTypes orderType, Decimal Price, InstrumentTypes instrumentType, ref bool RecurrenceFlag)
        {

            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Operation/RegisterCapitalOrder";
            try
            {
                if (RecurrenceFlag)
                {
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                    myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                    myHttpWebRequest.ContentType = "application/json";

                    myHttpWebRequest.Method = "POST";
                    myHttpWebRequest.Accept = "application/json";
                    myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                    myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                    myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                    //string postData = "{\"electronicOrderId\":" + orderID + ",\"vigencia\":false,\"isPreDispatchOrder\":false}";
                    var PostObject = new PurchaseOrderObject(ContractID, Ticker, quantity, orderType, Price, instrumentType);
                    var postData = PostObject.StringifyOrder();
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] byte1 = encoding.GetBytes(postData);
                    myHttpWebRequest.ContentLength = byte1.Length;
                    Stream newStream = myHttpWebRequest.GetRequestStream();

                    newStream.Write(byte1, 0, byte1.Length);
                    // Console.WriteLine("The value of 'ContentLength' property after sending the data is {0}", myHttpWebRequest.ContentLength);

                    // Close the Stream object.

                    HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    if (WebResponse.StatusDescription == "OK")
                    {
                        Stream responseStream = WebResponse.GetResponseStream();
                        if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                            responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                        else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                            responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                        StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                        string Html = Reader.ReadToEnd();
                        var JsonData = JsonConvert.DeserializeObject<OrderResponse>(Html);
                        //var definition = new { response = false };

                        // string json1 = @"{'Name':'James'}";
                        //var customer1 = JsonConvert.DeserializeAnonymousType(Html, definition);
                        //dynamic data = JObject.Parse(Html);
                        responseStream.Close();
                        return JsonData;
                    }

                    WebResponse.Close();
                    RecurrenceFlag = false;
                }


            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
                GenerateOrder(Ticker, quantity, orderType, Price, instrumentType, ref RecurrenceFlag);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        public PositionSummaryData GetPositionSummary()
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Portfolio/GetPositionSummary";
            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                string postData = "{\"request\":\"" + ContractID + "\"}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
                // Console.WriteLine("The value of 'ContentLength' property after sending the data is {0}", myHttpWebRequest.ContentLength);

                // Close the Stream object.

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<PositionSummaryData>(Html);

                    //dynamic data = JObject.Parse(Html);
                    responseStream.Close();
                    return JsonData;
                    //{"mercadosGlobalesSIC":[{"positionValueType":0,"issueId":"BRZU *","issueName":"Direxion Daily MSCI Brazil Bull 2X Share","instrumentType":2,"quantity":5,"averagePrice":1683.46000000,"lastPrice":2245.342498,"closePrice":2245.342498,"weightedAveragePrice":0.0,"yieldValue":2809.41249000,"marketValue":11226.712490,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.3338,"averageCost":8417.30000000,"positionPercentage":0.0708},{"positionValueType":0,"issueId":"ENPH *","issueName":"ENPHASE ENERGY, INC","instrumentType":2,"quantity":1,"averagePrice":765.50000000,"lastPrice":3690.0,"closePrice":3418.223368,"weightedAveragePrice":0.0,"yieldValue":2924.50000000,"marketValue":3690.0,"dailyVariationPercentage":0.0795,"historicalVariationPercentage":3.8204,"averageCost":765.50000000,"positionPercentage":0.0233},{"positionValueType":0,"issueId":"GE *","issueName":"GENERAL ELECTRIC CO.","instrumentType":2,"quantity":1,"averagePrice":222.42000000,"lastPrice":212.0,"closePrice":207.784479,"weightedAveragePrice":0.0,"yieldValue":-10.42000000,"marketValue":212.0,"dailyVariationPercentage":0.0203,"historicalVariationPercentage":-0.0468,"averageCost":222.42000000,"positionPercentage":0.0013},{"positionValueType":0,"issueId":"INO *","issueName":"INOVIO PHARMACEUTICALS, INC.","instrumentType":2,"quantity":1,"averagePrice":194.00000000,"lastPrice":186.48,"closePrice":193.098661,"weightedAveragePrice":0.0,"yieldValue":-7.52000000,"marketValue":186.48,"dailyVariationPercentage":-0.0343,"historicalVariationPercentage":-0.0388,"averageCost":194.00000000,"positionPercentage":0.0012},{"positionValueType":0,"issueId":"LK N","issueName":"LUCKIN COFFEE INC","instrumentType":2,"quantity":1,"averagePrice":665.72590000,"lastPrice":68.025754,"closePrice":68.025754,"weightedAveragePrice":0.0,"yieldValue":-597.70014600,"marketValue":68.025754,"dailyVariationPercentage":0.0,"historicalVariationPercentage":-0.8978,"averageCost":665.72590000,"positionPercentage":0.0004},{"positionValueType":0,"issueId":"NIO N","issueName":"NIO INC","instrumentType":2,"quantity":138,"averagePrice":1064.48000000,"lastPrice":1030.0,"closePrice":1061.546493,"weightedAveragePrice":0.0,"yieldValue":-4758.24000000,"marketValue":142140.0,"dailyVariationPercentage":-0.0297,"historicalVariationPercentage":-0.0324,"averageCost":146898.24000000,"positionPercentage":0.8961},{"positionValueType":0,"issueId":"Subtotal","instrumentType":0,"quantity":0,"averagePrice":0.0,"lastPrice":157523.218244,"closePrice":161607.260786,"weightedAveragePrice":0.0,"yieldValue":360.03234400,"marketValue":157523.21,"dailyVariationPercentage":-0.0252714560356795576879149344,"historicalVariationPercentage":0.0023,"averageCost":157163.18590000,"positionPercentage":0.9930}],
                    //"mercadoCapitales":[{"positionValueType":1,"issueId":"GCARSO A1","issueName":"GRUPO CARSO S.A. DE C.V.","instrumentType":0,"quantity":11,"averagePrice":68.97323636,"lastPrice":64.56,"closePrice":65.35,"weightedAveragePrice":0.0,"yieldValue":-48.54559996,"marketValue":710.16,"dailyVariationPercentage":-0.0121,"historicalVariationPercentage":-0.0640,"averageCost":758.70559996,"positionPercentage":0.0045},{"positionValueType":1,"issueId":"Subtotal","instrumentType":0,"quantity":0,"averagePrice":0.0,"lastPrice":710.16,"closePrice":718.85,"weightedAveragePrice":0.0,"yieldValue":-48.54559996,"marketValue":710.16,"dailyVariationPercentage":-0.0120887528691660290742157613,"historicalVariationPercentage":-0.0640,"averageCost":758.70559996,"positionPercentage":0.0045}],
                    //"sociedadesInversionDeuda":[{"positionValueType":5,"issueId":"GBMF2 BF","issueName":"GBM FONDO DE MERCADO DE DINERO","instrumentType":27,"quantity":1,"averagePrice":36.98950000,"lastPrice":36.99974,"closePrice":36.99974,"weightedAveragePrice":0.0,"yieldValue":0.01024000,"marketValue":36.99974,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0003,"averageCost":36.98950000,"positionPercentage":0.0002},{"positionValueType":5,"issueId":"Subtotal","instrumentType":0,"quantity":0,"averagePrice":0.0,"lastPrice":36.99974,"closePrice":36.99974,"weightedAveragePrice":0.0,"yieldValue":0.01024000,"marketValue":36.99,"dailyVariationPercentage":-0.0002632450930736270038654326,"historicalVariationPercentage":0.0000,"averageCost":36.98950000,"positionPercentage":0.0002}],
                    //"efectivo":[{"positionValueType":27,"issueId":"EFEC.  MISMO DIA","issueName":"EFEC.  MISMO DIA","instrumentType":-1,"quantity":0,"averagePrice":0.00000000,"lastPrice":0.0,"closePrice":0.0,"weightedAveragePrice":0.0,"yieldValue":16.68000000,"marketValue":16.68,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0,"averageCost":0.00000000,"positionPercentage":0.0001},{"positionValueType":27,"issueId":"EFEC. 24 HRS.","issueName":"EFEC. 24 HRS.","instrumentType":-1,"quantity":0,"averagePrice":0.00000000,"lastPrice":0.0,"closePrice":0.0,"weightedAveragePrice":0.0,"yieldValue":341.47000000,"marketValue":341.47,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0,"averageCost":0.00000000,"positionPercentage":0.0022},{"positionValueType":27,"issueId":"EFEC. 48 HRS.","issueName":"EFEC. 48 HRS.","instrumentType":-1,"quantity":0,"averagePrice":0.00000000,"lastPrice":0.0,"closePrice":0.0,"weightedAveragePrice":0.0,"yieldValue":0.00000000,"marketValue":0.0,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0,"averageCost":0.00000000,"positionPercentage":0.0},{"positionValueType":27,"issueId":"EFEC. MAYOR 48 HRS.","issueName":"EFEC. MAYOR 48 HRS.","instrumentType":-1,"quantity":0,"averagePrice":0.00000000,"lastPrice":0.0,"closePrice":0.0,"weightedAveragePrice":0.0,"yieldValue":0.00000000,"marketValue":0.0,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0,"averageCost":0.00000000,"positionPercentage":0.0},{"positionValueType":27,"issueId":"Subtotal","instrumentType":0,"quantity":0,"averagePrice":0.0,"lastPrice":0.0,"closePrice":0.0,"weightedAveragePrice":0.0,"yieldValue":358.15000000,"marketValue":358.15,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0,"averageCost":0.00000000,"positionPercentage":0.0023}],
                    //"totalPortfolioValue":[{"positionValueType":1000,"issueId":"Valor total de la cartera","issueName":"Valor total de la cartera","instrumentType":-1,"quantity":0,"averagePrice":0.00000000,"lastPrice":0.0,"closePrice":0.0,"weightedAveragePrice":0.0,"yieldValue":158628.52000000,"marketValue":158628.52,"dailyVariationPercentage":0.0,"historicalVariationPercentage":0.0,"averageCost":0.00000000,"positionPercentage":1.0000}]}
                }

                WebResponse.Close();


            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public PositionData GetPosition(string Ticker)
        {
            var Summary = GetPositionSummary();
            if (Summary != null)
            {
                var AllPositionsInMarket = new List<PositionData>();
                if (Summary.mercadosGlobalesSIC != null)
                {
                    AllPositionsInMarket.AddRange(Summary.mercadosGlobalesSIC);
                }
                if (Summary.mercadoCapitales != null)
                {

                    AllPositionsInMarket.AddRange(Summary.mercadoCapitales);
                }
                //var AllPositionsInMarket = Summary.mercadosGlobalesSIC.Concat(Summary.mercadoCapitales);
                var Tick = AllPositionsInMarket.Where(x => x.issueId == Ticker).FirstOrDefault();
                return Tick;

            }
            else
            {
                return null;
            }
        }


        public List<PositionData> GetAllPosition()
        {
            var Summary = GetPositionSummary();
            if (Summary != null)
            {
                var AllPositionsInMarket = new List<PositionData>();
                if (Summary.mercadosGlobalesSIC != null)
                {
                    AllPositionsInMarket.AddRange(Summary.mercadosGlobalesSIC);
                }
                if (Summary.mercadoCapitales != null)
                {

                    AllPositionsInMarket.AddRange(Summary.mercadoCapitales);
                }
                //var AllPositionsInMarket = Summary.mercadosGlobalesSIC.Concat(Summary.mercadoCapitales);
                //var Tick = AllPositionsInMarket.Where(x => x.issueId == Ticker).FirstOrDefault();
                return AllPositionsInMarket;

            }
            else
            {
                return null;
            }
        }

        public List<OrdersBlotter> GetAllOrders()
        {
            const string WEBSERVICE_URL = "https://homebroker-api.gbm.com/GBMP/api/Operation/GetBlotterByInstrument";
            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");

                DateTime utc = DateTime.UtcNow;
                var tzi = TimeZoneInfo.GetSystemTimeZones();
                TimeZoneInfo zone = TZConvert.GetTimeZoneInfo("Central Standard Time");
                //TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
                var culture = new CultureInfo("EN-US", true);
                var abbrDay = culture.DateTimeFormat.GetAbbreviatedDayName(localDateTime.DayOfWeek);
                var abbrMonth = culture.DateTimeFormat.GetAbbreviatedMonthName(localDateTime.Month);
                var dateText = DateTime.UtcNow.ToString("yyyy-MM-dd");
                String hourMinute = DateTime.Now.ToString("HH:mm:ss");
                var DateS = abbrDay
                    + " "
                    + abbrMonth
                    + " "
                    + dateText.Split('-')[2]
                    + " "
                    + hourMinute
                    + " CST "
                    + localDateTime.Year.ToString();

                string postData = "{\"contractId\":\"" + ContractID + "\",\"instrumentTypes\":[0,2,27,28],\"processDate\":\"" + DateS + "\"}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
                // Console.WriteLine("The value of 'ContentLength' property after sending the data is {0}", myHttpWebRequest.ContentLength);

                // Close the Stream object.

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<List<OrdersBlotter>>(Html);
                    //var definition = new { response = false };

                    // string json1 = @"{'Name':'James'}";
                    //var customer1 = JsonConvert.DeserializeAnonymousType(Html, definition);
                    //dynamic data = JObject.Parse(Html);
                    responseStream.Close();
                    return JsonData;
                }

                WebResponse.Close();


            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public TickerData GetTickerHistorical(string Ticker, int ElementsToTake)
        {
            string WEBSERVICE_URL = Uri.EscapeUriString("https://homebroker-api.gbm.com/GBMP/api/Market/GetInstrumentPricesIntradayPPP/" + Ticker);

            try
            {

                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(WEBSERVICE_URL);
                myHttpWebRequest.UserAgent = "okhttp/3.10.0";
                myHttpWebRequest.ContentType = "application/json";

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + this.BearerKey);
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                myHttpWebRequest.Headers.Add("Mobile-platform", "android");


                //string postData = "{\"issueId\":\"" + Ticker + "\",\"operationDate\":\"" + DateTime.UtcNow.ToString("yyyy-MM-dd") + "\"}";
                string postData = "{\"isOnLine\":true}";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                myHttpWebRequest.ContentLength = byte1.Length;
                Stream newStream = myHttpWebRequest.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);
                // Console.WriteLine("The value of 'ContentLength' property after sending the data is {0}", myHttpWebRequest.ContentLength);

                // Close the Stream object.

                HttpWebResponse WebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (WebResponse.StatusDescription == "OK")
                {
                    Stream responseStream = WebResponse.GetResponseStream();
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                    StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                    string Html = Reader.ReadToEnd();
                    var JsonData = JsonConvert.DeserializeObject<List<IntradayPrices>>(Html);
                    var Elements = JsonData.Select(x => x.price).Reverse().Skip(1).Take(ElementsToTake).ToList();//.Average();
                    var MEdian = Elements.Average();
                    var Weights = this.weigthsToMedian(ElementsToTake);
                    var WeigthtedMEdian = Elements.Select((x, index) => x * Weights[index]).Sum();

                    responseStream.Close();
                    //   return JsonData;
                    var NewTicker = new TickerData();
                    NewTicker.CurrentPrice = Elements.LastOrDefault();
                    NewTicker.Median = MEdian;
                    NewTicker.WeigthedMedian = WeigthtedMEdian;
                    NewTicker.IntradayPricesList = Elements;
                    return NewTicker;
                }

                WebResponse.Close();

            }
            catch (System.Net.WebException UnautorizedException)
            {
                this.Autenthicate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;


        }

        private List<double> weigthsToMedian(int ToProces)
        {
            var listadedobles = new List<double>();
            double Denominador = 2d;
            for (var i = 0; i < ToProces; i++)
            {
                listadedobles.Add(1d / Denominador);
                Denominador *= 2;
            }
            //listadedobles.Reverse();
            return listadedobles;

        }
    }
    public class OrdersBlotter
    {
        public string orderId;
        public string contractId;
        public int orderLife;
        public int intstrumentTypeId;
        public string processDate;
        public GBM.OrderState orderStatus;
        public int capitalOrderTypeId;
        public int algoTradingTypeId;
        public bool bitBuy;
        public string issueId;
        public Decimal orderPrice;
        public Decimal avaragePrice;
        public long originalQuantity;
        public long assignedQuantity;
        public Decimal commision;
        public bool isCancelable;
        public string issueName;
        public bool isPredFix;
        public bool isOrderAlive;
        public long predOrderId;
        public long orderLifeId;
    }

    public class OrderResponse
    {
        public long electronicOrderId;
        public int predespachadorId;

        public int vigenciaId;
    }
    public class PositionSummaryData
    {
        public List<PositionData> mercadosGlobalesSIC;
        public List<PositionData> mercadoCapitales;
        public List<PositionData> sociedadesInversionDeuda;
        public List<PositionData> efectivo;
        public List<PositionData> totalPortfolioValue;
      


    }
    public class PositionData
    {
        public int positionValueType;
        public string issueId;
        public string issueName;
        public int instrumentType;
        public int quantity;
        public double averagePrice;
        public double lastPrice;
        public double closePrice;
        public double weightedAveragePrice;
        public double yieldValue;
        public double marketValue;
        public Decimal dailyVariationPercentage;
        public double historicalVariationPercentage;
        public double averageCost;
        public double positionPercentage;
    }
    public class TickerData
    {
        public Double Median;
        public Double WeigthedMedian;
        public Double CurrentPrice;
        public double StandardDeviation;
        public List<double> IntradayPricesList;
    }
    public class IntradayPrices
    {
        public string date;
        public double price;
        public double percentChange;
        public long volume;
        public Decimal change;
    }
    public class TickerHistoricalData
    {
        public string dateIssueInterval;
        public string issueId;
        public double weightedAvgPrice;
        public double operatedVolume;
        public double operatedImport;
        public double closePrice;
    }
    public class contractBuyingPower
    {
        public Decimal buyingPower;
        public Decimal marketValueTotal;
        public Decimal TotalCash;
        public bool tradeMargin;
        public Decimal pendingOrdersRisk;
        public Decimal vistualGBMF2;
        public Decimal reporto;
    }

    //{"contractId":"AAG57601","duration":"1","orders":{"algoTradingTypeId":0,"capitalOrderTypeId":8,"hash":"11609793796673AAG57601GCARSOA118","instrumentType":0,"issueId":"GCARSO A1","price":66.90,"quantity":1}}

    class PurchaseOrderObject
    {
        public string contractId;
        public string duration;
        public List<OrderObject> orders;
        public PurchaseOrderObject(string Contract, string Ticker, int Quantity,GBMProyect.GBM.OrderTypes orderTypes, Decimal Price, GBMProyect.GBM.InstrumentTypes instrumentType)
        {
            contractId = Contract;
            duration = "1";
            var NewOrder = new OrderObject();
            NewOrder.quantity = Quantity;
            NewOrder.capitalOrderTypeId = (int)orderTypes;
            NewOrder.issueId = Ticker;
            NewOrder.hash = GenerateHash(Contract, Ticker, ref NewOrder);
            NewOrder.price = Price;
            NewOrder.instrumentType = (int)instrumentType;
            NewOrder.algoTradingTypeId = 0;
            List<OrderObject> NewList = new List<OrderObject>() { NewOrder };
            orders = NewList;

        }
        private string GenerateHash(string Contact, string tick, ref OrderObject order)
        {
            try
            {
                var Milis = (long)(((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
                //var Milis = (DateTime.UtcNow - (new DateTime(1970, 1, 1))).Milliseconds;
                var TheFirst = order;//;orders.FirstOrDefault();
                var name = TheFirst.issueId.Replace(" ", "");
                return Milis.ToString() + Contact + name + TheFirst.quantity.ToString() + TheFirst.capitalOrderTypeId.ToString();
            }
            catch (Exception)
            {

                return "";
            }

        }
        public string StringifyOrder()
        {
            var Json = JsonConvert.SerializeObject(this);
            return Json;
        }
        public class OrderObject
        {
            public int algoTradingTypeId;
            public int capitalOrderTypeId;
            public string hash;
            public int instrumentType = 0;
            public string issueId;
            public Decimal price;
            public int quantity;
        }

    }


    public class Ticker
    {
        public string issueId;
        public Decimal openPrice;
        public Decimal maxPrice;
        public Decimal minPrice;
        public double percentageChange;
        public Decimal valueChange;
        public long aggregatedVolume;
        public Decimal bidPrice;
        public long bidVolume;
        public Decimal askPrice;
        public long askVolume;
        public double ipcParticipationRate;
        public Decimal lastPrice;
        public Decimal closePrice;
        public long riseLowTypeId;
        public long instrumentTypeId;
        public long benchmarkId;
        public string benchmarkName;
        public Decimal benchmarkPercentage;
    }
    class LoginResponse
    {
        public string signInRedirect;
        public string accessToken;
        public string identityToken;
        public string refreshToken;
        public string tokenType;
        public int expiresIn;
        public int code;
        public string id;
        public string message;

    }
}

