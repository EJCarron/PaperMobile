using System;
using SqlWrapper;

namespace test2_mid {
    public static class Tables{
        public static DataTable AssignmentCompletions = new DataTable("AssignmentCompletions");
        public static DataTable Assignments = new DataTable("Assignments");
        public static DataTable ClassRooms = new DataTable("ClassRooms");
        public static DataTable LoginDetails = new DataTable("LoginDetails");
        public static DataTable Subscriptions = new DataTable("Subscriptions");
        public static DataTable Users = new DataTable("Users");

    }

    public class Assignments : DataTable {
        public Assignments(string tableName) : base(tableName) {
        }

        public static string tableName = "Assignments";

        public static COLUMN assignmentId = new COLUMN(Assignments.tableName, "assignmentId");
        public static COLUMN classRoomName = new COLUMN(Assignments.tableName, "classRoomName");
        public static COLUMN dueDate = new COLUMN(Assignments.tableName, "dueDate");
        public static COLUMN description = new COLUMN(Assignments.tableName, "description");
        public static COLUMN isClosed = new COLUMN(Assignments.tableName, "isClosed");
        public static COLUMN assignmentName = new COLUMN(Assignments.tableName, "assignmentName");

    }

    public class AssignmentCompletions : DataTable {
        public AssignmentCompletions(string tableName) : base(tableName) {
        }

        public static string tableName = "AssignmentCompletions";

        public static COLUMN assignmentId = new COLUMN(AssignmentCompletions.tableName, "assignmentId");
        public static COLUMN studemtId = new COLUMN(AssignmentCompletions.tableName, "studentId");
        public static COLUMN completionDate = new COLUMN(AssignmentCompletions.tableName, "completionDate");
    }

    public class ClassRooms : DataTable {
        public ClassRooms(string tableName) : base(tableName) {
        }

        public static string tableName = "ClassRooms";

        public static COLUMN classRoomName = new COLUMN(ClassRooms.tableName, "classRoomName");
        public static COLUMN teacherId = new COLUMN(ClassRooms.tableName, "teacherId");
    }

    public class LoginDetails : DataTable {
        public LoginDetails(string tableName) : base(tableName) {
        }

        public static string tableName = "LoginDetails";

        public static COLUMN userId = new COLUMN(LoginDetails.tableName, "userId");
        public static COLUMN emailAddress = new COLUMN(LoginDetails.tableName, "emailAddress");
        public static COLUMN userPassword = new COLUMN(LoginDetails.tableName, "userPassword");
    }

    public class Subscriptions : DataTable {
        public Subscriptions(string tableName) : base(tableName) {
        }

        public static string tableName = "Subscriptions";

        public static COLUMN studentId = new COLUMN(Subscriptions.tableName, "studentId");
        public static COLUMN classRoomName = new COLUMN(Subscriptions.tableName, "classRoomName");

    }

    public class Users : DataTable {
        public Users(string tableName) : base(tableName) {
        }

        public static string tableName = "Users";

        public static COLUMN userId = new COLUMN(Users.tableName, "userId");
        public static COLUMN userName = new COLUMN(Subscriptions.tableName, "userName");
        public static COLUMN userType = new COLUMN(Subscriptions.tableName, "userType");
    }
}
