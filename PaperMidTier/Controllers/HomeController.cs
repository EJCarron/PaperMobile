//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Mvc.Ajax;
//using Newtonsoft.Json;
//using Common;

//namespace test2_mid.Controllers
//{

	

//    public class HomeController : Controller
//    {
//        public ActionResult Index()
//        {
//            var mvcName = typeof(Controller).Assembly.GetName();
//            var isMono = Type.GetType("Mono.Runtime") != null;

//            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
//            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

//            string json_cmnd = Request.Form["json_cmnd"];
//            if (!String.IsNullOrEmpty(json_cmnd)) {
//                Json_Cmd cmdObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Json_Cmd>(json_cmnd);
//                switch(cmdObject.cmd){

//                    case "Add":{

//                            string param = (string)cmdObject.paramaters; 
//                        }break;

//                }


//                var ii = 0;
//            }
//            return Json( JsonConvert.SerializeObject("Earth"));

//            //return View();



//        }
//    }
//}
