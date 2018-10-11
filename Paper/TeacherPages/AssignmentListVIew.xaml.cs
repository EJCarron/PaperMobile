using System;
using System.Collections.Generic;
using Xamarin.Forms;
using The_Model;
using System.Collections.ObjectModel;
using PaperGeneral;
namespace Paper {
    public partial class AssignmentListView : ContentView {
        IHomeWorkModel model;

        ModelUser user;
        string classRoomName;
		public ListView list {

			get {

                return this.assignmentListView;
			}
		}
        public AssignmentListView() {
            
            InitializeComponent();


        }
       
       
        public async void OnArchive(object sender, EventArgs e) {
            
            int assignmentId = (int)((MenuItem)sender).CommandParameter;

            if (assignmentId != 0) {

                await this.model.changeAssignmentArchive((int)assignmentId, true, user.id);
            }

		}

        private string getUserDataKey() {

            return DataKeys.getUserKey(this.user.id);
        }




        public void register(IHomeWorkModel model, ModelUser user, string classRoomName) {
            this.model = model;
            this.user = user;
            this.classRoomName = classRoomName;

            model.addWatcher(viewUpdated, this.getUserDataKey());

        }

        public void viewUpdated(EModelUpdateType updateType) {
            //Get data from model


            //change data into list of classRoomViewCellData

            Collection<AssignmentViewCellData> assignments = new Collection<AssignmentViewCellData>();

            Collection<AssignmentListItem> listItems = user.makeClassRoomAssignmentListItems(this.classRoomName);

            foreach(AssignmentListItem assignment in listItems){

                assignments.Add(new AssignmentViewCellData(assignment.name, assignment.dueDate, assignment.isDone, assignment.id));
            }

            //set datacells as item source for list
            this.assignmentListView.ItemsSource = assignments;


        }
    }

    public class AssignmentViewCellData {
        public string assignment_name { get; set; }
        public string dueDate { get; set; }
        public string dueColor { get; set; }
        public int assignment_id { get; set; }

        public AssignmentViewCellData(string assignment_name, DateTime dueDate, bool isClosed, int assignment_id) {

            this.assignment_name = assignment_name;
            this.dueDate = dueDate.ToString("d");

            int overDue = DateTime.Compare(dueDate, DateTime.Now);


            if(overDue < 0){

                this.dueColor = "#FF0000";
            }else{

                this.dueColor = "##b4eeb4";
            }

            this.assignment_id = assignment_id;
        }

        public int getAssignmentName(){

            return this.assignment_id;
        }
    }

    public class ViewCell_AssignmentView : ViewCell {


        public static readonly BindableProperty name_property =
            BindableProperty.Create("assignment_name", typeof(string), typeof(AssignmentViewCellData), "Assignment_name");
        public static readonly BindableProperty Due_property =
            BindableProperty.Create("dueDate", typeof(string), typeof(AssignmentViewCellData), "Due");
        public static readonly BindableProperty dueColor_property =
            BindableProperty.Create("dueColor", typeof(string), typeof(AssignmentViewCellData), "Due");
		public static readonly BindableProperty id_property =
			 BindableProperty.Create("assignment_id", typeof(int), typeof(AssignmentViewCellData), 0);


        public string assignment_name {
            get { return (string)this.GetValue(name_property); }
            set { this.SetValue(name_property, value); }
        }

        public string dueDate {
            get { return (string)this.GetValue(ViewCell_AssignmentView.Due_property); }
            set { this.SetValue(Due_property, value); }
        }

        public string dueColor {
            get { return (string)this.GetValue(ViewCell_AssignmentView.dueColor_property); }
            set { this.SetValue(dueColor_property, value); }
        }

		public int assignment_id {
			get { return (int)this.GetValue(ViewCell_AssignmentView.id_property); }
			set { this.SetValue(id_property, value); }
		}

    }
}
