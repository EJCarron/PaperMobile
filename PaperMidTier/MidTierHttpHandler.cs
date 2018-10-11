using System;
using System.Web;
using System.Data;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.IO;
using System.Text;
using PaperGeneral;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SqlWrapper;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace test2_mid {
    public class MidTierHttpHandler : IHttpHandler {
        public bool IsReusable {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }





        public void ProcessRequest(HttpContext context) {

            ServerResponse report = new ServerResponse();

            //write your handler implementation here.

            HttpRequest request = context.Request;

            string requestBody = getRequestBody(request);


            //----Get CommandObject type from URI--------------
            string cmdObjTypeAsStr = request.QueryString["cmdObjType"];

            if (cmdObjTypeAsStr == null){
                report.errorResponse.setError("No Comman Object type");
            }

            //int cmdObjTypeAsInt = -1;

            //if (report.errorResponse.isOk){
            //    if (!int.TryParse(cmdObjTypeAsStr,out cmdObjTypeAsInt)){
            //        report.errorResponse.setError("Command Object Type is not an int");
            //    }
            //}

            ECmdObjType cmdObjTypeAsEnum = ECmdObjType.none;

            if(report.errorResponse.isOk){

                try {
                    cmdObjTypeAsEnum = (ECmdObjType)Enum.Parse(typeof(ECmdObjType), cmdObjTypeAsStr);
                }catch{

                    report.errorResponse.setError("invalid cmd obj type");
                }

            }


            //----Do Command to DataBase-------------------
            if (report.errorResponse.isOk){
                
                report =  DoDataBaseCommand(cmdObjTypeAsEnum,requestBody);

            }



            string requestMethod = request.HttpMethod;



            int ii = 0;




            //-------------------------build Response---------------------------

            string serverResponse = JsonConvert.SerializeObject(report);


            context.Response.AppendHeader("Test", "response");

            context.Response.Write(serverResponse);
        }










        public string getRequestBody(HttpRequest request){

            string body;

            using(Stream receiveStream = request.InputStream){

                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                    body = readStream.ReadToEnd();
                }
            }

            return body;
        }




        public ServerResponse DoDataBaseCommand(ECmdObjType objType, string requestBody){

            ServerResponse dataBaseResponse = null;

            try {
                switch (objType) {

                    case ECmdObjType.CrtAssignmentObj:
                        CreateAssignmentObj cAO = JsonConvert.DeserializeObject<CreateAssignmentObj>(requestBody);

                        return dataBaseResponse =   createAssignmnetOnDB(cAO);
                    

                    case ECmdObjType.CrtClassRoomObj:
                        CreateClassRoomObj cCO = JsonConvert.DeserializeObject<CreateClassRoomObj>(requestBody);

                        return dataBaseResponse =  createClassRoomOnDB(cCO);


                    case ECmdObjType.CrtUserObj:
                        CreateUserObj cUO =  JsonConvert.DeserializeObject<CreateUserObj>(requestBody);

                        return dataBaseResponse =  createUserOnDB(cUO);


                    case ECmdObjType.DltAssignmentObj:
                        DeleteAssignmentObj dAO = JsonConvert.DeserializeObject<DeleteAssignmentObj>(requestBody);

                        return dataBaseResponse =  deleteAssignmentFromDB(dAO);


                    case ECmdObjType.DltClassRoomObj:
                        DeleteClassRoomObj dCO = JsonConvert.DeserializeObject<DeleteClassRoomObj>(requestBody);

                        return dataBaseResponse =  deleteClassRoomFromDB(dCO);


                    case ECmdObjType.DltUserObj:
                        DeleteUserObj dUO = JsonConvert.DeserializeObject<DeleteUserObj>(requestBody);

                        return dataBaseResponse =  deleteUserFromDB(dUO);


                    case ECmdObjType.EdtAssignmentObj:
                        EditAssignmentObj eAO = JsonConvert.DeserializeObject<EditAssignmentObj>(requestBody);

                        return dataBaseResponse =  editAssignmentOnDB(eAO);


                    case ECmdObjType.EdtUserObj:
                        EditUserObj eUO = JsonConvert.DeserializeObject<EditUserObj>(requestBody);

                        return dataBaseResponse =  editUserOnDb(eUO);


                    case ECmdObjType.MrkAssignmentDnObj:
                        MarkAssignmentDoneObj mADO = JsonConvert.DeserializeObject<MarkAssignmentDoneObj>(requestBody);

                        return dataBaseResponse =  markAssignmentDoneOnDB(mADO);


                    case ECmdObjType.UnMrkAssignmentDnObj:
                        UndoMarkAssignmentDoneObj uMADO = JsonConvert.DeserializeObject<UndoMarkAssignmentDoneObj>(requestBody);

                        return dataBaseResponse =  unMarkAssignmentDoneOnDB(uMADO);


                    case ECmdObjType.SubStudentObj:
                        SubscribeStudentObj sSO = JsonConvert.DeserializeObject<SubscribeStudentObj>(requestBody);

                        return dataBaseResponse =  subStudentOnDB(sSO);


                    case ECmdObjType.UnSubStudentObj:
                        UnsubscribeStudentObj uSSO = JsonConvert.DeserializeObject<UnsubscribeStudentObj>(requestBody);

                        return dataBaseResponse = unSubStudentOnDB(uSSO);

                    case ECmdObjType.initAssignmentObj:
                        InitForAssignmentObj iFAO = JsonConvert.DeserializeObject<InitForAssignmentObj>(requestBody);

                        return dataBaseResponse = getAssignmentDataFromDB(iFAO);

                    case ECmdObjType.initClassRoomObj:
                        InitForClassRoomObj iFCO = JsonConvert.DeserializeObject<InitForClassRoomObj>(requestBody);

                        return dataBaseResponse = getClassRoomDataFromDB(iFCO);

                    case ECmdObjType.initStudentObj:
                        InitForStudentObj iFSO = JsonConvert.DeserializeObject<InitForStudentObj>(requestBody);

                        return dataBaseResponse = getStudentDataFromDB(iFSO);

                    case ECmdObjType.initTeacherObj:
                        InitForTeacherObj iFTO = JsonConvert.DeserializeObject<InitForTeacherObj>(requestBody);

                        return dataBaseResponse = getTeacherDataFromDB(iFTO);

                    case ECmdObjType.assignmentFetchObj:
                        AssignmentFetchObject AFO = JsonConvert.DeserializeObject<AssignmentFetchObject>(requestBody);
                        return dataBaseResponse = fetchAssignmentObjectFromDB(AFO) ;

                    case ECmdObjType.classRoomFetchObj:
                        ClassRoomFetchObject CFO = JsonConvert.DeserializeObject<ClassRoomFetchObject>(requestBody);
                        return dataBaseResponse = fetchClassRoomObjectFromDB(CFO);

                    case ECmdObjType.markedDoneFetchObj:
                        MarkedDoneFetchObject MDFO = JsonConvert.DeserializeObject<MarkedDoneFetchObject>(requestBody);
                        return dataBaseResponse = fetchMarkedDoneObjectFromDB(MDFO) ;

                    case ECmdObjType.subFetchObj:
                        SubscriptionFetchObject SFO = JsonConvert.DeserializeObject<SubscriptionFetchObject>(requestBody);
                        return dataBaseResponse = fetchSubObjectFromDB(SFO);

                    case ECmdObjType.userFetchObj:
                        UserFetchObject UFO = JsonConvert.DeserializeObject<UserFetchObject>(requestBody);
                        return dataBaseResponse = fetchUserObjectFromDB(UFO);

                    
                        
                }
            }catch{

                dataBaseResponse.errorResponse.setError("Unsuccesful");

                return dataBaseResponse;

            }

            return null;


        }

        //------------------------DATA BASE METHODS ---------------------------



        public ServerResponse fetchAssignmentObjectFromDB(AssignmentFetchObject AFO){
            return new ServerResponse();
        }




        public ServerResponse fetchClassRoomObjectFromDB(ClassRoomFetchObject CFO) {
            
            SELECT select = new SELECT()
                .star()
                .FROM(Tables.ClassRooms)
                .WHERE(
                    new OperatorExpression()
                    .addExpression(ClassRooms.teacherId)
                    .Equals()
                    .addExpression(new IntLiteral(Convert.ToInt32(CFO.dataMembers[1])))
                    .AND()
                    .addExpression(ClassRooms.classRoomName)
                    .Equals()
                    .addExpression(new StringLiteral(Convert.ToString(CFO.dataMembers[2])))
                );

            ServerResponse response = new ServerResponse();

            int returnTeacherId = -1;
            string returnClassRoomName = "";


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);
            MySqlCommand command = select.makeMySqlCommand(conn,ERenderType.NonParamed);

            try{

                MySqlDataReader reader = command.ExecuteReader();

                while(reader.Read()){

                    returnTeacherId = Convert.ToInt32(reader[0]);

                    returnClassRoomName = Convert.ToString(reader[1]);

                }
                reader.Close();

            }catch(Exception ex){

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            response.fetchObject = new ClassRoomFetchObject(returnTeacherId, returnClassRoomName);

            return response;

        }





        public ServerResponse fetchMarkedDoneObjectFromDB(MarkedDoneFetchObject MFO){
            return new ServerResponse();
        }

        public ServerResponse fetchSubObjectFromDB(SubscriptionFetchObject SFO) {
            return new ServerResponse();
        }

        public ServerResponse fetchUserObjectFromDB(UserFetchObject  UFO) {
            return new ServerResponse();
        }

        public ServerResponse createAssignmnetOnDB(CreateAssignmentObj cAO){

            return new
                ServerResponse();
        }

        public ServerResponse createClassRoomOnDB(CreateClassRoomObj cCO){

            return new ServerResponse();
        }

        public ServerResponse createUserOnDB(CreateUserObj cUO){

            return new ServerResponse();
        }

        public ServerResponse deleteAssignmentFromDB(DeleteAssignmentObj dAO){

            return new ServerResponse();
        }

        public ServerResponse deleteClassRoomFromDB(DeleteClassRoomObj dCO){

            return new ServerResponse();
        }

        public ServerResponse deleteUserFromDB(DeleteUserObj dUO){

            return new ServerResponse();
        }

        public ServerResponse editAssignmentOnDB(EditAssignmentObj eAO){

            return new ServerResponse();
        }

        public ServerResponse editUserOnDb(EditUserObj eUO){

            return new ServerResponse();
        }

        public ServerResponse markAssignmentDoneOnDB(MarkAssignmentDoneObj mADO){

            return new ServerResponse();
        }

        public ServerResponse unMarkAssignmentDoneOnDB(UndoMarkAssignmentDoneObj uMADO){

            return new ServerResponse();
        }



        //public ServerResponse subStudentOnDB(SubscribeStudentObj sSo){

        //    ServerResponse sResponse = new ServerResponse();

        //    sResponse.errorResponse = new Response();

        //    Model_Student student = new Model_Student("Ed", 2);

        //    string modelStudentJson = JsonConvert.SerializeObject(student);



        //    ModelUpdate studentUpdate = new ModelUpdate(modelStudentJson, student.type, DataKeys.getStudentKey(student.id));

        //    sResponse.modelUpdates.Add(studentUpdate);

        //    return sResponse;
        //}


        public ServerResponse subStudentOnDB(SubscribeStudentObj sSo) {
            ServerResponse sResponse = new ServerResponse();
            sResponse.errorResponse = new Response();

            return new ServerResponse();


        
        }


        public ServerResponse unSubStudentOnDB(UnsubscribeStudentObj uSSO){
            return new ServerResponse();
        }



        public ServerResponse getAssignmentDataFromDB(InitForAssignmentObj iFAO){
            return new ServerResponse();
        }



        public ServerResponse getClassRoomDataFromDB(InitForClassRoomObj iFCO){


            return new ServerResponse();
        }


        public ServerResponse getStudentDataFromDB(InitForStudentObj iFSO){
            return new ServerResponse();
        }

        public ServerResponse getTeacherDataFromDB(InitForTeacherObj iFTO){
            return new ServerResponse();
        }
    }


    class ccc : Dictionary<String,object>{


    }


}
