using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace PaperGeneral {
    public interface ICommandObject { }

    //-------------------DataKeys------------------------------

    public static class DataKeys {


        public const string teacher = "teacher";
        public const string classRoom = "classRoom";
        public const string assignment = "assignment";
        public const string student = "student";
        public const string user = "user";


        //++++++++++++++++++++++Database types++++++++++++++++++++=

        //---------------------DB Tables names ----------------------------


        public const string assignmentCompletions = "AssignmentCompletions";
        public const string assignments = "Assignments";
        public const string classRooms = "ClassRooms";
        public const string loginDetails = "LoginDetails";
        public const string subscriptions = "Subscriptions";
        public const string users = "Users";

        //-------------Column Names--------------------------


        public const string assignmentId = "assignmentId";
        public const string studentId = "studentId";
        public const string completionDate = "completionDate";
        public const string classRoomName = "classRoomName";
        public const string dueDate = "dueDate";
        public const string description = "description";
        public const string isClosed = "isClosed";
        public const string assignmentName = "assignmentName";
        public const string teacherId = "teacherId";
        public const string userId = "userId";
        public const string emailAddress = "emailAddress";
        public const string userPassWord = "UserPassword";
        public const string userName = "userName";
        public const string userType = "userType";


        public const string dataBaseConnectionString = "SERVER=mysql5.loosefoot.com;UID=edcarron102;PWD=futurama4;database=paper";


        //+++++++++++++++++++++Methods++++++++++++++++++++++++++++++++++++

        //static public string getTeacherKey(int teacherId) {

        //    return DataKeys.teacher + "_" + teacherId;
        //}

        static public string getClassRoomKey(string classRoomName) {

            return DataKeys.classRoom + "_" + classRoomName;
        }

        static public string getAssignmentKey(int assignmentId) {

            return DataKeys.assignment + "_" + assignmentId;
        }

        static public string getUserKey(int userId) {

            return DataKeys.user + "_" + userId;
        }


        //static public string getStudentKey(int studentId) {

        //    return DataKeys.student + "_" + studentId;
        //}
    }


    //++++++++++++++++++++++OperatorKeys+++++++++++++++++


    public static class OperatorKeys{

        static public string equalTo = "=";
        static public string notEqualTo = "!=";
        static public string greatThan = ">";
        static public string lessThan = "<";
        static public string greaterThanOrEqualTo = ">=";
        static public string lessThanOrEqualTo = "<=";
        static public string between = "BETWEEN";

    }


    //-------------------ENUMS--------------------------------
    //public enum EUserType {
    //    none = 0,
    //    teacher = 1,
    //    student = 2
    //}

    public enum ECmdObjType {

        none = 0,
        CrtAssignmentObj = 1,
        CrtClassRoomObj = 2,
        CrtUserObj = 3,
        ArchAssignmentObj = 4,
        ArchClassRoomObj = 5,
        ArchUserObj = 6,
        EdtAssignmentObj = 7,
        EdtUserObj = 8,
        MrkAssignmentDnObj = 9,
        UnMrkAssignmentDnObj = 10,
        SubStudentObj = 11,
        UnSubStudentObj = 12,
        initAssignmentObj = 13,
        initClassRoomObj = 14,
        initStudentObj = 15,
        initTeacherObj = 16,
        userFetchObj = 17,
        classRoomFetchObj = 18,
        assignmentFetchObj = 19,
        subFetchObj = 20,
        markedDoneFetchObj = 21,
        AttemptLoginObj = 22

    }

    public enum EOperatorTytpe{

        none = 0,
        EqualTo = 1,
        NotEqualTo = 2,
        GreaterThan = 3,
        LessThan = 4,
        GreaterThanOrEqualTo = 5,
        LessThanOrEqualTo = 6,
        Between = 7,
        Like = 8,
        In = 9

    }


    //++++++++++++++++COMMAND=OBJECTS++++++++++++++++++++++++

    //----------------ASSIGNMENT OBJECTS ------------------
    public class CreateAssignmentObj : ICommandObject {
        public string assignmentName;
        public DateTime dueDate;
        public string description;
        public bool isClosed;
        public string classRoomName;
        public int teacherId;

        public CreateAssignmentObj(
            string assignmentName,
            DateTime dueDate,
            string description,
            bool isClosed,
            string classRoomName,
            int teacherId
        ) {
            this.assignmentName = assignmentName;
            this.dueDate = dueDate;
            this.description = description;
            this.isClosed = isClosed;
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }
    }

    public class EditAssignmentObj : ICommandObject {
        public string assignmentName;
        public DateTime assignmentDueDate;
        public string assignmentDescription;
        public bool isClosed;
        public int assignmentId;
        public int teacherId;

        public EditAssignmentObj(
            string assignmentName,
            DateTime assignmentDueDate,
            string assignmentDescription,
            bool isClosed,
            int assignmentId,
            int teacherId
        ) {
            this.assignmentName = assignmentName;
            this.assignmentDueDate = assignmentDueDate;
            this.assignmentDescription = assignmentDescription;
            this.isClosed = isClosed;
            this.assignmentId = assignmentId;
            this.teacherId = teacherId;

        }

    }

    public class ChangeAssignmentArchiveObj : ICommandObject {
        public int assignmentId;
        public bool toArchive;
        public int teacherId;

        public ChangeAssignmentArchiveObj(int assignmentId, bool toArchive, int teacherId){
            this.assignmentId = assignmentId;
            this.toArchive = toArchive;
            this.teacherId = teacherId;
        }
    }

    //-------------CLASSROOM OBJECTS------------------

    public class CreateClassRoomObj : ICommandObject {
        public string classRoomName;
        public int teacherId;

        public CreateClassRoomObj(
            string classRoomName,
            int teacherId
                                 ) {

            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }
    }

    public class ChangeClassRoomArchiveObj : ICommandObject {
        public string classRoomName;
        public int teacherId;
        public bool toArchive;

        public ChangeClassRoomArchiveObj(string classRoomName, int teacherId, bool toArchive) {
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
            this.toArchive = toArchive;
        }
    }

    //------------USER OBJECTS---------------------------


    public class CreateUserObj : ICommandObject {
        public string userName;
        public string email;
        public string password;
        public EModelUserType userType;


        public CreateUserObj(
            string userName,
            string email,
            string password,
            EModelUserType userType
        ) {
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.userType = userType;
        }
    }

    public class EditUserObj : ICommandObject {
        public int userId;
        public string userName;
        public string email;
        public string password;
        public EModelUserType userType;


        // leave any data that doesnt need to be changed as NULL
        public EditUserObj(
            int userId,
            string userName,
            string email,
            string password,
            EModelUserType userType
        ) {
            this.userId = userId;
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.userType = userType;
        }
    }

    public class ChangeUserArchiveObj : ICommandObject {
        public int userId;
        public bool toArchive;
        public EModelUserType userType;

        public ChangeUserArchiveObj(int userId, bool toArchive, EModelUserType userType) {
            
            this.userId = userId;
            this.toArchive = toArchive;
            this.userType = userType;
        }
    }

    public class AttemptLoginObj : ICommandObject{
        public string email;
        public string password;

        public AttemptLoginObj(string email, string password){
            this.email = email;
            this.password = password;
        }

    }
    //----------------STUDENT OBJECTS -------------------------

    public class MarkAssignmentDoneObj : ICommandObject {
        public int assignmentId;
        public int studentId;


        public MarkAssignmentDoneObj(
            int assignmentId,
            int studentId
        ) {
            this.assignmentId = assignmentId;
            this.studentId = studentId;

        }
    }

    public class UndoMarkAssignmentDoneObj : ICommandObject {
        public int assignmentId;
        public int studentId;


        public UndoMarkAssignmentDoneObj(
            int assignmentId,
            int studentId
        ) {
            this.assignmentId = assignmentId;
            this.studentId = studentId;

        }
    }

    public class SubscribeStudentObj : ICommandObject {
        public int studentId;
        public string classRoomName;

        public SubscribeStudentObj(
            int studentId,
            string classRoomName
        ) {
            this.studentId = studentId;
            this.classRoomName = classRoomName;
        }

    }

    public class UnsubscribeStudentObj : ICommandObject {
        public int studentId;
        public string classRoomName;

        public UnsubscribeStudentObj(
            int studentId,
            string classRoomName
        ) {
            this.studentId = studentId;
            this.classRoomName = classRoomName;
        }

    }


    //----------------INITIALIZING OBJECTS-----------------------------

    public class InitForAssignmentObj : ICommandObject{
        public int assignmentId;

        public InitForAssignmentObj(int assignmentId){

            this.assignmentId = assignmentId;
        }
    }



    public class InitForClassRoomObj : ICommandObject{
        public string classRoomName;

        public InitForClassRoomObj(string classRoomName){

            this.classRoomName = classRoomName;
        }
    }



    public class InitForStudentObj : ICommandObject{
        public int studentId;

        public InitForStudentObj(int studentId){

            this.studentId = studentId;
        }
    }



    public class InitForTeacherObj : ICommandObject{
        public int teacherId;

        public InitForTeacherObj(int teacherId){
            this.teacherId = teacherId;
        }
    }

    //-------------------------Fetch Objects -----------------------------


    public class FetchObject : ICommandObject{

        public Dictionary<int, object> dataMembers = new Dictionary<int, object>();

        public bool isSame(FetchObject FO){
            for (int x = 1; x <= this.dataMembers.Count; x++) {

                if (this.dataMembers[x].ToString() != FO.dataMembers[x].ToString()) {
                    return false;
                }
            }

            return true;
        }

        public string render(){

            string renderString = "";

            foreach(KeyValuePair <int,object> dataMember in this.dataMembers){

                renderString += dataMember.Key.ToString() + ". " + dataMember.Value.ToString() + "\n";

            }

            return renderString;
        }
    }

    public class UserFetchObject : FetchObject{
        public UserFetchObject(
            int userId,
            string emailAddress,
            string userPassword,
            string userName,
            int userType
        ){
            this.dataMembers.Add(1,userId);
            this.dataMembers.Add(2, emailAddress);
            this.dataMembers.Add(3, userPassword);
            this.dataMembers.Add(4, userName);
            this.dataMembers.Add(5, userType);

        }

    }

    public class LoginFetchObject {
        public int userId;
        public EModelUserType userType;


        public LoginFetchObject(int userId, EModelUserType userType){
            this.userId = userId;
            this.userType = userType;

        }
    }

    public class ClassRoomFetchObject : FetchObject{
        public ClassRoomFetchObject(int teacherId, string classRoomName){
            this.dataMembers.Add(1,teacherId);
            this.dataMembers.Add(2,classRoomName);
        }
    }

    public class AssignmentFetchObject : FetchObject{
        

        public AssignmentFetchObject(
            int assignmentId,
            string classRoomName,
            DateTime dueDate,
            string description,
            bool isClosed,
            string assignmentName
        ){
            this.dataMembers.Add(1,assignmentId);
            this.dataMembers.Add(2,classRoomName);
            this.dataMembers.Add(3,dueDate);
            this.dataMembers.Add(4,description);
            this.dataMembers.Add(5,isClosed);
            this.dataMembers.Add(6,assignmentName);

        }

    }

    public class SubscriptionFetchObject : FetchObject{

        public SubscriptionFetchObject(int studentId,string classRoomName){
            this.dataMembers.Add(1,studentId);
            this.dataMembers.Add(2,classRoomName);
        }

    }

    public class MarkedDoneFetchObject : FetchObject{

        public MarkedDoneFetchObject(int assignmentId, int studentId){
            this.dataMembers.Add(1,assignmentId);
            this.dataMembers.Add(2,studentId);


        }

    }


    //++++++++++++++++++RESPONSE=OBJECTS+++++++++++++++++++++++++++++++++++



    //--------------------RESPONSES-------------------------

    public enum ResponseState {
        ok = 0,
        error = 1,
        bug = 2,

    }

    public enum SubResponseType {
        none = 0,
    }

    public class Response {
        public ResponseState state = ResponseState.ok;
        SubResponseType subState = SubResponseType.none;
        public string message = "";

        public Response() {

        }

        public Response(ResponseState state, string message) {

            this.state = state;
            this.message = message;
        }

        public Response(ResponseState state, SubResponseType subState, string message) {

            this.state = state;
            this.subState = subState;
            this.message = message;
        }

        public bool isOk {

            get { return this.state == ResponseState.ok; }

        }

        public void setError(string message){

            this.state = ResponseState.error;
            this.message = message;

        }
    }



    //---------------DATABASE RESPONSE---------------------------------


    public class ServerResponse {
        public Response errorResponse = new Response();
        public Collection<ModelUpdate> modelUpdates = new Collection<ModelUpdate>();
        public FetchObject fetchObject;
        public LoginFetchObject loginObj;
    }

     

    public class ModelUpdate{
       public string modelItemAsJson;
       public int modelType;
       public string dataKey;

        public ModelUpdate(string modelItemAsJson, EModelType modelType, string dataKey){
            this.modelItemAsJson = modelItemAsJson;
            this.modelType = (int)modelType;
            this.dataKey = dataKey;
        }
    }
}
