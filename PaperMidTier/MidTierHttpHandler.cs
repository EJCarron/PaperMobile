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

            if (cmdObjTypeAsStr == null) {
                report.errorResponse.setError("No Comman Object type");
            }

            //int cmdObjTypeAsInt = -1;

            //if (report.errorResponse.isOk){
            //    if (!int.TryParse(cmdObjTypeAsStr,out cmdObjTypeAsInt)){
            //        report.errorResponse.setError("Command Object Type is not an int");
            //    }
            //}

            ECmdObjType cmdObjTypeAsEnum = ECmdObjType.none;

            if (report.errorResponse.isOk) {

                try {
                    cmdObjTypeAsEnum = (ECmdObjType)Enum.Parse(typeof(ECmdObjType), cmdObjTypeAsStr);
                } catch {

                    report.errorResponse.setError("invalid cmd obj type");
                }

            }


            //----Do Command to DataBase-------------------
            if (report.errorResponse.isOk) {

                report = DoDataBaseCommand(cmdObjTypeAsEnum, requestBody);

            }



            string requestMethod = request.HttpMethod;



            int ii = 0;




            //-------------------------build Response---------------------------

            string serverResponse = JsonConvert.SerializeObject(report);


            context.Response.AppendHeader("Test", "response");

            context.Response.Write(serverResponse);
        }










        public string getRequestBody(HttpRequest request) {

            string body;

            using (Stream receiveStream = request.InputStream) {

                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                    body = readStream.ReadToEnd();
                }
            }

            return body;
        }




        public ServerResponse DoDataBaseCommand(ECmdObjType objType, string requestBody) {

            ServerResponse dataBaseResponse = null;

            try {
                switch (objType) {

                    case ECmdObjType.CrtAssignmentObj:
                        CreateAssignmentObj cAO = JsonConvert.DeserializeObject<CreateAssignmentObj>(requestBody);

                        return dataBaseResponse = createAssignmnetOnDB(cAO);


                    case ECmdObjType.CrtClassRoomObj:
                        CreateClassRoomObj cCO = JsonConvert.DeserializeObject<CreateClassRoomObj>(requestBody);

                        return dataBaseResponse = createClassRoomOnDB(cCO);


                    case ECmdObjType.CrtUserObj:
                        CreateUserObj cUO = JsonConvert.DeserializeObject<CreateUserObj>(requestBody);

                        return dataBaseResponse = createUserOnDB(cUO);

                    case ECmdObjType.ArchAssignmentObj:
                        ChangeAssignmentArchiveObj cAAO = JsonConvert.DeserializeObject<ChangeAssignmentArchiveObj>(requestBody);

                        return dataBaseResponse = changeArchiveOfAssignmentOnDB(cAAO);

                    case ECmdObjType.ArchClassRoomObj:
                        ChangeClassRoomArchiveObj dCO = JsonConvert.DeserializeObject<ChangeClassRoomArchiveObj>(requestBody);

                        return dataBaseResponse = changeArchiveOfClassRoomOnDB(dCO);


                    case ECmdObjType.ArchUserObj:
                        ChangeUserArchiveObj dUO = JsonConvert.DeserializeObject<ChangeUserArchiveObj>(requestBody);

                        return dataBaseResponse = changeArchiveOfUserOnDB(dUO);


                    case ECmdObjType.EdtAssignmentObj:
                        EditAssignmentObj eAO = JsonConvert.DeserializeObject<EditAssignmentObj>(requestBody);

                        return dataBaseResponse = editAssignmentOnDB(eAO);


                    case ECmdObjType.EdtUserObj:
                        EditUserObj eUO = JsonConvert.DeserializeObject<EditUserObj>(requestBody);

                        return dataBaseResponse = editUserOnDb(eUO);


                    case ECmdObjType.MrkAssignmentDnObj:
                        MarkAssignmentDoneObj mADO = JsonConvert.DeserializeObject<MarkAssignmentDoneObj>(requestBody);

                        return dataBaseResponse = markAssignmentDoneOnDB(mADO);


                    case ECmdObjType.UnMrkAssignmentDnObj:
                        UndoMarkAssignmentDoneObj uMADO = JsonConvert.DeserializeObject<UndoMarkAssignmentDoneObj>(requestBody);

                        return dataBaseResponse = unMarkAssignmentDoneOnDB(uMADO);


                    case ECmdObjType.SubStudentObj:
                        SubscribeStudentObj sSO = JsonConvert.DeserializeObject<SubscribeStudentObj>(requestBody);

                        return dataBaseResponse = subStudentOnDB(sSO);


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
                        return dataBaseResponse = fetchAssignmentObjectFromDB(AFO);

                    case ECmdObjType.classRoomFetchObj:
                        ClassRoomFetchObject CFO = JsonConvert.DeserializeObject<ClassRoomFetchObject>(requestBody);
                        return dataBaseResponse = fetchClassRoomObjectFromDB(CFO);

                    case ECmdObjType.markedDoneFetchObj:
                        MarkedDoneFetchObject MDFO = JsonConvert.DeserializeObject<MarkedDoneFetchObject>(requestBody);
                        return dataBaseResponse = fetchMarkedDoneObjectFromDB(MDFO);

                    case ECmdObjType.subFetchObj:
                        SubscriptionFetchObject SFO = JsonConvert.DeserializeObject<SubscriptionFetchObject>(requestBody);
                        return dataBaseResponse = fetchSubObjectFromDB(SFO);

                    case ECmdObjType.userFetchObj:
                        UserFetchObject UFO = JsonConvert.DeserializeObject<UserFetchObject>(requestBody);
                        return dataBaseResponse = fetchUserObjectFromDB(UFO);

                    case ECmdObjType.AttemptLoginObj:
                        AttemptLoginObj ALO = JsonConvert.DeserializeObject<AttemptLoginObj>(requestBody);
                        return dataBaseResponse = attemptLoginOnDB(ALO);
                }
            } catch {

                dataBaseResponse.errorResponse.setError("Unsuccesful");

                return dataBaseResponse;

            }

            return null;


        }

        //------------------------DATA BASE METHODS ---------------------------


        public ServerResponse attemptLoginOnDB(AttemptLoginObj aLO){

            ServerResponse response = new ServerResponse();

            SELECT select = new SELECT()
                .col(Users.userId)
                .col(Users.userType)
                .col(Users.isClosed)
                .col(LoginDetails.salt)
                .col(LoginDetails.saltedHash)
                .FROMJOIN(Tables.Users, Tables.LoginDetails, new OperatorExpression()
                          .addExpression(Users.userId)
                          .Equals()
                          .addExpression(LoginDetails.userId)
                         )
                .WHERE(new OperatorExpression()
                       .addExpression(LoginDetails.emailAddress)
                       .Equals()
                       .addExpression(new StringLiteral(aLO.email))
                      );

            int userId = -1;
            int userType = -1;
            int isClosed = -1;
            string salt ="";
            string saltedHash = "";

            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);


            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    userId = reader.GetInt32(Users.userId);
                    userType = reader.GetInt32(Users.userType);
                    isClosed = reader.GetInt32(Users.isClosed);
                    salt = reader.GetString(LoginDetails.salt);
                    saltedHash = reader.GetString(LoginDetails.saltedHash);
                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            if(userId == -1 || userType == -1 ){
                response.errorResponse.setError("Incorrect email or Password");
                return response;
            }


            bool isPasswordCorrect = SaltedHash.isPasswordCorrect(aLO.password, salt, saltedHash);

            if(!isPasswordCorrect){
                response.errorResponse.setError("Incorrect email or Password");
                return response;
               
            }


            if(isClosed == 1){
                response.errorResponse.setError("Your account is archived");
                return response;
            }





            switch(userType){

                case 1:
                    response.loginObj = new LoginFetchObject(userId, EModelUserType.teacher);
                    response.modelUpdates.Add(makeTeacherModelUpdate(response, userId));
                    return response;

                case 2:
                    response.loginObj = new LoginFetchObject(userId, EModelUserType.student);
                    response.modelUpdates.Add(makeStudentModelUpdate(response, userId));
                    return response;
            }

            return response;
        }


        public ServerResponse fetchAssignmentObjectFromDB(AssignmentFetchObject AFO) {
            AssignmentFetchObject assignment = null;

            ServerResponse response = new ServerResponse();

            SELECT select = new SELECT()
                .star()
                .FROM(Tables.Assignments)
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.assignmentId)
                       .Equals()
                       .addExpression(new IntLiteral(Convert.ToInt32(AFO.dataMembers[1])))
                      );
            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);



            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);


            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    assignment = new AssignmentFetchObject(
                        reader.GetInt32(Assignments.assignmentId),
                        reader.GetString(Assignments.classRoomName),
                        reader.GetDateTime(Assignments.dueDate),
                        reader.GetString(Assignments.description),
                        reader.GetBoolean(Assignments.isClosed),
                        reader.GetString(Assignments.assignmentName)

                    );

                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();


            response.fetchObject = assignment;

            return response;

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

            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------


            ServerResponse response = new ServerResponse();

            int returnTeacherId = -1;
            string returnClassRoomName = "";

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);



            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);


            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    returnTeacherId = Convert.ToInt32(reader[ClassRooms.teacherId.columnName]);

                    returnClassRoomName = Convert.ToString(reader[ClassRooms.classRoomName.columnName]);



                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();



            response.fetchObject = new ClassRoomFetchObject(returnTeacherId, returnClassRoomName);

            return response;

        }




        // DOESNT WORK - No clue why?
        public ServerResponse fetchMarkedDoneObjectFromDB(MarkedDoneFetchObject MFO) {
            ServerResponse response = new ServerResponse();


            //Something goes wrong after this line
            // debug stops working and then breaks and gives unclear error back on bridge.
            SELECT select = new SELECT()
                .star()
                .FROM(Tables.AssignmentCompletions)
                .WHERE(new OperatorExpression()
                       .addExpression(AssignmentCompletions.assignmentId)
                       .Equals()
                       .addExpression(new IntLiteral((int)MFO.dataMembers[1]))
                       .AND()
                       .addExpression(AssignmentCompletions.studentId)
                       .Equals()
                       .addExpression(new IntLiteral((int)MFO.dataMembers[2]))
                      );


            Console.WriteLine("TEST");
            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);

            MarkedDoneFetchObject markedDone = null;

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    markedDone = new MarkedDoneFetchObject(
                        reader.GetInt32(AssignmentCompletions.assignmentId),
                        reader.GetInt32(AssignmentCompletions.studentId)

                    );

                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            response.fetchObject = markedDone;

            return response;

        }






        public ServerResponse fetchSubObjectFromDB(SubscriptionFetchObject SFO) {
            ServerResponse response = new ServerResponse();

            SELECT select = new SELECT()
                .star()
                .FROM(Tables.Subscriptions)
                .WHERE(new OperatorExpression()
                       .addExpression(Subscriptions.studentId)
                       .Equals()
                       .addExpression(new IntLiteral((int)SFO.dataMembers[1]))
                       .AND()
                       .addExpression(Subscriptions.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral((string)SFO.dataMembers[2]))
                      );
            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);

            SubscriptionFetchObject subscription = null;

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    subscription = new SubscriptionFetchObject(

                        reader.GetInt32(Subscriptions.studentId),
                        reader.GetString(Subscriptions.classRoomName)
                    );

                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();


            response.fetchObject = subscription;
            return response;

        }






        public ServerResponse fetchUserObjectFromDB(UserFetchObject UFO) {
            ServerResponse response = new ServerResponse();

            SELECT select = new SELECT()
                .col(Users.userId)
                .col(Users.userName)
                .col(Users.userType)
                .col(LoginDetails.emailAddress)
                .col(LoginDetails.userPassword)
                .FROMJOIN(Tables.Users, Tables.LoginDetails,
                          new OperatorExpression()
                          .addExpression(Users.userId)
                          .Equals()
                          .addExpression(LoginDetails.userId)
                         )
                .WHERE(new OperatorExpression()
                       .addExpression(Users.userId)
                       .Equals()
                       .addExpression(new IntLiteral((int)UFO.dataMembers[1]))
                      );

            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);

            UserFetchObject user = null;

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    user = new UserFetchObject(
                        reader.GetInt32(Users.userId),
                        reader.GetString(LoginDetails.emailAddress),
                        reader.GetString(LoginDetails.userPassword),
                        reader.GetString(Users.userName),
                        reader.GetInt32(Users.userType)
                    );

                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            response.fetchObject = user;

            return response;
        }






        public ServerResponse createAssignmnetOnDB(CreateAssignmentObj cAO) {

            ServerResponse response = new ServerResponse();

            INSERTINTO insert = new INSERTINTO(Tables.Assignments)
                .ValuePair(Assignments.classRoomName, new StringLiteral(cAO.classRoomName))
                .ValuePair(Assignments.dueDate, new DateLiteral(cAO.dueDate))
                .ValuePair(Assignments.description, new StringLiteral(cAO.description))
                .ValuePair(Assignments.isClosed, new BoolLiteral(cAO.isClosed))
                .ValuePair(Assignments.assignmentName, new StringLiteral(cAO.assignmentName));

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);

            int newAssignmentId = -1;

            try {
                conn.Open();
                command.ExecuteNonQuery();
                newAssignmentId = (int)command.LastInsertedId;



            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            ModelUpdate teachearUpdate = makeTeacherModelUpdate(response, cAO.teacherId);

            response.modelUpdates.Add(teachearUpdate);

            return response;

        }



        public ServerResponse createClassRoomOnDB(CreateClassRoomObj cCO) {

            INSERTINTO insert = new INSERTINTO(Tables.ClassRooms)
                .ValuePair(ClassRooms.teacherId, new IntLiteral(cCO.teacherId))
                .ValuePair(ClassRooms.classRoomName, new StringLiteral(cCO.classRoomName));

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            ServerResponse response = new ServerResponse();

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }


            response.modelUpdates.Add(makeTeacherModelUpdate(response, cCO.teacherId));

            return response;

        }


        public bool areDetailsUnique(ServerResponse response, CreateUserObj cUO){

          
            SELECT select = new SELECT()
                .col(LoginDetails.emailAddress)
                .col(LoginDetails.userPassword)
                .FROM(Tables.LoginDetails)
                .WHERE(new OperatorExpression()
                       .addExpression(LoginDetails.emailAddress)
                       .Equals()
                       .addExpression(new StringLiteral(cUO.email))
                      );



            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);

            string email = null;



            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    email = reader.GetString(LoginDetails.emailAddress);


                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            if(email == null){
                return true;
            }
            return false;
        }


        public ServerResponse createUserOnDB(CreateUserObj cUO) {

            ServerResponse response = new ServerResponse();

            if (areDetailsUnique(response, cUO)) {


                int newUserId = -1;

                INSERTINTO insert = new INSERTINTO(Tables.Users)
                    .ValuePair(Users.userName, new StringLiteral(cUO.userName))
                    .ValuePair(Users.userType, new IntLiteral((int)cUO.userType));


                //Debug Code------------------------------------------------

                Console.WriteLine(insert.render(ERenderType.NonParamed));
                //Debug Code------------------------------------------------



                MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

                MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);

                try {
                    conn.Open();
                    command.ExecuteNonQuery();
                    newUserId = (int)command.LastInsertedId;

                } catch (Exception ex) {

                    response.errorResponse.setError(ex.ToString());
                }

                conn.Close();

                if (newUserId == -1) {
                    response.errorResponse.setError("User creation failed");
                    return response;
                }

                createUserLoginDetails(response, newUserId, cUO);

                switch (cUO.userType) {

                    case EModelUserType.student:
                        response.modelUpdates.Add(makeStudentModelUpdate(response, newUserId));
                        response.loginObj = new LoginFetchObject(newUserId, EModelUserType.student);
                        break;

                    case EModelUserType.teacher:
                        response.modelUpdates.Add(makeTeacherModelUpdate(response, newUserId));
                        response.loginObj = new LoginFetchObject(newUserId, EModelUserType.teacher);
                        break;
                }

                return response;
            }else{

                response.errorResponse.setError("Email already in use");
                return response;
            }
        }



        public void createUserLoginDetails(ServerResponse response, int newUserId, CreateUserObj cUO) {

            //-------------------Make the salt and Hash------------------

            SaltedHash hash = new SaltedHash(cUO.password);

            string salt = hash.salt;
            string saltedHash = hash.saltedHash;

            //------------------Enter Login Details ---------------------
            INSERTINTO insert = new INSERTINTO(Tables.LoginDetails)
                .ValuePair(LoginDetails.emailAddress, new StringLiteral(cUO.email))
                .ValuePair(LoginDetails.userId, new IntLiteral(newUserId))
                .ValuePair(LoginDetails.userPassword, new StringLiteral(cUO.password))
                .ValuePair(LoginDetails.salt, new StringLiteral(salt))
                .ValuePair(LoginDetails.saltedHash, new StringLiteral(saltedHash))
                ;

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();


            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

        }


        public ServerResponse changeArchiveOfAssignmentOnDB(ChangeAssignmentArchiveObj cAAO) {

            ServerResponse response = new ServerResponse();

            int closure = -1;

            if (cAAO.toArchive) {
                closure = 1;
            } else {
                closure = 0;
            }

            UPDATE update = new UPDATE(Tables.Assignments)
                .addValuePair(Assignments.isClosed, new IntLiteral(closure))
                .WHEREEQUALS(Assignments.assignmentId, new IntLiteral(cAAO.assignmentId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();


            response.modelUpdates.Add(makeTeacherModelUpdate(response, cAAO.teacherId));

            return response;
        }


        public ServerResponse changeArchiveOfClassRoomOnDB(ChangeClassRoomArchiveObj aCO) {

            ServerResponse response = new ServerResponse();

            switch (aCO.toArchive) {
                case true:
                    changeClassRoomClosure(response, aCO.classRoomName, 1);
                    break;

                case false:
                    changeClassRoomClosure(response, aCO.classRoomName, 0);
                    break;
            }

            response.modelUpdates.Add(makeTeacherModelUpdate(response, aCO.teacherId));

            return response;
        }


        public void changeClassRoomClosure(ServerResponse response, string classRoomName, int closure) {

            UPDATE update = new UPDATE(Tables.ClassRooms)
                .addValuePair(ClassRooms.isClosed, new IntLiteral(closure))
                .WHERE(new OperatorExpression()
                       .addExpression(ClassRooms.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(classRoomName))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            changeClassRoomAssignmentsClosure(response, classRoomName, closure);
        }



        public void changeClassRoomAssignmentsClosure(ServerResponse response, string classRoomName, int closure) {

            UPDATE update = new UPDATE(Tables.Assignments)
                .addValuePair(Assignments.isClosed, new IntLiteral(closure))
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(classRoomName))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();
        }




        public ServerResponse changeArchiveOfUserOnDB(ChangeUserArchiveObj aUO) {

            ServerResponse response = new ServerResponse();

            switch (aUO.toArchive) {
                case true:
                    changeUserClosure(response, aUO.userId, 1, aUO.userType);
                    break;

                case false:
                    changeUserClosure(response, aUO.userId, 0, aUO.userType);
                    break;
            }

            switch (aUO.userType) {
                case EModelUserType.student:
                    response.modelUpdates.Add(makeStudentModelUpdate(response, aUO.userId));
                    break;

                case EModelUserType.teacher:
                    response.modelUpdates.Add(makeTeacherModelUpdate(response, aUO.userId));
                    break;
            }

            return response;

        }



        public void changeUserClosure(ServerResponse response, int userId, int closure, EModelUserType userType) {

            UPDATE update = new UPDATE(Tables.Users)
                .addValuePair(Users.isClosed, new IntLiteral(closure))
                .WHEREEQUALS(Users.userId, new IntLiteral(userId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();


            if (userType == EModelUserType.teacher) {
                changeUserClassRoomsClosure(response, userId, closure);
            }
        }

        public void changeUserClassRoomsClosure(ServerResponse response, int userId, int closure) {

            UPDATE update = new UPDATE(Tables.ClassRooms)
                .addValuePair(ClassRooms.isClosed, new IntLiteral(closure))
                .WHEREEQUALS(ClassRooms.teacherId, new IntLiteral(userId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            changeUserAssignmentsClosure(response, userId, closure);
        }

        public void changeUserAssignmentsClosure(ServerResponse response, int userId, int closure) {

            UPDATE update = new UPDATE(Tables.Assignments)
                .JOIN(Tables.ClassRooms, new OperatorExpression()
                      .addExpression(Assignments.classRoomName)
                      .Equals()
                      .addExpression(ClassRooms.classRoomName)
                     )
                .addValuePair(Assignments.isClosed, new IntLiteral(closure))
                .WHEREEQUALS(ClassRooms.teacherId, new IntLiteral(userId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

        }


        public ServerResponse editAssignmentOnDB(EditAssignmentObj eAO) {

            ServerResponse response = new ServerResponse();

            int isClosed = 0;

            if (eAO.isClosed) {
                isClosed = 1;
            }

            UPDATE update = new UPDATE(Tables.Assignments)
                .addValuePair(Assignments.assignmentName, new StringLiteral(eAO.assignmentName))
                .addValuePair(Assignments.description, new StringLiteral(eAO.assignmentDescription))
                .addValuePair(Assignments.dueDate, new DateLiteral(eAO.assignmentDueDate))
                .addValuePair(Assignments.isClosed, new IntLiteral(isClosed))
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.assignmentId)
                       .Equals()
                       .addExpression(new IntLiteral(eAO.assignmentId))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            response.modelUpdates.Add(makeTeacherModelUpdate(response, eAO.teacherId));

            return response;

        }






        public ServerResponse editUserOnDb(EditUserObj eUO) {

            ServerResponse response = new ServerResponse();

            if (eUO.email != null) {
                changeUserData(response, eUO.userId, eUO.email, LoginDetails.emailAddress, Tables.LoginDetails, LoginDetails.userId);
            }

            if (eUO.password != null) {
                changeUserData(response, eUO.userId, eUO.password, LoginDetails.userPassword, Tables.LoginDetails, LoginDetails.userId);
            }

            if (eUO.userName != null) {
                changeUserData(response, eUO.userId, eUO.userName, Users.userName, Tables.Users, Users.userId);
            }


            switch (eUO.userType) {

                case EModelUserType.student:
                    response.modelUpdates.Add(makeStudentModelUpdate(response, eUO.userId));
                    break;

                case EModelUserType.teacher:
                    response.modelUpdates.Add(makeTeacherModelUpdate(response, eUO.userId));
                    break;
            }

            return response;

        }


        public void changeUserData(ServerResponse response, int userId, string newData, SqlWrapper.COLUMN column, SqlWrapper.DataTable table, SqlWrapper.COLUMN whereColumn) {
            UPDATE update = new UPDATE(table)
                .addValuePair(column, new StringLiteral(newData))
                .WHERE(new OperatorExpression()
                       .addExpression(whereColumn)
                       .Equals()
                       .addExpression(new IntLiteral(userId))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();


        }



        public ServerResponse markAssignmentDoneOnDB(MarkAssignmentDoneObj mADO) {

            ServerResponse response = new ServerResponse();

            INSERTINTO insert = new INSERTINTO(Tables.AssignmentCompletions)
                .ValuePair(AssignmentCompletions.assignmentId, new IntLiteral(mADO.assignmentId))
                .ValuePair(AssignmentCompletions.studentId, new IntLiteral(mADO.studentId))
                .ValuePair(AssignmentCompletions.completionDate, new DateLiteral(DateTime.Now));


            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }


            response.modelUpdates.Add(makeStudentModelUpdate(response, mADO.studentId));

            return response;
        }

        public ServerResponse unMarkAssignmentDoneOnDB(UndoMarkAssignmentDoneObj uMADO) {

            ServerResponse response = new ServerResponse();

            DELETE delete = new DELETE().FROM(Tables.AssignmentCompletions).WHERE(new OperatorExpression()
                                                                                  .addExpression(AssignmentCompletions.assignmentId)
                                                                                  .Equals()
                                                                                  .addExpression(new IntLiteral(uMADO.assignmentId))
                                                                                  .AND()
                                                                                  .addExpression(AssignmentCompletions.studentId)
                                                                                  .Equals()
                                                                                  .addExpression(new IntLiteral(uMADO.studentId))
                                                                                 );

            //Debug Code------------------------------------------------

            Console.WriteLine(delete.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = delete.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }


            response.modelUpdates.Add(makeStudentModelUpdate(response, uMADO.studentId));

            return response;

        }

        public bool checkIfClassRoomExists(ServerResponse response, string classRoomName){

            SELECT select = new SELECT()
                .col(ClassRooms.classRoomName)
                .FROM(Tables.ClassRooms)
                .WHEREEquals(ClassRooms.classRoomName, new StringLiteral(classRoomName));


            string roomName = null;

            //DEBUG CODE---------------------------------------------
            Console.WriteLine(select.render(ERenderType.NonParamed));
            //DEBUG CODE---------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.NonParamed);



            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    roomName = reader.GetString(ClassRooms.classRoomName);

                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            if(roomName == null){
                return false;
            }
            return true;

        }


        public ServerResponse subStudentOnDB(SubscribeStudentObj sSo) {

            ServerResponse response = new ServerResponse();


            if(!checkIfClassRoomExists(response, sSo.classRoomName)){

                response.errorResponse.setError("ClassRoom Does Not Exist");
                return response;
            }

            INSERTINTO insert = new INSERTINTO(Tables.Subscriptions)
                .ValuePair(Subscriptions.classRoomName, new StringLiteral(sSo.classRoomName))
                .ValuePair(Subscriptions.studentId, new IntLiteral(sSo.studentId));

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }


            response.modelUpdates.Add(makeStudentModelUpdate(response, sSo.studentId));

            return response;

        }


        public ServerResponse unSubStudentOnDB(UnsubscribeStudentObj uSSO) {
            ServerResponse response = new ServerResponse();

            DELETE delete = new DELETE().FROM(Tables.Subscriptions).WHERE(new OperatorExpression()
                                                                          .addExpression(Subscriptions.classRoomName)
                                                                          .Equals()
                                                                          .addExpression(new StringLiteral(uSSO.classRoomName))
                                                                          .AND()
                                                                          .addExpression(Subscriptions.studentId)
                                                                          .Equals()
                                                                          .addExpression(new IntLiteral(uSSO.studentId))
                                                                         );

            //Debug Code------------------------------------------------

            Console.WriteLine(delete.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = delete.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }


            response.modelUpdates.Add(makeStudentModelUpdate(response, uSSO.studentId));

            return response;

        }



        public ServerResponse getAssignmentDataFromDB(InitForAssignmentObj iFAO) {
            return new ServerResponse();
        }



        public ServerResponse getClassRoomDataFromDB(InitForClassRoomObj iFCO) {


            return new ServerResponse();
        }


        public ServerResponse getStudentDataFromDB(InitForStudentObj iFSO) {
            ServerResponse response = new ServerResponse();

            response.modelUpdates.Add(makeStudentModelUpdate(response, iFSO.studentId));

            return response;
        }

        public ServerResponse getTeacherDataFromDB(InitForTeacherObj iFTO) {
            ServerResponse response = new ServerResponse();

            response.modelUpdates.Add(makeTeacherModelUpdate(response, iFTO.teacherId));

            return response;
        }



        //=====================Make Model Updates==========================


        //-----------------------------Student Update ------------------------------

        public ModelUpdate makeStudentModelUpdate(ServerResponse response, int userId) {

            ModelUser student = makeModelUser(response, userId, EModelUserType.student);

            Collection<ModelAssignment> assignments = getStudentAssignments(response, userId);

            Collection<string> subscriptions = getSubscriptions(response, userId);

            student.assignments = assignments;
            student.classRooms = subscriptions;

            string studentJson = JsonConvert.SerializeObject(student);

            ModelUpdate studentUpdate = new ModelUpdate(studentJson, EModelType.modelUser, DataKeys.getUserKey(userId));

            return studentUpdate;
        }

        public ModelUser makeModelUser(ServerResponse response, int studentId, EModelUserType userType) {
            SELECT select = new SELECT()
                .star()
                .FROM(Tables.Users)
                .WHERE(new OperatorExpression()
                       .addExpression(Users.userId)
                       .Equals()
                       .addExpression(new IntLiteral(studentId))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            ModelUser student = null;

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    student = new ModelUser(
                        reader.GetString(Users.userName),
                        reader.GetInt32(Users.userId),
                        userType
                    );

                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            return student;


        }

        Collection<ModelAssignment> getStudentAssignments(ServerResponse response, int studentId) {

            Collection<ModelAssignment> assignments = new Collection<ModelAssignment>();

            SELECT select = new SELECT()
                .col(Assignments.assignmentName)
                .col(Assignments.dueDate)
                .col(Assignments.assignmentId)
                .col(Assignments.classRoomName)
                .col(Assignments.description)
                .col(AssignmentCompletions.completionDate)
                .FROM(
                    new JOIN(EJoinType.Join, Tables.Assignments, Tables.Subscriptions,
                             new OperatorExpression()
                             .addExpression(Assignments.classRoomName)
                             .Equals()
                             .addExpression(Subscriptions.classRoomName)
                             .AND()
                             .addExpression(Subscriptions.studentId)
                             .Equals()
                             .addExpression(new IntLiteral(studentId))
                 ),
                    new JOIN(EJoinType.Left, null, Tables.AssignmentCompletions,
                             new OperatorExpression()
                             .addExpression(AssignmentCompletions.assignmentId)
                             .Equals()
                             .addExpression(Assignments.assignmentId)
                             .AND()
                             .addExpression(AssignmentCompletions.studentId)
                             .Equals()
                             .addExpression(Subscriptions.studentId)

                            )
                )
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    bool isDone = true;

                    var ordinal = reader.GetOrdinal(AssignmentCompletions.completionDate);

                    if (reader.IsDBNull(ordinal)) {

                        isDone = false;
                    }

                    assignments.Add(new ModelAssignment(
                        reader.GetString(Assignments.assignmentName),
                        reader.GetInt32(Assignments.assignmentId),
                        reader.GetDateTime(Assignments.dueDate),
                        reader.GetString(Assignments.description),
                        isDone,
                        reader.GetString(Assignments.classRoomName)

                    ));


                }
                reader.Close();

            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            return assignments;



        }

        Collection<string> getSubscriptions(ServerResponse response, int studentId) {

            Collection<string> subscriptions = new Collection<string>();

            SELECT select = new SELECT()
                .col(Subscriptions.classRoomName)
                .FROMJOIN(Tables.Subscriptions, Tables.ClassRooms, new OperatorExpression()
                          .addExpression(Subscriptions.classRoomName)
                          .Equals()
                          .addExpression(ClassRooms.classRoomName)
                         )
                .WHERE(new OperatorExpression()
                       .addExpression(Subscriptions.studentId)
                       .Equals()
                       .addExpression(new IntLiteral(studentId))
                       .AND()
                       .addExpression(ClassRooms.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );
            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    subscriptions.Add(reader.GetString(Subscriptions.classRoomName));
                }
                reader.Close();
            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            return subscriptions;
        }




        //--------------------------Teacher Update------------------------------------

        ModelUpdate makeTeacherModelUpdate(ServerResponse response, int userId) {

            ModelUser teacher = makeModelUser(response, userId, EModelUserType.teacher);
            Collection<ModelAssignment> assignments = getTeacherAssignments(response, userId);
            Collection<string> classRooms = getTeacherClassRooms(response, userId);

            teacher.assignments = assignments;
            teacher.classRooms = classRooms;

            string teacherJson = JsonConvert.SerializeObject(teacher);

            ModelUpdate teacherUpdate = new ModelUpdate(teacherJson, EModelType.modelUser, DataKeys.getUserKey(userId));

            return teacherUpdate;
        }




        Collection<ModelAssignment> getTeacherAssignments(ServerResponse response, int userId){

            Collection<ModelAssignment> assignments = new Collection<ModelAssignment>();

            SELECT select = new SELECT()
                .col(Assignments.assignmentName)
                .col(Assignments.dueDate)
                .col(Assignments.assignmentId)
                .col(Assignments.classRoomName)
                .col(Assignments.description)
                .FROMJOIN(Tables.Assignments, Tables.ClassRooms, new OperatorExpression()
                          .addExpression(Assignments.classRoomName)
                          .Equals()
                          .addExpression(ClassRooms.classRoomName)
                         )
                .WHERE(new OperatorExpression()
                       .addExpression(ClassRooms.teacherId)
                       .Equals()
                       .addExpression(new IntLiteral(userId))
                       .AND()
                       .addExpression(Assignments.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );
            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    assignments.Add(new ModelAssignment(
                        reader.GetString(Assignments.assignmentName),
                        reader.GetInt32(Assignments.assignmentId),
                        reader.GetDateTime(Assignments.dueDate),
                        reader.GetString(Assignments.description),
                        false,
                        reader.GetString(Assignments.classRoomName)
                    ));
                   
                }
                reader.Close();
            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            return assignments;
        }






        Collection<string> getTeacherClassRooms (ServerResponse response, int userId){

            Collection<string> classRooms = new Collection<string>();

            SELECT select = new SELECT().col(ClassRooms.classRoomName).FROM(Tables.ClassRooms).WHERE(new OperatorExpression()
                                                                                                     .addExpression(ClassRooms.teacherId)
                                                                                                     .Equals()
                                                                                                     .addExpression(new IntLiteral(userId))
                                                                                                     .AND()
                                                                                                     .addExpression(ClassRooms.isClosed)
                                                                                                     .Equals()
                                                                                                     .addExpression(new IntLiteral(0))
                                                                                                    );
            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    classRooms.Add(reader.GetString(ClassRooms.classRoomName));

                }
                reader.Close();
            } catch (Exception ex) {

                response.errorResponse.setError(ex.ToString());
            }

            conn.Close();

            return classRooms;
        }
    }

    class ccc : Dictionary<String,object>{


    }


}
