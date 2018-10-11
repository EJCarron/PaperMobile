//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Paper;
//using BodgeDB;
//using System.Threading;
//using InternetObjs;
//using PaperGeneral;
//using Newtonsoft.Json;

//namespace Model_store {

//    public delegate void Watcher(EModelUpdateType updateType);

//    public class ModelItem {
//        public string dataKey;
//        public object dataItem;
//        public bool removed = false;

//        public List<Watcher> watchers = new List<Watcher>();


//        public ModelItem(string dataKey, object dataItem) {

//            this.dataKey = dataKey;
//            this.dataItem = dataItem;
//        }

//        public void addWatcher(Watcher watcher) {

//            this.watchers.Add(watcher);

//        }



//        public void updateModelItem( DataItemUpdate update) {

//            if (update.remove){
//                this.removed = true;
//            } else {
//                this.dataItem = update.dataItem;
//            }

  

//        }

//    }

//   public class DataItemUpdate {
//        public string dataKey;
//        public object dataItem;
//        public bool remove = false;

//        public DataItemUpdate(string dataKey, object dataItem) {

//            this.dataKey = dataKey;
//            this.dataItem = dataItem;
//        }

       
//    }




//    public class DataItemUpdate_Remove : DataItemUpdate {



//        public DataItemUpdate_Remove(string dataKey):base(dataKey, null){

//            this.remove = true;
//        }
//    }



//    public enum EModelUpdateType{
//        removal = 0,
//        dataUpdate = 1
//    }



//    public class ModelStore : IModelStore {
//        private Dictionary<string, ModelItem> modelItems = new Dictionary<string, ModelItem>();

//        public void updateItems(params DataItemUpdate[] updates) {

//            this.updateItems((IList<DataItemUpdate>)updates);
//        }

//        public void updateItems(IList<DataItemUpdate> listOfUpdates,params DataItemUpdate[] updatesParam){//}) {

//            foreach (DataItemUpdate update in updatesParam) {
//                listOfUpdates.Add(update);
//            }


//            Collection<ModelItem> removedModelItems = new Collection<ModelItem>();

//            List<Watcher> watchersToAlert = new List<Watcher>();
//            List<Watcher> watchersToAlertAboutRemoval = new List<Watcher>();

//            foreach (DataItemUpdate update in listOfUpdates) {

//                ModelItem modelItemToBeUpdated = null;

//                if (update.remove) {

                     
//                    if (this.modelItems.ContainsKey(update.dataKey)) { 

//                        modelItemToBeUpdated = this.modelItems[update.dataKey];

//                        removedModelItems.Add(modelItemToBeUpdated);

//                        this.modelItems.Remove(update.dataKey); // remove from list of model items

//                        foreach (Watcher watcher in modelItemToBeUpdated.watchers) {

//                            if (!watchersToAlertAboutRemoval.Contains(watcher)) {

//                                watchersToAlertAboutRemoval.Add(watcher);
//                            }
//                        }
//                    }

//                } else {

//                    if (!this.modelItems.ContainsKey(update.dataKey)) {

//                        ModelItem new_modelItem = new ModelItem(update.dataKey, update.dataKey);

//                        this.modelItems.Add(update.dataKey, new_modelItem);

//                    }
//                    modelItemToBeUpdated = this.modelItems[update.dataKey];
//                }
//                if (modelItemToBeUpdated != null) {
//                    modelItemToBeUpdated.updateModelItem(update);
//                }

//            }

//            // get list of watchers wathcing NON removal updates.

//            foreach (DataItemUpdate update in listOfUpdates) {
//                if (!update.remove) {
//                    foreach (Watcher watcher in this.modelItems[update.dataKey].watchers) {

//                        if (!watchersToAlert.Contains(watcher)) {

//                            watchersToAlert.Add(watcher);
//                        }
//                    }
//                } 
//            }

//            // allert watchers of removals

//            foreach (Watcher watcher in watchersToAlertAboutRemoval) {
//                watcher(EModelUpdateType.removal);
//            }

//            // allert watchers of changes;

//            foreach (Watcher watcher in watchersToAlert) {

//                if (areAllWatcherTargetsLoaded(watcher)) {

//                    watcher(EModelUpdateType.dataUpdate);
//                }
//            }

//        }


