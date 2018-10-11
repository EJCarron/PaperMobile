using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using PaperGeneral;

namespace Paper {
    public partial class AssignmentPage : ContentPage {
        IHomeWorkModel model;

        public AssignmentPage(int assignmentId, ModelUser user) {
            InitializeComponent();
            this.model = ((App)Application.Current).model;

            this.assignmentView.register(model, assignmentId, user.id);

            AssignmentPage_Controler controler = new AssignmentPage_Controler(assignmentId, this.edit_Button, this.model, user, this);

            //controler.initialize();
        }
    }

    public class AssignmentPage_Controler{
        int assignmentId;
        Button edit_Button;
        IHomeWorkModel model;
        ModelUser user;
        Page page;

        public AssignmentPage_Controler (int assignmentId, Button edit_Button, IHomeWorkModel model,  ModelUser user, Page page){

            edit_Button.Clicked += Edit_Button_Clicked;

            this.assignmentId = assignmentId;
            this.edit_Button = edit_Button;
            this.model = model;
            this.user = user;
            this.page = page;
        }

        public void initialize(){
			this.model.initializeForAssignment(assignmentId);
		}
        async void Edit_Button_Clicked(object sender, EventArgs e) {
            await this.page.Navigation.PushAsync(new AssignmentEditPage(assignmentId, user));
        }
    }
}
