//using System;
//using The_Model;
//using PaperGeneral;
//using System.Threading.Tasks;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;

//namespace Test_Rig {
//    public class CommandCentre {
//        public Dictionary<int, IExecutable> commands = new Dictionary<int, IExecutable>();
        
//        public void addCommand (IExecutable command){

//            int listing = this.commands.Count + 1;

//            this.commands.Add(listing, command);
//        }


//        public void addCommands(params IExecutable[] newCommands){

//            foreach (IExecutable command in newCommands){

//                this.addCommand(command);
//            }
//        }

//        public String renderCommands(){

//            string renderedCommands = "Select command:\n";

//            foreach(KeyValuePair<int,IExecutable> command in this.commands){

//                renderedCommands += Convert.ToString(command.Key) + ". " + command.Value.renderName() + "\n";
//            }

//            renderedCommands += "0. for all\n";

//            return renderedCommands;
//        }

//        public async Task<string> executeCommands(Collection<int> commandsWaiting){
//            Collection<IExecutable> commandsReady = new Collection<IExecutable>();

//            foreach (int commandKey in commandsWaiting) {
//                if (commandKey == 0) {

//                    return await executeAllCommands();
//                } else {
//                    commandsReady.Add(this.commands[commandKey]);
//                }
//            }
//                string responseMessages = "";

//                foreach(IExecutable commandReady in commandsReady){

//                    responseMessages += await commandReady.execute() ;
//                    responseMessages += "\n" + "\n";
//                }

//                return responseMessages;
//            }

//        public async Task<string> executeAllCommands() {
//            string responseMessages = "";

//            foreach (IExecutable commandReady in this.commands.Values) {

//                responseMessages += await commandReady.execute();
//                responseMessages += "\n" + "\n";
//            }

//            return responseMessages;
//        }



//    }




//    public interface IExecutable{

//        Task<string> execute();

//        string renderName();

  

//    }



//    public class AddClassRoomToTeacher : IExecutable{
//        IHomeWorkModel model;
//        string classRoomName;
//        int teacherId;
//        string name = "Add ClassRoom To Teacher";

//        public AddClassRoomToTeacher(
//            IHomeWorkModel model,
//            string classRoomName,
//            int teacherId
//        ){
//            this.model = model;
//            this.classRoomName = classRoomName;
//            this.teacherId = teacherId;
//        }

//        public async Task<string> execute(){

//            Response response = await this.model.addClassRoomToTeacher(this.classRoomName, this.teacherId);

//            return response.message;
//        }

//        public string renderName(){
//            return this.name;
//        }
//    }




//    public class  AddAssignmentToClassRoom : IExecutable {
//        IHomeWorkModel model;
//        string assignmentName;
//        DateTime dueDate;
//        string description;
//        bool isClosed;
//        string classRoomName;
//        int teacherId;
//        string name = "Add Assignment To ClassRoom";

//        public AddAssignmentToClassRoom(
//            IHomeWorkModel model,
//            string assignmentName,
//            DateTime dueDate,
//            string description,
//            bool isClosed,
//            string classRoomName,
//            int teacherId
//        ) {
//            this.model = model;
//            this.assignmentName = assignmentName;
//            this.dueDate = dueDate;
//            this.description = description;
//            this.isClosed = isClosed;
//            this.classRoomName = classRoomName;
//            this.teacherId = teacherId;
//        }
//        public async Task <string> execute() {
        
//            Response response = await this.model.addAssignmentToClassRoom(
//                                            this.assignmentName,
//                                            this.dueDate,
//                                            this.description,
//                                            this.isClosed,
//                                            this.classRoomName,
//                                            this.teacherId
//                                        );

//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }




//    public class  EditAssignment : IExecutable {
//        IHomeWorkModel model;
//        string assignmentName;
//        DateTime assignmentDueDate;
//        string assignmentDescription;
//        bool isClosed;
//        int assignmentId;
//        string name = "Edit Assignment";

//        public EditAssignment (
//            IHomeWorkModel model,
//            string assignmentName,
//            DateTime assignmentDueDate,
//            string assignmentDescription,
//            bool isClosed,
//            int assignmentId
//        ) {
//            this.model = model;
//            this.assignmentName = assignmentName;
//            this.assignmentDueDate = assignmentDueDate;
//            this.assignmentDescription = assignmentDescription;
//            this.isClosed = isClosed;
//            this.assignmentId = assignmentId;
//        }

//        public async Task<string> execute() { 
        
//            Response response = await  this.model.editAssignment(
//                                    this.assignmentName,
//                                    this.assignmentDueDate,
//                                    this.assignmentDescription,
//                                    this.isClosed,
//                                    this.assignmentId
//                                );

//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }



//    public class DeleteAssignment : IExecutable {
//        IHomeWorkModel model;
//        int assignmentId;
//        string name = "Delete Assignment";