//        public bool areAllWatcherTargetsLoaded(Watcher watcher) {
//            List<ModelItem> targets = new List<ModelItem>();


//            foreach (KeyValuePair<string, ModelItem> modelItem in this.modelItems) {

//                if (modelItem.Value.watchers.Contains(watcher)) {

//                    targets.Add(modelItem.Value);
//                }
//            }

//            foreach (ModelItem target in targets) {

//                if (target.dataItem == null) {

//                    return false;
//                }
//            }

//            return true;
//        }




//        public void addWatcher(Watcher watcher, params string[] dataKeys) {

//            foreach (string dataKey in dataKeys) {

//                if (!this.modelItems.ContainsKey(dataKey)) {

//                    ModelItem new_modelItem = new ModelItem(dataKey, null);

//                    this.modelItems.Add(dataKey, new_modelItem);

//                    new_modelItem.addWatcher(watcher);
//                } else {
//                    this.modelItems[dataKey].addWatcher(watcher);

//                    if (areAllWatcherTargetsLoaded(watcher)) {

//                        watcher(EModelUpdateType.dataUpdate);
//                    }

//                }
//            }



//        }

//        public void RemoveWatcher() { 
        
            
        
//        }

//        public T getItem<T>(string dataKey) {

//            if (!this.modelItems.ContainsKey(dataKey) || (this.modelItems[dataKey].dataItem == null)) {

//                return default(T);

//            } else {
//                return (T)this.modelItems[dataKey].dataItem;

//            }

//        }
//    }


//    public interface IModelStore {

//        void updateItems(params DataItemUpdate[] updates);
//        void updateItems(IList<DataItemUpdate> listOfUpdates, params DataItemUpdate[] updatesParam);
//        void addWatcher(Watcher watcher, params string[] dataKeys);
//        T getItem<T>(string dataKey);
//    }

//    public interface IHomeWorkModel : IModelStore {

//        Task<Response>  addClassRoomToTeacher(string classRoomname, int teacherId);
//        Task<Response> addAssignmentToClassRoom(string assignmentName, DateTime dueDate, string description, bool isClosed, string classRoomName,int teacherId);
//        Task<Response> editAssignment(string assignmentName, DateTime assignmentDueDate, string assignmentDescription, bool isClosed, int assignmentId);
//        Task<Response> deleteAssignment(int assignmentId);
//        Task<Response> deleteClassRoom(string classRoomName);
//        Task<Response> markAssignmentDone(int assignmentId, int studentId);
//        Task<Response> undoMarkAssignmentDone(int assignmentId, int studentId);
//        Task<Response> SubscribeStudentToClassRoom(int studentId, string classRoomName);
//        Task<Response> unsubscribeStudent(int studentId, string classRoomName);
//        void initializeWithTestData();
//        Task<Response> initializeForTeacher(int teacherId);
//        Task<Response> initializeForClassRoom(string classRoomName);
//        Task<Response> initializeForAssignment(int assignmentId);
//        Task<Response> initializeForStudent(int studentId);
//    }

//    public class HttpHomeWorkModel : ModelStore, IHomeWorkModel{
//        private HttpBridge bridge = HttpBridge.getInstance();


//        //-------------------Tool Methods-------------------------------------

//        public DataItemUpdate makeDataItemUpdate(ModelUpdate modelUpdate) {

//            EModelType modelType = (EModelType)modelUpdate.modelType;

//            switch (modelType) {

//                case EModelType.modelAssignemt:
//                    Model_Assignment assignment = JsonConvert.DeserializeObject<Model_Assignment>(modelUpdate.modelItemAsJson);
//                    return new DataItemUpdate(modelUpdate.dataKey, assignment);

//                case EModelType.modelClassRoom:
//                    Model_ClassRoom classRoom = JsonConvert.DeserializeObject<Model_ClassRoom>(modelUpdate.modelItemAsJson);
//                    return new DataItemUpdate(modelUpdate.dataKey, classRoom);

//                case EModelType.modelStudent:
//                    Model_Student student = JsonConvert.DeserializeObject<Model_Student>(modelUpdate.modelItemAsJson);
//                    return new DataItemUpdate(modelUpdate.dataKey, student);

//                case EModelType.modelTeacher:
//                    Model_Teacher teacher = JsonConvert.DeserializeObject<Model_Teacher>(modelUpdate.modelItemAsJson);
//                    return new DataItemUpdate(modelUpdate.dataKey, teacher);

