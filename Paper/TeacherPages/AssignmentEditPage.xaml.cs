using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using General;
using PaperGeneral;

namespace Paper {

    public partial class AssignmentEditPage : ContentPage {
      

        public AssignmentEditPage(int assignmentId, ModelUser user) {
            InitializeComponent();

            IModelStore model = ((App)Application.Current).model;



            ModelAssignment assignment = user.getAssignemt(assignmentId);

            this.name_Entry.Text = assignment.name;
            this.description_Entry.Text = assignment.description;


            //DatePicker stuff
            this.datePicker.Date = assignment.dueDate;

            DateTime now = DateTime.Now;
            this.datePicker.Format = "d";
            this.datePicker.MinimumDate = now;
            this.datePicker.MaximumDate = now.AddYears(1);
            //--------------------------------------


            AssignmentEditPage_Controler controler = new AssignmentEditPage_Controler(
                name_Entry, datePicker, description_Entry,
                confirm_Button,
                this,
                assignmentId,
                user.id
            );
        }
    }

    public class AssignmentEditPage_Controler {
		Entry name_Entry;
        DatePicker datePicker;
		Entry description_Entry;
        AssignmentEditPage page;
		int assignmentId;
        int userId;

		public AssignmentEditPage_Controler(
            Entry name_Entry, DatePicker datePicker, Entry description_Entry,
			Button confirm_Button,
            AssignmentEditPage page,
            int assignmentId, int userId){

			confirm_Button.Clicked += Confirm_Button_Clicked;

			this.name_Entry = name_Entry;
			this.datePicker = datePicker;
			this.description_Entry = description_Entry;
			this.page = page;
            this.assignmentId = assignmentId;
            this.userId = userId;
        }

		async void Confirm_Button_Clicked(object sender, EventArgs e) {
			IHomeWorkModel model = ((App)Application.Current).model;

			string assignmentName = this.name_Entry.Text;
            DateTime assignmentDueDate = this.datePicker.Date;
			string assignmentDescription = this.description_Entry.Text;

			if (assignmentName != null) {

                    Task<Page> tPop = ((App)Application.Current).MainPage.Navigation.PopAsync();

                  

                Task<Response> tEdit =  model.editAssignment(assignmentName,assignmentDueDate,assignmentDescription,false, assignmentId, userId);

                    await Task.WhenAll(tPop, tEdit);
				
			}
		} 
    }
}
