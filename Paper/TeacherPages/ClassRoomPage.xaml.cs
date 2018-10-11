using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PaperGeneral;

namespace Paper {
    public partial class ClassRoomPage : ContentPage {
        IHomeWorkModel model;

        public string classRoomName;

        public ClassRoomPage(string classRoomName, int teacherId) {
            this.classRoomName = classRoomName;

            this.model = ((App)Application.Current).model;
            this.model.initializeForClassRoom(classRoomName);

            InitializeComponent();

            this.classRoomName_Label.Text = classRoomName;

            ModelUser user = this.model.getItem<ModelUser>(DataKeys.getUserKey(teacherId));
       
            this.assignmentListView.register(model,user, classRoomName);

            ClassRoomPageControler controler = new ClassRoomPageControler(this,addAssignment_button,teacherId, classRoomName, assignmentListView, user);

        }


    }

	public class ClassRoomPageControler {
        ClassRoomPage page;
        int teacherId;
        string classRoomName;
        ModelUser user;

        IHomeWorkModel model = ((App)Application.Current).model;

        public ClassRoomPageControler(ClassRoomPage page,Button addAssignment_button, int teacherId, string classRoomName, AssignmentListView assignmentListView, ModelUser user) {
            this.page = page;
			addAssignment_button.Clicked += addAssignment_buttonClick;
            assignmentListView.list.ItemTapped += assignmentListView_tapped;
            this.teacherId = teacherId;
            this.classRoomName = classRoomName;
            this.user = user;



		}

		async void addAssignment_buttonClick(object sender, EventArgs e) {
			

            await((App)Application.Current).MainPage.Navigation.PushAsync(new CreateAssignmentPage(classRoomName, teacherId));


		}

        async void assignmentListView_tapped(object sender, ItemTappedEventArgs e) {

            int assignmentId = ((AssignmentViewCellData)e.Item).assignment_id;

            await ((App)Application.Current).MainPage.Navigation.PushAsync(new AssignmentPage(assignmentId, user));


		}


    }
}