//            }
//            return null;
//        }
           

//        public async Task<Response> doModelHttpRequest(ICommandObject cmdObject, ECmdObjType cmdType){

//            ServerResponse response = await this.bridge.makeHttPostRequest(cmdObject, cmdType);
//            Collection<DataItemUpdate> updates = new Collection<DataItemUpdate>();

//            foreach (ModelUpdate modelUpdate in response.modelUpdates) {

//                updates.Add(this.makeDataItemUpdate(modelUpdate));
//            }

//            this.updateItems(updates);
//            //Accept and implement any changes to the model received

//            return response.errorResponse;

//        }


//        //--------------ModelMethods-----------------------------------------
//        public async Task<Response> addAssignmentToClassRoom(string assignmentName, DateTime dueDate, string description, bool isClosed, string classRoomName, int teacherId) {

//            //Send Comand to midTier

//            CreateAssignmentObj cmdObj = new CreateAssignmentObj(
//                assignmentName,
//                dueDate,
//                description,
//                isClosed,
//                classRoomName,
//                teacherId
//            );

//            return await doModelHttpRequest(cmdObj, ECmdObjType.CrtAssignmentObj);
//        }




//        public async Task<Response> addClassRoomToTeacher(string classRoomname, int teacherId) {
//            CreateClassRoomObj cmdObj = new CreateClassRoomObj(classRoomname, teacherId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.CrtClassRoomObj);
//        }



//        public async Task<Response> deleteAssignment(int assignmentId) {
//            DeleteAssignmentObj cmdObj = new DeleteAssignmentObj(assignmentId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.DltAssignmentObj);
//        }



//        public async Task<Response> deleteClassRoom(string classRoomName) {
//            DeleteClassRoomObj cmdObj = new DeleteClassRoomObj(classRoomName);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.DltClassRoomObj);
//        }



//        public async Task<Response> editAssignment(
//            string assignmentName,
//            DateTime assignmentDueDate,
//            string assignmentDescription,
//            bool isClosed,
//            int assignmentId
//        ) {
//            EditAssignmentObj cmdObj = new EditAssignmentObj(
//                assignmentName,
//                assignmentDueDate,
//                assignmentDescription,
//                isClosed,
//                assignmentId
//            );


//            return await doModelHttpRequest(cmdObj, ECmdObjType.EdtAssignmentObj);
//        }



//        public async Task<Response> initializeForAssignment(int assignmentId) {
//            InitForAssignmentObj cmdObj = new InitForAssignmentObj(assignmentId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.initAssignmentObj);
//        }



//        public async Task<Response> initializeForClassRoom(string classRoomName) {
//            InitForClassRoomObj cmdObj = new InitForClassRoomObj(classRoomName);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.initClassRoomObj);
//        }



//        public async Task<Response> initializeForStudent(int studentId) {
//            InitForStudentObj cmdObj = new InitForStudentObj(studentId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.initStudentObj);
//        }



//        public async Task<Response> initializeForTeacher(int teacherId) {
//            InitForTeacherObj cmdObj = new InitForTeacherObj(teacherId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.initTeacherObj);
//        }



//        public void initializeWithTestData() {
//            throw new NotImplementedException();
//        }



//        public async Task<Response> markAssignmentDone(int assignmentId, int studentId) {
//            MarkAssignmentDoneObj cmdObj = new MarkAssignmentDoneObj(assignmentId, studentId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.MrkAssignmentDnObj); 
//        }



//        public async Task<Response> SubscribeStudentToClassRoom(int studentId, string classRoomName) {
//            SubscribeStudentObj cmdObj = new SubscribeStudentObj(studentId, classRoomName);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.SubStudentObj);
//        }



//        public async Task<Response> undoMarkAssignmentDone(int assignmentId, int studentId) {
//            UndoMarkAssignmentDoneObj cmdObj = new UndoMarkAssignmentDoneObj(assignmentId, studentId);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.UnMrkAssignmentDnObj);

//        }



//        public async Task<Response> unsubscribeStudent(int studentId, string classRoomName) {
//            UnsubscribeStudentObj cmdObj = new UnsubscribeStudentObj(studentId, classRoomName);

//            return await doModelHttpRequest(cmdObj, ECmdObjType.UnSubStudentObj);
//        }
//    }







