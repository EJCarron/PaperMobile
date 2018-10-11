//using System;
//using System.Net.Http;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using PaperGeneral;
//using Newtonsoft.Json;

//namespace InternetObjs {
//    public class HttpBridge{

//        //---------SINGLETONCONSTRUCTOR-------------------------
//        private static HttpBridge uniqueInstance;

//        private HttpBridge() {        }


//        public static HttpBridge getInstance(){

//            if(uniqueInstance == null) {

//                uniqueInstance = new HttpBridge();
//            }
//            return uniqueInstance;
//        }





//        //------------HTTPREQUESTS------------------------------

//        public async Task<string> makeHttpRequest(string cmd){
            

//            string responseContent;

//            HttpClient client = new HttpClient();

//            Dictionary<string, string> values = new Dictionary<string, string> {

//                    {"cmnd", cmd}
//                };



//            try{
                
//                FormUrlEncodedContent form = new FormUrlEncodedContent(values);


//                HttpRequestMessage requestMessage = constructRequest("http://127.0.0.1:32767/ajax.ashx", "POST", "a=b");



//                Task<HttpResponseMessage> getResponseStringTask = client.SendAsync(requestMessage);

//                //Task<HttpResponseMessage> getResponseStringTask = client.PostAsync("http://127.0.0.1:32767/ajax.ashx", content);
//                HttpResponseMessage urlContents = await getResponseStringTask;

//                //string herd = urlContents.Headers["Test"];

//                responseContent = await urlContents.Content.ReadAsStringAsync();
//            }catch (Exception e) {

//                responseContent = e.Message;


//            }

//            return responseContent;


//        }









//        public async Task<ServerResponse> makeHttPostRequest(ICommandObject commandObject, ECmdObjType cmdObjType){

//            string responseContent;



//            HttpClient client = new HttpClient();


//            try {

//                string jsonString = JsonConvert.SerializeObject(commandObject);


//                string cmdObjTypePair = "cmdObjType=" + cmdObjType;

//                HttpRequestMessage requestMessage = constructRequest(
//                    "http://127.0.0.1:32767/ajax.ashx",
//                    "POST",
//                    jsonString,
//                    cmdObjTypePair
//                    );



//                Task<HttpResponseMessage> getResponseStringTask = client.SendAsync(requestMessage);

//                //Task<HttpResponseMessage> getResponseStringTask = client.PostAsync("http://127.0.0.1:32767/ajax.ashx", content);
//                HttpResponseMessage urlContents = await getResponseStringTask;

//                //string herd = urlContents.Headers["Test"];

//                responseContent = await urlContents.Content.ReadAsStringAsync();
//            } catch (Exception e) {


//                ServerResponse errorResponse = new ServerResponse();

//                Response error = new Response(ResponseState.error, e.Message);

//                errorResponse.errorResponse = error;

//                return errorResponse;

//            }

//            ServerResponse response = JsonConvert.DeserializeObject<ServerResponse>(responseContent);

//            return response;

//        }








//        private HttpRequestMessage constructRequest(
//            string uri,
//            string method,
//            string body,
//            params string[] uriParameters){

//            HttpRequestMessage requestMessage = new HttpRequestMessage();


//            string fullUriString = uri + "?";

//            bool ifFirst = true;
//            foreach(string parameter in uriParameters){
//                if (ifFirst) {
//                    fullUriString += parameter;
//                    ifFirst = false;
//                }else{

//                    fullUriString += ";" + parameter;
//                }
//            }

//            Uri tempUri = new Uri(fullUriString);



//            requestMessage.RequestUri = tempUri;

//            requestMessage.Method = new HttpMethod(method);

//            requestMessage.Content = new StringContent(body);

//            return requestMessage;
//        }

//    }


//}
