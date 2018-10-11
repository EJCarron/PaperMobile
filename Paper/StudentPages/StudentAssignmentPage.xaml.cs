using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;

namespace Paper {
    public partial class StudentAssignmentPage : ContentPage {
        IHomeWorkModel model;
        public StudentAssignmentPage(int assignmentId, int userId) {
            this.model = ((App)Application.Current).model;

            InitializeComponent();
            this.assignmentView.register(this.model, assignmentId, userId);

            StudentAssignmentPage_Controler controler = new StudentAssignmentPage_Controler(assignmentId, this.model);

            controler.initialize();
        }
    }

    public class StudentAssignmentPage_Controler{
        int assignmentId;
        IHomeWorkModel model;

        public StudentAssignmentPage_Controler(int assignmentId, IHomeWorkModel model){

            this.assignmentId = assignmentId;
            this.model = model;

        }
        public void initialize(){

            this.model.initializeForAssignment(this.assignmentId);
        }

    }
}