//    public class BodgeHomeWorkModel : ModelStore {
//        private DataBase dataBase = new DataBase();
//        private HttpBridge bridge = HttpBridge.getInstance();

//        public void initializeWithTestData() {
//            string error = "";

//            User student = dataBase.makeUser("John");
//            User teacher = dataBase.makeUser("Smith");

//            ClassRoom classRoom1 = dataBase.makeClassRoom("Hist8", ref error);
//            ClassRoom classRoom2 = dataBase.makeClassRoom("Geo7", ref error);

//            Assignment assignment1 = dataBase.makeAssignment("Essay", new DateTime(2017, 8, 1), "", true);
//            Assignment assignment2 = dataBase.makeAssignment("Worksheet", new DateTime(2017, 10, 2), "", false);
//            Assignment assignment3 = dataBase.makeAssignment("Presentation", new DateTime(2017, 10, 3), "", true);
//            Assignment assignment4 = dataBase.makeAssignment("Poster", new DateTime(2017, 10, 4), "", false);
//            Assignment assignment5 = dataBase.makeAssignment("Test", DateTime.Now, "studdddaas", false);
//            Assignment assignment6 = dataBase.makeAssignment("Test1", new DateTime(2017, 1, 11), "studdddaas", false);
//            Assignment assignment7 = dataBase.makeAssignment("Test1", new DateTime(2017, 10, 11), "studdddaas", false);
//            Assignment assignment8 = dataBase.makeAssignment("Test1", new DateTime(2017, 9, 30), "studdddaas", false);

//            assignment1.description = "Write 200 words on the war of the Roses and it's implications.";

//            classRoom1.assignments.Add(assignment1.id, assignment1);
//            classRoom2.assignments.Add(assignment2.id, assignment2);
//            classRoom1.assignments.Add(assignment3.id, assignment3);
//            classRoom2.assignments.Add(assignment4.id, assignment4);
//            classRoom1.assignments.Add(assignment5.id, assignment5);
//            classRoom1.assignments.Add(assignment6.id, assignment6);
//            classRoom1.assignments.Add(assignment7.id, assignment7);
//            classRoom1.assignments.Add(assignment8.id, assignment8);

//            teacher.classRooms.Add(classRoom1.name, classRoom1);

//            teacher.classRooms.Add(classRoom2.name, classRoom2);

//            this.dataBase.users.Add(student.id, student);
//            this.dataBase.users.Add(teacher.id, teacher);


//        }

//        private ClassRoom findClassRoomByName(string classRoomName) {
//            foreach (User user in this.dataBase.users.Values) {
//                foreach (ClassRoom classRoom in user.classRooms.Values) {

//                    if (classRoom.name == classRoomName) {

//                        return classRoom;
//                    }
//                }

//            }

//            return null;
//        }

//        private DataItemUpdate makeStudentUpdate(int studentId) {
//            User dbStudent = this.dataBase.users[studentId];

//            Model_Student student = new Model_Student(dbStudent.name, studentId);

//            foreach (Subscription subscription in this.dataBase.subscriptions) {

//                if (subscription.studentId == dbStudent.id) {

//                    ClassRoom dbClassRoom = findClassRoomByName(subscription.classRoomName);

//                    student.subscriptions.Add(new Students_SubscriptionListItem(dbClassRoom.name, dbClassRoom.getNextDueDate()));

//                    foreach (Assignment assignment in dbClassRoom.assignments.Values) {


//                        string assignmentParent = dbClassRoom.name;

//                        if (assignment.completions.ContainsKey(studentId)) {

//                            Students_AssignmentListItem sal = new Students_AssignmentListItem(
//                                assignment.name,
//                                assignment.dueDate,
//                                assignment.id,
//                                assignmentParent,
//                                true
//                            );
//                            student.assignments.Add(sal);

//                        } else {
//                            Students_AssignmentListItem sal = new Students_AssignmentListItem(
//                                assignment.name,
//                                assignment.dueDate,
//                                assignment.id,
//                                assignmentParent,
//                                false
//                            );
//                            student.assignments.Add(sal);
//                        }


//                    }
//                }
//            }


//            return new DataItemUpdate(DataKeys.getStudentKey(studentId), student);

//        }

