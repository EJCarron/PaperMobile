//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;

//namespace PaperGeneral {












//    public class Model_Teacher {
//        public Collection<Teachers_ClasroomListItem> classRooms = new Collection<Teachers_ClasroomListItem>();
//        public string name;
//        public int id;
//        public EModelType type = EModelType.modelTeacher;


//        public Model_Teacher(string name, int id) {

//            this.name = name;
//            this.id = id;

//        }
//    }

//    public class Model_Student {
//        public Collection<Students_SubscriptionListItem> subscriptions = new Collection<Students_SubscriptionListItem>();
//        public Collection<Students_AssignmentListItem> assignments = new Collection<Students_AssignmentListItem>();
//        public string name;
//        public int id;
//        public EModelType type = EModelType.modelStudent;

//        public Model_Student(string name, int id) {

//            this.name = name;
//            this.id = id;
//        }


//    }



//    public class Model_ClassRoom {
//        public Collection<ClassRooms_AssignmentListItem> assignments = new Collection<ClassRooms_AssignmentListItem>();
//        public Collection<int> students;

//        public string name;
//        public DateTime nextDueDate;
//        public EModelType type = EModelType.modelClassRoom;

//        public string getNextDueDate() {

//            ClassRooms_AssignmentListItem nextDue = new ClassRooms_AssignmentListItem("", new DateTime(0, 0, 0), false, 0);

//            DateTime nextDueDate_temp = new DateTime(0, 0, 0);

//            foreach (ClassRooms_AssignmentListItem assignment in this.assignments) {

//                DateTime dueDate = assignment.dueDate;

//                int result = DateTime.Compare(dueDate, nextDueDate_temp);

//                if (result < 0 || nextDueDate_temp == new DateTime(0, 0, 0)) {

//                    nextDueDate_temp = dueDate;

//                    nextDue = assignment;

//                }
//            }

//            if (nextDue.name == null) {

//                return "No assignments set.";
//            }

//            return nextDue.dueDate.ToString();
//        }

//        public Model_ClassRoom(string name) {


//            this.name = name;

//        }

//    }

//    public class Model_Assignment {
//        public string name;
//        public int id;
//        public DateTime dueDate;
//        public string description;
//        public bool isClosed;
//        public bool isDone;
//        public EModelType type = EModelType.modelAssignemt;

//        public Model_Assignment(string name, DateTime dueDate, string description, bool isClosed) {

//            this.name = name;
//            this.dueDate = dueDate;
//            this.description = description;
//            this.isClosed = isClosed;
//        }
//    }


//}
