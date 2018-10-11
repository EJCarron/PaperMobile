using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using PaperGeneral;


namespace The_Model {
    public interface IModelStore {

        void updateItems(params DataItemUpdate[] updates);
        void updateItems(IList<DataItemUpdate> listOfUpdates, params DataItemUpdate[] updatesParam);
        void addWatcher(Watcher watcher, params string[] dataKeys);
        T getItem<T>(string dataKey);
    }

    public interface IHomeWorkModel : IModelStore {

        Task<ServerResponse> fetchDataBaseObject(FetchObject fetchObj, ECmdObjType cmdType);
        Task<ServerResponse> createUser(string userName, string email, string password, EModelUserType userType);
        Task<Response> addClassRoomToTeacher(string classRoomname, int teacherId);
        Task<Response> addAssignmentToClassRoom(string assignmentName, DateTime dueDate, string description, bool isClosed, string classRoomName, int teacherId);

        Task<Response> editUser(int userId, string userName, string email, string password, EModelUserType userType);
        Task<Response> editAssignment(string assignmentName, DateTime assignmentDueDate, string assignmentDescription, bool isClosed, int assignmentId, int teacherId);

        Task<Response> changeUserArchive(int userId, bool toArchive, EModelUserType userType);
        Task<Response> changeAssignmentArchive(int assignmentId,bool toArchive, int teacherId);
        Task<Response> changeClassRoomArchive(string classRoomName, bool toArchive, int teacherId);

        Task<Response> markAssignmentDone(int assignmentId, int studentId);
        Task<Response> undoMarkAssignmentDone(int assignmentId, int studentId);
        Task<Response> SubscribeStudentToClassRoom(int studentId, string classRoomName);
        Task<Response> unsubscribeStudent(int studentId, string classRoomName);
       
        Task<Response> initializeForTeacher(int teacherId);
        Task<Response> initializeForClassRoom(string classRoomName);
        Task<Response> initializeForAssignment(int assignmentId);
        Task<Response> initializeForStudent(int studentId);

        Task<ServerResponse> attemptLogin(string email, string password);


    }

    public class HttpHomeWorkModel : ModelStore, IHomeWorkModel {
        private HttpBridge bridge = HttpBridge.getInstance();


        //-------------------Tool Methods-------------------------------------

        public DataItemUpdate makeDataItemUpdate(ModelUpdate modelUpdate) {

            EModelType modelType = (EModelType)modelUpdate.modelType;

            switch (modelType) {

                case EModelType.modelAssignemt:
                    ModelAssignment assignment = JsonConvert.DeserializeObject<ModelAssignment>(modelUpdate.modelItemAsJson);
                    return new DataItemUpdate(modelUpdate.dataKey, assignment);

                case EModelType.modelClassRoom:
                    ModelClassRoom classRoom = JsonConvert.DeserializeObject<ModelClassRoom>(modelUpdate.modelItemAsJson);
                    return new DataItemUpdate(modelUpdate.dataKey, classRoom);

                case EModelType.modelUser:
                    ModelUser user = JsonConvert.DeserializeObject<ModelUser>(modelUpdate.modelItemAsJson);
                    return new DataItemUpdate(modelUpdate.dataKey, user);

            }
            return null;
        }


        public async Task<Response> doModelHttpRequest(ICommandObject cmdObject, ECmdObjType cmdType) {

            ServerResponse response = await this.bridge.makeHttPostRequest(cmdObject, cmdType);
            Collection<DataItemUpdate> updates = new Collection<DataItemUpdate>();

            foreach (ModelUpdate modelUpdate in response.modelUpdates) {

                updates.Add(this.makeDataItemUpdate(modelUpdate));
            }

            this.updateItems(updates);
            //Accept and implement any changes to the model received

            return response.errorResponse;

        }


        //--------------ModelMethods-----------------------------------------



        public async Task<ServerResponse> attemptLogin(string email, string password){

            AttemptLoginObj cmdObj = new AttemptLoginObj(email, password);

            ServerResponse response = await this.bridge.makeHttPostRequest(cmdObj, ECmdObjType.AttemptLoginObj);
            Collection<DataItemUpdate> updates = new Collection<DataItemUpdate>();

            foreach (ModelUpdate modelUpdate in response.modelUpdates) {

                updates.Add(this.makeDataItemUpdate(modelUpdate));
            }

            this.updateItems(updates);
            //Accept and implement any changes to the model received

            return response;

        }

        public async Task<Response> addAssignmentToClassRoom(string assignmentName, DateTime dueDate, string description, bool isClosed, string classRoomName, int teacherId) {

            //Send Comand to midTier

            CreateAssignmentObj cmdObj = new CreateAssignmentObj(
                assignmentName,
                dueDate,
                description,
                isClosed,
                classRoomName,
                teacherId
            );

            return await doModelHttpRequest(cmdObj, ECmdObjType.CrtAssignmentObj);
        }




        public async Task<Response> addClassRoomToTeacher(string classRoomname, int teacherId) {
            CreateClassRoomObj cmdObj = new CreateClassRoomObj(classRoomname, teacherId);

            return await doModelHttpRequest(cmdObj, ECmdObjType.CrtClassRoomObj);
        }