//        private DataItemUpdate makeTeacherUpdate(int teacherId) {
//            User dbTeacher = this.dataBase.users[teacherId];

//            Model_Teacher teacher = new Model_Teacher(dbTeacher.name, teacherId);

//            foreach (ClassRoom classRoom in dbTeacher.classRooms.Values) {

//                Teachers_ClasroomListItem tcl = new Teachers_ClasroomListItem(classRoom.name, classRoom.getNextDueDate());

//                teacher.classRooms.Add(tcl);
//            }

//            return new DataItemUpdate(DataKeys.getTeacherKey(teacherId), teacher);
//        }




//        private DataItemUpdate makeClassRoomUpdate(string classRoomName) {
//            ClassRoom dbClassRoom = new ClassRoom("");

//            foreach (User user in dataBase.users.Values) {

//                if (user.classRooms.ContainsKey(classRoomName)) {
//                    dbClassRoom = user.classRooms[classRoomName];
//                }
//            }

//            Model_ClassRoom classRoom = new Model_ClassRoom(classRoomName);

//            foreach (Assignment assignment in dbClassRoom.assignments.Values) {

//                ClassRooms_AssignmentListItem cal = new ClassRooms_AssignmentListItem(assignment.name, assignment.dueDate, assignment.isClosed, assignment.id);

//                classRoom.assignments.Add(cal);

//            }

//            return new DataItemUpdate(DataKeys.getClassRoomKey(classRoomName), classRoom);
//        }



//        private DataItemUpdate makeAssignmentUpdate(int assignmentId) {
//            Assignment dbAssignment = new Assignment("", new DateTime(1, 1, 1), "", false, 0);

//            foreach (User user in dataBase.users.Values) {

//                foreach (ClassRoom classRoom in user.classRooms.Values) {

//                    if (classRoom.assignments.ContainsKey(assignmentId)) {

//                        dbAssignment = classRoom.assignments[assignmentId];
//                    }
//                }
//            }


//            Model_Assignment assignment = new Model_Assignment(dbAssignment.name, dbAssignment.dueDate, dbAssignment.description, dbAssignment.isClosed);

//            return new DataItemUpdate(DataKeys.getAssignmentKey(assignmentId), assignment);
//        }

//        private DataItemUpdate_Remove makeRemoveAssignmentUpdate(int assignmentId) {

//            return new DataItemUpdate_Remove(DataKeys.getAssignmentKey(assignmentId));
//        }

//        private Collection<DataItemUpdate> makeRemoveClassRoomUpdate(string classRoomName) {
//            Collection<DataItemUpdate> removals = new Collection<DataItemUpdate>();

//            removals.Add(new DataItemUpdate_Remove(DataKeys.getClassRoomKey(classRoomName)));

//            ClassRoom dbClassRoom = getClassRoomFromName(classRoomName);

//            foreach (Assignment assignment in dbClassRoom.assignments.Values) {

//                removals.Add(new DataItemUpdate_Remove(DataKeys.getAssignmentKey(assignment.id)));
//            }
//            return removals;
//        }

//        public void initializeForTeacher(int teacherId) {

//            DataItemUpdate teacherUpdate = this.makeTeacherUpdate(teacherId);

//            this.updateItems(
//                teacherUpdate
//            );

//        }

//        public void initializeForStudent(int studentId) {

//            this.updateItems(
//                makeStudentUpdate(studentId)
//            );
//        }

//        public void initializeForClassRoom(string classRoomName) {

//            DataItemUpdate classRoomUpdate = this.makeClassRoomUpdate(classRoomName);

//            this.updateItems(classRoomUpdate);

//        }

//        public void initializeForAssignment(int assignmentId) {

//            DataItemUpdate assignmentUpdate = this.makeAssignmentUpdate(assignmentId);

//            this.updateItems(assignmentUpdate);
//        }

//        public async Task<Response> addClassRoomToTeacher(
//            string classRoomname,
//            int teacherId
//        ) {
//            await Task.Delay(1000);

//            string error = "";
//            // change the database

//            User teacher = dataBase.users[teacherId];

//            teacher.classRooms.Add(classRoomname, dataBase.makeClassRoom(classRoomname, ref error));

//            // update the model to reflect changes in the database

//            Model_ClassRoom newClassRoom = new Model_ClassRoom(classRoomname);