//        public DeleteAssignment (IHomeWorkModel model, int assignmentId){
//            this.model = model;
//            this.assignmentId = assignmentId;
//        }

//        public async Task<string> execute() { 
        
//            Response response = await this.model.changeAssignmentArchive(this.assignmentId);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }




//    public class DeleteClassRoom : IExecutable {
//        IHomeWorkModel model;
//        string classRoomName;
//        string name = "Delete ClassRoom";

//        public DeleteClassRoom(IHomeWorkModel model, string classRoomName){
//            this.model = model;
//            this.classRoomName = classRoomName;
//        }

//        public async Task<string> execute() {

//            Response response = await this.model.changeClassRoomArchive(this.classRoomName);

//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }



//    public class MarkAssignmentDone : IExecutable{
//        IHomeWorkModel model;
//        int assignmentId;
//        int studentId;
//        string name = "Mark Assignment Done";

//        public MarkAssignmentDone(IHomeWorkModel model, int assignmentId, int studentId){

//            this.model = model;
//            this.assignmentId = assignmentId;
//            this.studentId = studentId;
//        }

//        public async Task<string> execute() {

//            Response response = await this.model.markAssignmentDone(this.assignmentId, this.studentId);

//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }



//    public class  UndoMarkAssignmentDone: IExecutable {
//        IHomeWorkModel model;
//        int assignmentId;
//        int studentId;
//        string name = "Undo Mark Assignment Done";

//        public UndoMarkAssignmentDone(IHomeWorkModel model, int assignmentId, int studentId){
//            this.model = model;
//            this.assignmentId = assignmentId;
//            this.studentId = studentId;
//        }

//        public async Task<string> execute() {

//            Response response = await this.model.undoMarkAssignmentDone(this.assignmentId, this.studentId);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }

//    public class  SubStudent: IExecutable{
//        IHomeWorkModel model;
//        int studentId;
//        string classRoomName;
//        string name = "Subscribe Student To ClassRoom";

//        public SubStudent(IHomeWorkModel model, int studentId, string classRoomName){

//            this.model = model;
//            this.studentId = studentId;
//            this.classRoomName = classRoomName;
//        }

//        public async Task <string> execute() {

//            Response response = await this.model.SubscribeStudentToClassRoom(this.studentId, this.classRoomName);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }

//    public class  UnSubStudent: IExecutable {
//        IHomeWorkModel model;
//        int studentId;
//        string classRoomName;
//        string name = "Unsubscribe Student From ClassRoom";

//        public UnSubStudent(IHomeWorkModel model, int studentId, string classRoomName){
//            this.model = model;
//            this.studentId = studentId;
//            this.classRoomName = classRoomName;
//        }

//        public async Task <string> execute() {

//            Response response = await this.model.unsubscribeStudent(this.studentId, this.classRoomName);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }

//    public class  InitTeacher: IExecutable{
//        IHomeWorkModel model;
//        int teacherId;
//        string name = "Initialize For Teacher";

//        public InitTeacher(IHomeWorkModel model, int teacherId){
//            this.model = model;
//            this.teacherId = teacherId;
//        }

//        public async Task<string> execute() {
//            Response response = await this.model.initializeForTeacher(this.teacherId);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }

//    public class InitClassRoom: IExecutable {
//        IHomeWorkModel model;
//        string classRoomName;
//        string name = "Initialize For ClassRoom";

//        public InitClassRoom(IHomeWorkModel model, string classRoomName){
//            this.model = model;
//            this.classRoomName = classRoomName;
//        }

//        public async Task <string> execute() {
//            Response response = await this.model.initializeForClassRoom(this.classRoomName);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }

//    public class  InitAssignment: IExecutable {
//        IHomeWorkModel model;
//        int assignmentId;
//        string name = "Initiazile For Assignment";

//        public InitAssignment(IHomeWorkModel model, int assignmentId){

//            this.model = model;
//            this.assignmentId = assignmentId;
//        }

//        public async Task<string> execute() {

//            Response response = await this.model.initializeForAssignment(this.assignmentId);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }

//    public class InitStudent: IExecutable{
//        IHomeWorkModel model;
//        int studentId;
//        string name = "Initialize For Student";

//        public InitStudent(IHomeWorkModel model, int studentId){

//            this.model = model;
//            this.studentId = studentId;
//        }

//        public async Task<string > execute() {
//            Response response = await this.model.initializeForStudent(this.studentId);
//            return response.message;
//        }

//        public string renderName() {
//            return this.name;
//        }
//    }




//    //async Task<string> addClassRoomToTeacher() {

//    //    Console.WriteLine("\n");
//    //    Console.WriteLine("Input classRoom name:");
//    //    string name = Console.ReadLine();
//    //    Console.WriteLine("Input teacher Id:");
//    //    string idInput = Console.ReadLine();

//    //    int id = Convert.ToInt32(idInput);

//    //    return await this.model.addClassRoomToTeacher(name, id);

//    //}

//}
