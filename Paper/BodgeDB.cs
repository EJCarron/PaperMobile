using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using General;
namespace BodgeDB {
	
    public class DataBase{
        public Dictionary<int, User> users = new Dictionary<int, User>();
        public Collection<Subscription> subscriptions = new Collection<Subscription>();
        public int nextUserId = 1;

        public int nextAssignmentId = 3;

        public User makeUser(string name){

            return new User(name, this.nextUserId++);
        }

        public ClassRoom makeClassRoom(string name, ref string error){
            
            return new ClassRoom(name);
        }

        public Assignment makeAssignment(string name, DateTime dueDate, string description, bool isClosed){


            return new Assignment(name, dueDate, description, isClosed, this.nextAssignmentId++);
        }
    }


    public class User {
        public Dictionary<string, ClassRoom> classRooms = new Dictionary<string, ClassRoom>();
        public string name;
        public int id;



        public User(string name, int id) {

            this.name = name;
            this.id = id;

        }


    }

	

	public class ClassRoom {
        public Dictionary<int, Assignment> assignments = new Dictionary<int, Assignment>();


		public string name;
		

		public string getNextDueDate() {

            Assignment nextDue = new Assignment(null, new DateTime(1,1,1) ,null, false,0);

            DateTime nextDueDate_temp = new DateTime(1,1,1);

            foreach (Assignment assignment in this.assignments.Values) {

                DateTime dueDate = assignment.dueDate;

                int result = DateTime.Compare(dueDate, nextDueDate_temp);

                if (result < 0 || nextDueDate_temp == new DateTime(1, 1, 1)) {

					nextDueDate_temp = dueDate;

                    nextDue = assignment;
                }

                if (nextDue.name == null) {

                    return "No assignments set.";
                }
            }
                return nextDue.dueDate.ToString();
            
		}

		public ClassRoom(string name) {


			this.name = name;

		}

	}

	public class Assignment {
		public string name;
		public int id;
        public DateTime dueDate;
		public string description;
		public bool isClosed;
        public Dictionary<int,AssignmentCompletion> completions = new Dictionary<int,AssignmentCompletion>();

        public Assignment(string name, DateTime dueDate, string description, bool isClosed, int id) {

			this.name = name;
			this.dueDate = dueDate;
            this.description = description;
			this.isClosed = isClosed;
            this.id = id;
		}


        public void addNewCompletion(int assignmentId, int studentId, DateTime completionDate){

            AssignmentCompletion newCompletion = new AssignmentCompletion(assignmentId, studentId, completionDate);

            completions.Add(studentId,newCompletion);
        }


	}

    public class AssignmentCompletion{
        public int assignmentid;
        public int studentid;
        DateTime completionDate;

        public AssignmentCompletion(int assignmentid, int studentid, DateTime completionDate){

            this.assignmentid = assignmentid;
            this.studentid = studentid;
            this.completionDate = completionDate;
        }

    }

    public class Subscription {
        public int studentId;
        public string classRoomName;

        public Subscription(int studentId, string classRoomName) {

            this.studentId = studentId;
            this.classRoomName = classRoomName;
        }

        public override bool Equals(object obj) {
            Subscription subscription = (Subscription)obj;

            return 
                subscription.studentId == this.studentId 
                && subscription.classRoomName == this.classRoomName;
        }

        public override int GetHashCode() {
            return (this.classRoomName + "#" + this.studentId.ToString()).GetHashCode();
        }
    }
}