//            this.updateItems(
//                this.makeTeacherUpdate(teacherId),
//                new DataItemUpdate(DataKeys.getClassRoomKey(classRoomname), newClassRoom)
//            );


//            return new Response();
//        }

//        public async Task<Response> addAssignmentToClassRoom(
//            string assignmentName, 
//            DateTime dueDate, 
//            string description, 
//            bool isClosed, 
//            string classRoomName, 
//            int teacherId
//        ) {
//            await Task.Delay(1000);

//            Assignment newAssignment = dataBase.makeAssignment(assignmentName, dueDate, description, isClosed);
//            dataBase.users[teacherId].classRooms[classRoomName].assignments.Add(newAssignment.id, newAssignment);

//            this.updateItems(
//                this.makeClassRoomUpdate(classRoomName),
//                this.makeTeacherUpdate(teacherId)
//                                );


//            return new Response();
//        }

//        public async Task<Response> editAssignment(
//            string assignmentName,
//            DateTime assignmentDueDate,
//            string assignmentDescription,
//            bool isClosed,
//            int assignmentId
//        ) {
//            await Task.Delay(1000);

//            //Fetching database objects
//            ClassRoom classRoom = getAssignmentParent(assignmentId);
//            User teacher = getClassRoomParent(classRoom.name);
//            Assignment assignmentToEdit = classRoom.assignments[assignmentId];

//            //changing database
//            assignmentToEdit.name = assignmentName;
//            assignmentToEdit.dueDate = assignmentDueDate;
//            assignmentToEdit.description = assignmentDescription;
//            assignmentToEdit.isClosed = isClosed;

//            //update the model items.
//            this.updateItems(
//                this.makeAssignmentUpdate(assignmentId),
//                this.makeClassRoomUpdate(classRoom.name),
//                this.makeTeacherUpdate(teacher.id)
//                                );

//            return new Response();

//        }

//        public async Task<Response> deleteAssignment(
//            int assignmentId
//        ) {
//            await Task.Delay(1000);
//            ClassRoom classRoom = getAssignmentParent(assignmentId);
//            User teacher = getClassRoomParent(classRoom.name);
//            Assignment assignmentToRemove = classRoom.assignments[assignmentId];

//            classRoom.assignments.Remove(assignmentId);

//            this.updateItems(
//                makeRemoveAssignmentUpdate(assignmentId),
//                makeClassRoomUpdate(classRoom.name),
//                makeTeacherUpdate(teacher.id)
//            );

//            return new Response();
//        }

//        public async Task<Response> deleteClassRoom(string classRoomName) {
//            await Task.Delay(1000);
//            User teacher = getClassRoomParent(classRoomName);


//            //Make updates

//            Collection<DataItemUpdate> classRoomAndAssignmentUpdates = makeRemoveClassRoomUpdate(classRoomName);


//            //Remove classRoom from dataBase
//            teacher.classRooms.Remove(classRoomName);

//            //Deploy updates
//            this.updateItems(
//                classRoomAndAssignmentUpdates,

//                makeTeacherUpdate(teacher.id)
//            );
//            return new Response();
//        }


//        public async Task<Response> markAssignmentDone(
//            int assignmentId, 
//            int studentId
//        ) {
//            await Task.Delay(1000);

//            //Add NewCompletion to Assignment
//            ClassRoom classRoom = getAssignmentParent(assignmentId);

//            classRoom.assignments[assignmentId].addNewCompletion(assignmentId, studentId, new DateTime());

//            //Deploy Updates

//            this.updateItems(makeStudentUpdate(studentId), makeAssignmentUpdate(assignmentId));

//            return new Response();
//        }

//        public async Task<Response> undoMarkAssignmentDone(
//            int assignmentId, 
//            int studentId
//        ) {
//            await Task.Delay(1000);
//            //Remove completion
//            ClassRoom classRoom = getAssignmentParent(assignmentId);

//            classRoom.assignments[assignmentId].completions.Remove(studentId);


//            //Deploy Updates

//            this.updateItems(makeStudentUpdate(studentId), makeAssignmentUpdate(assignmentId));

//            return new Response();
//        }


//        class Test{
//            public string test = "";
//        }


//        public async Task<Response> SubscribeStudentToClassRoom(
//            int studentId,
//            string classRoomName
//        ) {

//            SubscribeStudentObj cmdObj = new SubscribeStudentObj(studentId, "Hist9");

