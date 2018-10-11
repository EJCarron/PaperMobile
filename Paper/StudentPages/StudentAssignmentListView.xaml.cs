using System;
using System.Collections.Generic;
using Xamarin.Forms;
using The_Model;
using System.Collections.ObjectModel;
using PaperGeneral;

namespace Paper {
    public partial class StudentAssignmentListView : ContentView {
        IHomeWorkModel model;
        int studentId;

        public ListView list{

            get{

                return this.assignmentListView;
            }
        }

        public async void refreshCommand(object sender, EventArgs e) {

            await this.model.initializeForStudent(this.studentId);
            this.assignmentListView.EndRefresh();
        }

        public StudentAssignmentListView() {
            InitializeComponent();
        }

        public async void changeIsDoneState(object sender, EventArgs e) {

            AssignmentListItem assignment = (AssignmentListItem)((MenuItem)sender).CommandParameter;

            if (assignment.id != 0) {

                if (assignment.isDone == true) {
                    await this.model.undoMarkAssignmentDone(assignment.id, studentId);
                } else if (assignment.isDone == false) {

                    await this.model.markAssignmentDone(assignment.id, studentId);
                }
            }

        }

        public void register(IHomeWorkModel model, int studentId){
            this.model = model;
            this.studentId = studentId;


            model.addWatcher(viewUpdated, DataKeys.getUserKey(studentId));
        }


        public void viewUpdated(EModelUpdateType updateType){

            //Get data from model
            ModelUser student = this.model.getItem<ModelUser>(DataKeys.getUserKey(studentId));

            //change data into list of AssignmentViewCellData

            Collection<StudentAssignmentViewCellData> assignments = new Collection<StudentAssignmentViewCellData>();

            Collection<AssignmentListItem> listItems = student.makeStudentAssignmentListItems();
            foreach (AssignmentListItem assignment in listItems) {

               
                int dueDateResult = DateTime.Compare(assignment.dueDate, DateTime.Now);


                if (!(dueDateResult <= 0 && assignment.isDone == true)) {
                    

                    StudentAssignmentViewCellData viewCell = new StudentAssignmentViewCellData(
                                                                                               assignment,
                                                                                                this
                                                                                                            );

                    assignments.Add(viewCell);
                }
            }

            //set datacells as item source for list
            this.assignmentListView.ItemsSource = assignments;

        }
    }

    public class StudentAssignmentViewCellData {
        public string assignment_name { get; set; }
        public DateTime dueDate { get; set; }
        public string doneColor { get; set; }
        public int assignment_id { get; set; }
        public string isDone { get; set; }
        public string classRoom { get; set; }
        public bool markedDone;
        public AssignmentListItem assignment { get; set; }

        StudentAssignmentListView list;

        public StudentAssignmentViewCellData(
            AssignmentListItem assignment,
            StudentAssignmentListView list
        
        ){
            this.list = list;
            this.assignment = assignment;

            this.assignment_name = assignment.name;
            this.dueDate = assignment.dueDate;
            this.assignment_id = assignment.id;
            this.classRoom = assignment.classRoom;
            this.markedDone = assignment.isDone;




            int dueDateResult = DateTime.Compare(this.dueDate, DateTime.Now);


            if (dueDateResult>0 && this.markedDone == true) {
                
                this.doneColor = "#66ff66";
                this.isDone = "Undo";


            }  else if (dueDateResult <= 0 && this.markedDone == false){

                this.doneColor = "#ff0000";
                this.isDone = "Mark Done";

            } else if (dueDateResult > 0 && this.markedDone == false){

                this.doneColor = "#3399ff";
                this.isDone = "MarkDone";
            }


        }
    }

    public class ViewCell_StudentAssignmentView : ViewCell{

        public static readonly BindableProperty name_property =
            BindableProperty.Create("assignment_name", typeof(string), typeof(StudentAssignmentViewCellData), "Assignment_name");
        public static readonly BindableProperty Due_property =
            BindableProperty.Create("dueDate", typeof(string), typeof(StudentAssignmentViewCellData), "Due");
        public static readonly BindableProperty doneColor_property =
            BindableProperty.Create("doneColor", typeof(string), typeof(StudentAssignmentViewCellData), "Due");
        public static readonly BindableProperty contextAction_property =
             BindableProperty.Create("assignment", typeof(AssignmentListItem), typeof(StudentAssignmentViewCellData), null);
        public static readonly BindableProperty isDone_property =
            BindableProperty.Create("isDone", typeof(string), typeof(StudentAssignmentViewCellData), "isDone");
        public static readonly BindableProperty classRoom_property =
            BindableProperty.Create("classRoom", typeof(string), typeof(StudentAssignmentViewCellData), "classRoom");


        public string assignment_name {
            get { return (string)this.GetValue(name_property); }
            set { this.SetValue(name_property, value); }
        }

        public string dueDate {
            get { return (string)this.GetValue(Due_property); }
            set { this.SetValue(Due_property, value); }
        }

        public string doneColor {
            get { return (string)this.GetValue(doneColor_property); }
            set { this.SetValue(doneColor_property, value); }
        }

        public AssignmentListItem assignment {
            get { return (AssignmentListItem)this.GetValue(contextAction_property); }
            set { this.SetValue(contextAction_property, value); }
        }

        public string isDone {
            get { return (string)this.GetValue(isDone_property); }
            set { this.SetValue(isDone_property, value); }
        }

        public string classRoom {
            get { return (string)this.GetValue(classRoom_property); }
            set { this.SetValue(classRoom_property, value); }
        }
    }
}
