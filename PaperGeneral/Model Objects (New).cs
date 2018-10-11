using System;
using System.Collections.ObjectModel;
namespace PaperGeneral {

    public enum EModelType {

        none = 0,
        modelAssignemt = 1,
        modelClassRoom = 2,
        modelUser = 3
    }

    public enum EModelUserType {
        none = 0,
        teacher = 1,
        student = 2
    }

    public class ModelUser{
        public string name;
        public int id;
        public Collection<ModelAssignment> assignments = new Collection<ModelAssignment>();
        public Collection<string> classRooms = new Collection<string>();

        public EModelType type = EModelType.modelUser;
        public EModelUserType userType;


        public ModelUser(string name, int id, EModelUserType userType){

            this.name = name;
            this.id = id;
            this.userType = userType;
        }


        public ModelAssignment getAssignemt(int assignmentId){

            ModelAssignment modelAssignment = null;

            foreach(ModelAssignment assignment in this.assignments){

                if(assignment.id == assignmentId){
                    modelAssignment = assignment;
                }
            }

            return modelAssignment;
        }

        public Collection<AssignmentListItem> makeStudentAssignmentListItems(){

            Collection<AssignmentListItem> listItems = new Collection<AssignmentListItem>();

            foreach(ModelAssignment assignment in this.assignments){
                listItems.Add(assignment.makeListItem());
            }

            return listItems;

        }

        public Collection<AssignmentListItem> makeClassRoomAssignmentListItems(string classRoomName){

            Collection<AssignmentListItem> listItems = new Collection<AssignmentListItem>();

            foreach(ModelAssignment assignment in this.assignments){

                if(assignment.classRoomName == classRoomName){

                    listItems.Add(assignment.makeListItem());
                }
            }

            return listItems;
        }

        public Collection<Students_SubscriptionListItem> makeSubscriptionListItems(){

            Collection<Students_SubscriptionListItem> listItems = new Collection<Students_SubscriptionListItem>();

            foreach(string sub in this.classRooms){
                
                Collection<ModelAssignment> subcriptionAssignments = new Collection<ModelAssignment>();

                foreach(ModelAssignment assignment in this.assignments){
                    if(assignment.classRoomName == sub){
                        subcriptionAssignments.Add(assignment);
                    }
                }

                string nextdueDate = getNextDueDate(subcriptionAssignments);

                listItems.Add(new Students_SubscriptionListItem(sub, nextdueDate));

            }

            return listItems;
        }


        public string getNextDueDate(Collection<ModelAssignment> assignments){

            if(assignments.Count == 0){
                return "No assignments Due";
            }else{

                DateTime nextDue = new DateTime(1,1,1);

                bool isSet = false;

                foreach(ModelAssignment assignment in assignments){
                    int isFuture = DateTime.Compare(assignment.dueDate, DateTime.Now);

                    if(isFuture >= 0){
                        if(!isSet){
                            nextDue = assignment.dueDate;
                            isSet = true;
                        }else{

                            int isSooner = DateTime.Compare(assignment.dueDate, nextDue);

                            if(isSooner < 0){
                                nextDue = assignment.dueDate;
                            }
                        }
                    }
                }

                if(isSet){
                    return nextDue.ToString("d");
                }else{
                    return "No assignments Due";
                }
            }
        }

        public Collection<ClasroomListItem> makeClassRoomListItems(){

            Collection<ClasroomListItem> listItems = new Collection<ClasroomListItem>();

            foreach(string classRoom in this.classRooms){
                
                Collection<ModelAssignment> classRoomAssignments = new Collection<ModelAssignment>();

                foreach(ModelAssignment assignment in this.assignments){
                    if(assignment.classRoomName == classRoom){
                        classRoomAssignments.Add(assignment);
                    }
                }

                string nextDueDate = this.getNextDueDate(classRoomAssignments);

                listItems.Add(new ClasroomListItem(classRoom, nextDueDate));
            }


            return listItems;
        }

    }

    public class ModelClassRoom {
        public string name;
        public string teacherName;
        public Collection<string> assignments = new Collection<string>();
        public EModelType type = EModelType.modelClassRoom;

        public ModelClassRoom(string name) {
            this.name = name;

        }

    }

    public class ModelAssignment{
        public string name;
        public int id;
        public DateTime dueDate;
        public string description;
        public bool isDone;
        public EModelType type = EModelType.modelAssignemt;
        public string classRoomName;

        public ModelAssignment(string name, int id, DateTime dueDate, string description, bool isDone, string classRoomName){
            this.name = name;
            this.id = id;
            this.dueDate = dueDate;
            this.description = description;
            this.isDone = isDone;
            this.classRoomName = classRoomName;
        }


        public AssignmentListItem makeListItem(){

            AssignmentListItem listItem = new AssignmentListItem(
                this.name,
                this.dueDate,
                this.id,
                this.classRoomName,
                this.isDone
            );

            return listItem;
        }

    }


    public class ClasroomListItem {
        public string name;
        public string nextDue;

        public ClasroomListItem(string name, string nextDue) {

            this.name = name;
            this.nextDue = nextDue;
        }


    }

    public class ClassRooms_AssignmentListItem {
        public string name;
        public DateTime dueDate;
        public bool isClosed;
        public int id;

        public ClassRooms_AssignmentListItem(string name, DateTime dueDate, bool isClosed, int id) {

            this.name = name;
            this.dueDate = dueDate;
            this.isClosed = isClosed;
            this.id = id;
        }

    }


    public class Students_SubscriptionListItem {
        public string name;
        public string nextDue;



        public Students_SubscriptionListItem(string name, string nextDue) {

            this.name = name;
            this.nextDue = nextDue;

        }


    }

    public class AssignmentListItem {
        public string name;
        public DateTime dueDate;
        public bool isDone;
        public int id;
        public string classRoom;

        public AssignmentListItem(string name, DateTime dueDate, int id, string classRoom, bool isDone) {
            this.name = name;
            this.dueDate = dueDate;
            this.id = id;
            this.classRoom = classRoom;
            this.isDone = isDone;

        }

    }
}