//            ServerResponse serverResponse = await this.bridge.makeHttPostRequest(cmdObj, ECmdObjType.SubStudentObj);

//            int i = 1;

//            return new Response();
        
        
//        }



//        //public async Task<Response> SubscribeStudentToClassRoom(
//        //    int studentId, 
//        //    string classRoomName
//        //) {
//        //    await Task.Delay(1000);

//        //    //var command = new CreateAssignmentObj("testAssignmet", new DateTime(1, 1, 1), "", true, "testClassRoom", 1);



//        //    // return await bridge.makeHttPostRequest("test");

//        //    //Add Subscription to the Database

//        //    Subscription newSubscription = new Subscription(studentId, classRoomName);

//        //    if (this.dataBase.subscriptions.Contains(newSubscription)) {

//        //        return new Response(ResponseState.error, "Student already subscribed to ClassRoom");
//        //    } else if (!doesClassRoomExist(classRoomName)) {

//        //        return new Response(ResponseState.error, "ClassRoom does not exist");
//        //    } else {

//        //        this.dataBase.subscriptions.Add(newSubscription);
//        //    }

//        //    //Update Model


//        //    this.updateItems(makeStudentUpdate(studentId));



//        //    //return new Response(ResponseState.ok, "Student now subscribed");
//        //}


//        public async Task<Response> unsubscribeStudent(
//            int studentId, 
//            string classRoomName
//        ) {
//            await Task.Delay(1000);

//            //Remove subscription

//            Subscription subToRemove = new Subscription(0, null);

//            foreach (Subscription sub in this.dataBase.subscriptions) {

//                if (sub.studentId == studentId && sub.classRoomName == classRoomName) {

//                    subToRemove = sub;
//                    break;
//                }
//            }

//            this.dataBase.subscriptions.Remove(subToRemove);

//            //Update Model

//            this.updateItems(makeStudentUpdate(studentId));

//            return new Response();
//        }

//        //++++++++++++++++MODEL TOOLS++++++++++++++++++++++++++++++++++

//        public bool doesClassRoomExist(string classRoomName) {

//            foreach (User user in this.dataBase.users.Values) {

//                foreach (ClassRoom classRoom in user.classRooms.Values) {

//                    if (classRoom.name == classRoomName) {

//                        return true;
//                    }
//                }
//            }
//            return false;
//        }

//        public ClassRoom getAssignmentParent(int assignmentId) {

//            foreach (User teacher in dataBase.users.Values) {

//                foreach (ClassRoom classRoom in teacher.classRooms.Values) {

//                    if (classRoom.assignments.ContainsKey(assignmentId)) {

//                        return classRoom;
//                    }
//                }
//            }
//            return null;
//        }

//        public User getClassRoomParent(string classRoomName) {

//            foreach (User teacher in dataBase.users.Values) {

//                if (teacher.classRooms.ContainsKey(classRoomName)) {

//                    return teacher;
//                }
//            }
//            return null;
//        }

//        public User getAssignmentTeacher(int assignmentId) {

//            ClassRoom classRoom = getAssignmentParent(assignmentId);

//            return getClassRoomParent(classRoom.name);

//        }

//        public ClassRoom getClassRoomFromName(string classRoomName) {

//            User teacher = getClassRoomParent(classRoomName);

//            return teacher.classRooms[classRoomName];

//        }

//    }

//    //public enum ResponseState {
//    //    ok = 0,
//    //    error = 1,
//    //    bug = 2,

//    //}

//    //public enum SubResponseType{
//    //    none = 0,
//    //}

//    //public class Response {
//    //    public ResponseState state = ResponseState.ok;
//    //    SubResponseType subState = SubResponseType.none;
//    //    public string message = "";

//    //    public Response(){
            
//    //    }

//    //    public Response(ResponseState state, string message){

//    //        this.state = state;
//    //        this.message = message;
//    //    }

//    //    public Response(ResponseState state, SubResponseType subState, string message) {

//    //        this.state = state;
//    //        this.subState = subState;
//    //        this.message = message;
//    //    }

//    //    public bool isOk {

//    //        get { return this.state == ResponseState.ok; }

//    //    }
//    //}

//    static class general {

//        public static ModelStore myModelStore = new ModelStore();

//    }


//}
    
    