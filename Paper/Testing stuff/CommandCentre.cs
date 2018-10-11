using System;
using The_Model;
using PaperGeneral;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Paper {


    public class CommandCentre {
        public Dictionary<string, IExecutable> commands = new Dictionary<string, IExecutable>();

        public void addCommand(string text ,IExecutable command) {

            this.commands.Add(text, command);
        }


        public String renderCommands() {

            string renderedCommands = "Select command:\n";

            foreach (KeyValuePair<string, IExecutable> command in this.commands) {

                renderedCommands += Convert.ToString(command.Key) + ". " + command.Value.renderName() + "\n";
            }

            renderedCommands += "0. for all\n";

            return renderedCommands;
        }

        public async Task<string> executeCommands(Collection<string> commandsWaiting) {
            Collection<IExecutable> commandsReady = new Collection<IExecutable>();

            foreach (string commandKey in commandsWaiting) {

                commandsReady.Add(this.commands[commandKey]);
            }
            
            string responseMessages = "";

            foreach (IExecutable commandReady in commandsReady) {

                responseMessages += await commandReady.execute();
                responseMessages += "\n" + "\n";
            }

            return responseMessages;
        }



        public async Task<string> executeAllCommands() {
            string responseMessages = "";

            foreach (IExecutable commandReady in this.commands.Values) {

                responseMessages += await commandReady.execute();
                responseMessages += "\n" + "\n";
            }

            return responseMessages;
        }



    }




    public interface IExecutable {

        Task<string> execute();

        string renderName();



    }



    public class AddClassRoomToTeacher : IExecutable {
        IHomeWorkModel model;
        string classRoomName;
        int teacherId;
        string name = "Add ClassRoom To Teacher";

        public AddClassRoomToTeacher(
            IHomeWorkModel model,
            string classRoomName,
            int teacherId
        ) {
            this.model = model;
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {

            Response response = await this.model.addClassRoomToTeacher(this.classRoomName, this.teacherId);

            if (!response.isOk) {

                return response.message;
            }

            ClassRoomFetchObject CFO = new ClassRoomFetchObject(this.teacherId, this.classRoomName);

            ServerResponse serverResponse = await this.model.fetchDataBaseObject(CFO, ECmdObjType.classRoomFetchObj);

            if (!serverResponse.errorResponse.isOk){

                return serverResponse.errorResponse.message;
            }

            if(CFO.isSame(serverResponse.fetchObject)){

                return "Success! \n" + CFO.render();
            }else{
                return "Failure :(";
            }



        }

        public string renderName() {
            return this.name;
        }
    }




    public class AddAssignmentToClassRoom : IExecutable {
        IHomeWorkModel model;
        string assignmentName;
        DateTime dueDate;
        string description;
        bool isClosed;
        string classRoomName;
        int teacherId;
        string name = "Add Assignment To ClassRoom";

        public AddAssignmentToClassRoom(
            IHomeWorkModel model,
            string assignmentName,
            DateTime dueDate,
            string description,
            bool isClosed,
            string classRoomName,
            int teacherId
        ) {
            this.model = model;
            this.assignmentName = assignmentName;
            this.dueDate = dueDate;
            this.description = description;
            this.isClosed = isClosed;
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }
        public async Task<string> execute() {

            Response response = await this.model.addAssignmentToClassRoom(
                                            this.assignmentName,
                                            this.dueDate,
                                            this.description,
                                            this.isClosed,
                                            this.classRoomName,
                                            this.teacherId
                                        );
            if(!response.isOk){

                return response.message;
            }

            ModelUser teacher = this.model.getItem<ModelUser>(DataKeys.getUserKey(this.teacherId));

            ModelAssignment assignment = null;

            foreach(ModelAssignment ass in teacher.assignments){

                if(ass.name == this.assignmentName){
                    assignment = ass;
                }
            }

            if(assignment == null){
                return "Failure, assignment not on model";
            }

            AssignmentFetchObject aFO = new AssignmentFetchObject(
                assignment.id,
                this.classRoomName,
                this.dueDate,
                this.description,
                this.isClosed,
                this.assignmentName
            );

            ServerResponse serverResponse = await this.model.fetchDataBaseObject(aFO, ECmdObjType.assignmentFetchObj);

            if (!serverResponse.errorResponse.isOk) {

                return serverResponse.errorResponse.message;
            }

            if (aFO.isSame(serverResponse.fetchObject)) {

                return "Success! \n" + aFO.render();
            } else {
                return "Failure :(";
            }

        }

        public string renderName() {
            return this.name;
        }
    }




    public class EditAssignment : IExecutable {
        IHomeWorkModel model;
        string assignmentName;
        DateTime assignmentDueDate;
        string assignmentDescription;
        bool isClosed;
        int assignmentId;
        int teacherId;
        string name = "Edit Assignment";

        public EditAssignment(
            IHomeWorkModel model,
            string assignmentName,
            DateTime assignmentDueDate,
            string assignmentDescription,
            bool isClosed,
            int assignmentId,
            int teacherId
        ) {
            this.model = model;
            this.assignmentName = assignmentName;
            this.assignmentDueDate = assignmentDueDate;
            this.assignmentDescription = assignmentDescription;
            this.isClosed = isClosed;
            this.assignmentId = assignmentId;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {

            Response response = await this.model.editAssignment(
                                    this.assignmentName,
                                    this.assignmentDueDate,
                                    this.assignmentDescription,
                                    this.isClosed,
                                    this.assignmentId, 
                                    this.teacherId

                                );

            if(!response.isOk){
                return response.message;
            }

            ModelUser teacher = this.model.getItem<ModelUser>(DataKeys.getUserKey(this.teacherId));

            ModelAssignment assignment = null;

            foreach (ModelAssignment ass in teacher.assignments) {

                if (ass.id == this.assignmentId) {
                    assignment = ass;
                }
            }

            if (assignment == null) {
                return "Failure, assignment not on model";
            }

            AssignmentFetchObject aFO = new AssignmentFetchObject(this.assignmentId,
                                                                  assignment.classRoomName,
                                                                  this.assignmentDueDate,
                                                                  this.assignmentDescription,
                                                                  this.isClosed,
                                                                  this.assignmentName
                                                                 );

            ServerResponse serverResponse = await this.model.fetchDataBaseObject(aFO, ECmdObjType.assignmentFetchObj);

            if (!serverResponse.errorResponse.isOk) {

                return serverResponse.errorResponse.message;
            }

            if (aFO.isSame(serverResponse.fetchObject)) {

                return "Success! \n" + aFO.render();
            } else {
                return "Failure :(";
            }



        }

        public string renderName() {
            return this.name;
        }
    }



    public class ArchiveAssignment : IExecutable {
        IHomeWorkModel model;
        int assignmentId;
        int teacherId;
        string name = "Archive Assignment";

        public ArchiveAssignment(IHomeWorkModel model, int assignmentId, int teacherId) {
            this.model = model;
            this.assignmentId = assignmentId;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {

            Response response = await this.model.changeAssignmentArchive(this.assignmentId, true, this.teacherId);

            if (!response.isOk) {
                return response.message;
            } else { return "Success"; }


        }

        public string renderName() {
            return this.name;
        }
    }

    public class UnArchiveAssignment : IExecutable {
        IHomeWorkModel model;
        int assignmentId;
        int teacherId;
        string name = "UnArchive Assignment";

        public UnArchiveAssignment(IHomeWorkModel model, int assignmentId, int teacherId) {
            this.model = model;
            this.assignmentId = assignmentId;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {

            Response response = await this.model.changeAssignmentArchive(this.assignmentId, false, this.teacherId);
            if (!response.isOk) {
                return response.message;
            } else { return "Success"; }
        }

        public string renderName() {
            return this.name;
        }
    }


    public class ArchiveClassRoom : IExecutable {
        IHomeWorkModel model;
        string classRoomName;
        int teacherId;
        string name = "Archive ClassRoom";

        public ArchiveClassRoom(IHomeWorkModel model, string classRoomName, int teacherId) {
            this.model = model;
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {

            Response response = await this.model.changeClassRoomArchive(this.classRoomName, true, this.teacherId);

            if (!response.isOk) {
                return response.message;
            } else { return "Success"; }
        }

        public string renderName() {
            return this.name;
        }
    }

    public class UnArchiveClassRoom: IExecutable {
        IHomeWorkModel model;
        string classRoomName;
        int teacherId;
        string name = "UnArchive ClassRoom";

        public UnArchiveClassRoom(IHomeWorkModel model, string classRoomName, int teacherId){
            this.model = model;
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }

        public async Task<string> execute(){

            Response response = await this.model.changeClassRoomArchive(this.classRoomName, false, this.teacherId);

            if (!response.isOk) {
                return response.message;
            } else { return "Success"; }
        }

        public string renderName(){
            return this.name;
        }

    }

    public class MarkAssignmentDone : IExecutable {
        IHomeWorkModel model;
        int assignmentId;
        int studentId;
        string name = "Mark Assignment Done";

        public MarkAssignmentDone(IHomeWorkModel model, int assignmentId, int studentId) {

            this.model = model;
            this.assignmentId = assignmentId;
            this.studentId = studentId;
        }

        public async Task<string> execute() {

            Response response = await this.model.markAssignmentDone(this.assignmentId, this.studentId);


            if (!response.isOk) {
                return response.message;
            }


            return "Success!!";

            //ModelUser student = this.model.getItem<ModelUser>(DataKeys.getUserKey(this.studentId));

            //ModelAssignment assignment = null;

            //foreach (ModelAssignment ass in student.assignments) {

            //    if (ass.id == this.assignmentId) {
            //        assignment = ass;
            //    }
            //}

            //if(assignment == null){
            //    return "Failure assignment not on model";
            //}

            //MarkedDoneFetchObject mDFO = new MarkedDoneFetchObject(this.assignmentId, this.studentId);

            //ServerResponse serverResponse = await this.model.fetchDataBaseObject(mDFO, ECmdObjType.markedDoneFetchObj);

            //if (!serverResponse.errorResponse.isOk) {

            //    return serverResponse.errorResponse.message;
            //}

            //if (mDFO.isSame(serverResponse.fetchObject)) {

            //    return "Success! \n" + mDFO.render();
            //} else {
            //    return "Failure :(";
            //}
        }

        public string renderName() {
            return this.name;
        }
    }



    public class UndoMarkAssignmentDone : IExecutable {
        IHomeWorkModel model;
        int assignmentId;
        int studentId;
        string name = "Undo Mark Assignment Done";

        public UndoMarkAssignmentDone(IHomeWorkModel model, int assignmentId, int studentId) {
            this.model = model;
            this.assignmentId = assignmentId;
            this.studentId = studentId;
        }

        public async Task<string> execute() {

            Response response = await this.model.undoMarkAssignmentDone(this.assignmentId, this.studentId);


            if (!response.isOk) {
                return response.message;
            }


            return "Success!";

            //ModelUser student = this.model.getItem<ModelUser>(DataKeys.getUserKey(this.studentId));

            //ModelAssignment assignment = null;

            //foreach (ModelAssignment ass in student.assignments) {

            //    if (ass.id == this.assignmentId) {
            //        assignment = ass;
            //    }
            //}

            //if(assignment == null){
            //    return "Failure assignment not on model";
            //}

            //MarkedDoneFetchObject mDFO = new MarkedDoneFetchObject(this.assignmentId, this.studentId);

            //ServerResponse serverResponse = await this.model.fetchDataBaseObject(mDFO, ECmdObjType.markedDoneFetchObj);

            //if (!serverResponse.errorResponse.isOk) {

            //    return serverResponse.errorResponse.message;
            //}

            //if (mDFO.isSame(serverResponse.fetchObject)) {

            //    return "Success! \n" + mDFO.render();
            //} else {
            //    return "Failure :(";
            //}
        }

        public string renderName() {
            return this.name;
        }
    }

    public class SubStudent : IExecutable {
        IHomeWorkModel model;
        int studentId;
        string classRoomName;
        string name = "Subscribe Student To ClassRoom";

        public SubStudent(IHomeWorkModel model, int studentId, string classRoomName) {

            this.model = model;
            this.studentId = studentId;
            this.classRoomName = classRoomName;
        }

        public async Task<string> execute() {

            Response response = await this.model.SubscribeStudentToClassRoom(this.studentId, this.classRoomName);
            if (!response.isOk) {
                return response.message;
            }

            return "Success!";

            //SubscriptionFetchObject sFO = new SubscriptionFetchObject(this.studentId, this.classRoomName);

            //ServerResponse serverResponse = await this.model.fetchDataBaseObject(sFO, ECmdObjType.subFetchObj);

            //if (!serverResponse.errorResponse.isOk) {

            //    return serverResponse.errorResponse.message;
            //}

            //if (sFO.isSame(serverResponse.fetchObject)) {

            //    return "Success! \n" + sFO.render();
            //} else {
            //    return "Failure :(";
            //}

        }

        public string renderName() {
            return this.name;
        }
    }

    public class UnSubStudent : IExecutable {
        IHomeWorkModel model;
        int studentId;
        string classRoomName;
        string name = "Unsubscribe Student From ClassRoom";

        public UnSubStudent(IHomeWorkModel model, int studentId, string classRoomName) {
            this.model = model;
            this.studentId = studentId;
            this.classRoomName = classRoomName;
        }

        public async Task<string> execute() {

            Response response = await this.model.unsubscribeStudent(this.studentId, this.classRoomName);
            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }


    public class CreateTeacher : IExecutable {
        IHomeWorkModel model;
        string userName;
        string email;
        string password;
        string name = "Create Teacher";


        public CreateTeacher(IHomeWorkModel model,string userName, string email, string password){
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.model = model;
        }

        public async Task<string> execute() {

            ServerResponse response = await this.model.createUser(this.userName, this.email, this.password, EModelUserType.teacher);

            if (!response.errorResponse.isOk) {
                return response.errorResponse.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }

    public class CreateStudent : IExecutable {
        IHomeWorkModel model;
        string userName;
        string email;
        string password;
        string name = "Create Student";


        public CreateStudent(IHomeWorkModel model, string userName, string email, string password) {
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.model = model;
        }

        public async Task<string> execute() {

            ServerResponse response = await this.model.createUser(this.userName, this.email, this.password, EModelUserType.student);

            if (!response.errorResponse.isOk) {
                return response.errorResponse.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }

    public class EditTeacher : IExecutable {
        IHomeWorkModel model;
        int teacherId;
        string userName;
        string email;
        string password;
        string name = "Edit Teacher";

        public EditTeacher (IHomeWorkModel model, int teacherId, string userName, string email, string password) {
            this.teacherId = teacherId;
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.model = model;
        }


        public async Task<string> execute() {
            Response response = await this.model.editUser(this.teacherId, this.userName, this.email, this.password, EModelUserType.teacher);

            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }

    public class EditStudent : IExecutable {
        IHomeWorkModel model;
        int studentId;
        string userName;
        string email;
        string password;
        string name = "Edit Student";

        public EditStudent(IHomeWorkModel model, int studentId, string userName, string email, string password) {
            this.studentId = studentId;
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.model = model;
        }


        public async Task<string> execute() {
            Response response = await this.model.editUser(this.studentId, this.userName, this.email, this.password, EModelUserType.student);

            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }


    public class ArchiveTeacher : IExecutable {
        IHomeWorkModel model;
        int teacherId;
        string name = "Archive Teacher";

        public ArchiveTeacher(IHomeWorkModel model, int teacherId){
            this.model = model;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {
            Response response = await this.model.changeUserArchive(this.teacherId, true, EModelUserType.teacher);

            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }


    public class ArchiveStudent : IExecutable {
        IHomeWorkModel model;
        int studentId;
        string name = "Archive Student";

        public ArchiveStudent(IHomeWorkModel model, int studentId) {
            this.model = model;
            this.studentId = studentId;
        }

        public async Task<string> execute() {
            Response response = await this.model.changeUserArchive(this.studentId, true, EModelUserType.student);

            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }


    public class UnArchiveTeacher : IExecutable {
        IHomeWorkModel model;
        int teacherId;
        string name = "UnArchive Teacher";

        public UnArchiveTeacher(IHomeWorkModel model, int teacherId) {
            this.model = model;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {
            Response response = await this.model.changeUserArchive(this.teacherId, false, EModelUserType.teacher);

            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }


    public class UnArchiveStudent : IExecutable {
        IHomeWorkModel model;
        int studentId;
        string name = "UnArchive Student";

        public UnArchiveStudent(IHomeWorkModel model, int studentId) {
            this.model = model;
            this.studentId = studentId;
        }

        public async Task<string> execute() {
            Response response = await this.model.changeUserArchive(this.studentId, false, EModelUserType.student);

            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }

    public class InitTeacher : IExecutable {
        IHomeWorkModel model;
        int teacherId;
        string name = "Initialize For Teacher";

        public InitTeacher(IHomeWorkModel model, int teacherId) {
            this.model = model;
            this.teacherId = teacherId;
        }

        public async Task<string> execute() {
            Response response = await this.model.initializeForTeacher(this.teacherId);
            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }

    public class InitClassRoom : IExecutable {
        IHomeWorkModel model;
        string classRoomName;
        string name = "Initialize For ClassRoom";

        public InitClassRoom(IHomeWorkModel model, string classRoomName) {
            this.model = model;
            this.classRoomName = classRoomName;
        }

        public async Task<string> execute() {
            Response response = await this.model.initializeForClassRoom(this.classRoomName);
            return response.message;
        }

        public string renderName() {
            return this.name;
        }
    }

    public class InitAssignment : IExecutable {
        IHomeWorkModel model;
        int assignmentId;
        string name = "Initiazile For Assignment";

        public InitAssignment(IHomeWorkModel model, int assignmentId) {

            this.model = model;
            this.assignmentId = assignmentId;
        }

        public async Task<string> execute() {

            Response response = await this.model.initializeForAssignment(this.assignmentId);
            return response.message;
        }

        public string renderName() {
            return this.name;
        }
    }

    public class InitStudent : IExecutable {
        IHomeWorkModel model;
        int studentId;
        string name = "Initialize For Student";

        public InitStudent(IHomeWorkModel model, int studentId) {

            this.model = model;
            this.studentId = studentId;
        }

        public async Task<string> execute() {
            Response response = await this.model.initializeForStudent(this.studentId);
            if (!response.isOk) {
                return response.message;
            }

            return "Success!";
        }

        public string renderName() {
            return this.name;
        }
    }

}