        public async Task<Response> changeAssignmentArchive(int assignmentId, bool toArchive, int teacherId) {
            ChangeAssignmentArchiveObj cmdObj = new ChangeAssignmentArchiveObj(assignmentId, toArchive, teacherId);
           

            return await doModelHttpRequest(cmdObj, ECmdObjType.ArchAssignmentObj);
        }



        public async Task<Response> changeClassRoomArchive(string classRoomName, bool toArchive, int teacherId) {
            ChangeClassRoomArchiveObj cmdObj = new ChangeClassRoomArchiveObj(classRoomName, teacherId, toArchive);

            return await doModelHttpRequest(cmdObj, ECmdObjType.ArchClassRoomObj);
        }



        public async Task<Response> editAssignment(
            string assignmentName,
            DateTime assignmentDueDate,
            string assignmentDescription,
            bool isClosed,
            int assignmentId,
            int teacherId
        ) {
            EditAssignmentObj cmdObj = new EditAssignmentObj(
                assignmentName,
                assignmentDueDate,
                assignmentDescription,
                isClosed,
                assignmentId,
                teacherId
            );


            return await doModelHttpRequest(cmdObj, ECmdObjType.EdtAssignmentObj);
        }



        public async Task<Response> initializeForAssignment(int assignmentId) {
            InitForAssignmentObj cmdObj = new InitForAssignmentObj(assignmentId);

            return await doModelHttpRequest(cmdObj, ECmdObjType.initAssignmentObj);
        }



        public async Task<Response> initializeForClassRoom(string classRoomName) {
            InitForClassRoomObj cmdObj = new InitForClassRoomObj(classRoomName);

            return await doModelHttpRequest(cmdObj, ECmdObjType.initClassRoomObj);
        }



        public async Task<Response> initializeForStudent(int studentId) {
            InitForStudentObj cmdObj = new InitForStudentObj(studentId);

            return await doModelHttpRequest(cmdObj, ECmdObjType.initStudentObj);
        }



        public async  Task<Response> initializeForTeacher(int teacherId) {
            InitForTeacherObj cmdObj = new InitForTeacherObj(teacherId);

            return await doModelHttpRequest(cmdObj, ECmdObjType.initTeacherObj);
        }



        public void initializeWithTestData() {
            throw new NotImplementedException();
        }



        public async Task<Response> markAssignmentDone(int assignmentId, int studentId) {
            MarkAssignmentDoneObj cmdObj = new MarkAssignmentDoneObj(assignmentId, studentId);

            return await doModelHttpRequest(cmdObj, ECmdObjType.MrkAssignmentDnObj);
        }



        public async Task<Response> SubscribeStudentToClassRoom(int studentId, string classRoomName) {
            SubscribeStudentObj cmdObj = new SubscribeStudentObj(studentId, classRoomName);

            return await doModelHttpRequest(cmdObj, ECmdObjType.SubStudentObj);
        }



        public async Task<Response> undoMarkAssignmentDone(int assignmentId, int studentId) {
            UndoMarkAssignmentDoneObj cmdObj = new UndoMarkAssignmentDoneObj(assignmentId, studentId);

            return await doModelHttpRequest(cmdObj, ECmdObjType.UnMrkAssignmentDnObj);

        }



        public async Task<Response> unsubscribeStudent(int studentId, string classRoomName) {
            UnsubscribeStudentObj cmdObj = new UnsubscribeStudentObj(studentId, classRoomName);

            return await doModelHttpRequest(cmdObj, ECmdObjType.UnSubStudentObj);
        }


        public async Task<ServerResponse> createUser (string userName, string email, string password, EModelUserType userType){

            CreateUserObj cmdObj = new CreateUserObj(userName, email, password, userType);

            ServerResponse response = await this.bridge.makeHttPostRequest(cmdObj, ECmdObjType.CrtUserObj);

            Collection<DataItemUpdate> updates = new Collection<DataItemUpdate>();

            foreach (ModelUpdate modelUpdate in response.modelUpdates) {

                updates.Add(this.makeDataItemUpdate(modelUpdate));
            }

            this.updateItems(updates);
            //Accept and implement any changes to the model received

            return response;
        }


        public async Task<Response> editUser (int userId, string userName, string email, string password, EModelUserType userType){

            EditUserObj cmdObj = new EditUserObj(userId, userName, email, password, userType);

            return await doModelHttpRequest(cmdObj, ECmdObjType.EdtUserObj);

        }


        public async Task<Response> changeUserArchive (int userId, bool toArchive, EModelUserType userType){

            ChangeUserArchiveObj cmdObj = new ChangeUserArchiveObj(userId, toArchive, userType);

            return await doModelHttpRequest(cmdObj, ECmdObjType.ArchUserObj);
        }

        //---------------------------Fetch Methods----------------------------

        public async Task<ServerResponse> fetchDataBaseObject(FetchObject fetchObj, ECmdObjType cmdType){

            return await this.bridge.makeHttPostRequest(fetchObj, cmdType);
        }
    }

}


