using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Paper {
    public partial class studentBaseTabPage : TabbedPage {
        IHomeWorkModel model;
        int studentId;
        public studentBaseTabPage(int studentId) {
            this.model = ((App)Application.Current).model;
            this.studentId = studentId;
			InitializeComponent();

            NavigationPage subscriptionPage = new NavigationPage(new SubscriptionsPage(studentId));
            subscriptionPage.Title = "ClassRooms";

            NavigationPage assignmentPage = new NavigationPage(new AssignmentsPage(studentId));
			assignmentPage.Title = "Assignments";

            Children.Add(subscriptionPage );
            Children.Add(assignmentPage);

            //this.model.initializeForStudent(studentId);

           
        }
    }
}
