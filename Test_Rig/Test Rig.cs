using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using PaperGeneral;
using Newtonsoft.Json;
using The_Model;

namespace Test_Rig {



    class MainClass {
       




        public static void Main(string[] args) {
            HttpHomeWorkModel model = new HttpHomeWorkModel();

            ClassRoomFetchObject CFO = new ClassRoomFetchObject(1, "hist8");




            Task<ServerResponse> responseTask =  model.fetchDataBaseObject(CFO, ECmdObjType.classRoomFetchObj);

            //responseTask.RunSynchronously();


            if (responseTask.Result.fetchObject == null) {

                Console.WriteLine(responseTask.Result.errorResponse.message);

            } else {

                Console.Write(responseTask.Result.fetchObject.render());
            }
            //CommandCentre commandCentre = new CommandCentre();

            //commandCentre.addCommands(
            //    new InitStudent(model, 1),
            //    new InitTeacher(model, 2),
            //    new InitClassRoom(model, "Hist8"),
            //    new InitAssignment(model, 1)
            //);

            //Console.Write(commandCentre.renderCommands());

            //Collection<int> commandKeys = new Collection<int>();

            //bool go = true;

            //while (go){

            //    string  userInput = Console.ReadLine();
            //    int commandKey = -1;


            //    try{
            //        commandKey = Convert.ToInt32(userInput);
            //    }catch{

            //        Console.WriteLine("Invalid Command Key");
            //    }

            //    if(commandKey >= 0 && commandKey <= commandCentre.commands.Count){

            //        commandKeys.Add(commandKey);
            //    }else {
            //        Console.WriteLine("Invalid Command Key");

            //    }

            //    Console.WriteLine("Execute: G, OR enter Another: Press any Key");

            //    string userInput2 = Console.ReadLine();

            //    if(userInput2 == "g" || userInput2 == "G"){

            //        go = false;
            //    }

            //}


            //Console.Write(commandCentre.executeCommands(commandKeys));



        }
    }
}
