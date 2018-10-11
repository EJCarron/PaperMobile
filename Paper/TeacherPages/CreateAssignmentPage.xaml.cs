using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PaperGeneral;

namespace Paper {
    public partial class CreateAssignmentPage : ContentPage {
        public CreateAssignmentPage(string classRoomName, int teacherId) {
            InitializeComponent();

            IModelStore model = ((App)Application.Current).model;

            //DatePicker stuff


            DateTime now = DateTime.Now;

            this.datePicker.Date = now;
			this.datePicker.MinimumDate = now;
			this.datePicker.MaximumDate = now.AddYears(1);
            this.datePicker.Format = "d";
			//--------------------------------------


            CreateAssignmentPage_Controler controler = new CreateAssignmentPage_Controler(name_Entry,datePicker,description_Entry,confirm_Button, this, classRoomName, teacherId);
        }
    }

    public class CreateAssignmentPage_Controler {
        Entry name_Entry;
        DatePicker datePicker;
        Entry description_Entry;
        CreateAssignmentPage page;
        string classRoomName;
        int teacherId;

        public CreateAssignmentPage_Controler(
            Entry name_Entry,DatePicker datePicker,Entry description_Entry,
            Button confirm_Button, 
            CreateAssignmentPage page, 
            string classRoomName,
            int teacherId
        ){

            confirm_Button.Clicked += Confirm_Button_Clicked;

            this.name_Entry = name_Entry;
            this.datePicker = datePicker;
            this.description_Entry = description_Entry;
            this.page = page;
            this.classRoomName = classRoomName;
            this.teacherId = teacherId;
        }

        async void Confirm_Button_Clicked(object sender, EventArgs e) {
			IHomeWorkModel model = ((App)Application.Current).model;

            string assignmentName = this.name_Entry.Text;
            DateTime assignmentDueDate = this.datePicker.Date;
            string assignmentDescription = this.description_Entry.Text;

			if (assignmentName != null) {
                    Task <Response> tAdd = model.addAssignmentToClassRoom(assignmentName, assignmentDueDate, assignmentDescription, false, classRoomName, teacherId);

                    Task <Page> tPop =  ((App)Application.Current).MainPage.Navigation.PopAsync();

                    await Task.WhenAll(tAdd, tPop);

			}
        }
    }
        
}
