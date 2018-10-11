using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;

namespace Paper {
    public partial class AssignmentsPage : ContentPage {
        IHomeWorkModel model;

        public int studentId;

        public AssignmentsPage(int studentId) {
            InitializeComponent();
            this.studentId = studentId;
            this.model = ((App)Application.Current).model;
            this.assignmentListView.register(this.model, studentId);

            AssignmentsPage_Controler controler = new AssignmentsPage_Controler(assignmentListView, this, this.studentId);
        }


    }

    public class AssignmentsPage_Controler {
        Page page;
        int studentId;

        public AssignmentsPage_Controler(StudentAssignmentListView assignmentListView, Page page, int studentId) {

            assignmentListView.list.ItemTapped += AssignmentListView_ItemTapped;
            this.page = page;
            this.studentId = studentId;

        }


        async void AssignmentListView_ItemTapped(object sender, ItemTappedEventArgs e) {

            int assignmentId = ((StudentAssignmentViewCellData)e.Item).assignment_id;

            await this.page.Navigation.PushAsync(new StudentAssignmentPage(assignmentId, this.studentId));

        }
    }
}