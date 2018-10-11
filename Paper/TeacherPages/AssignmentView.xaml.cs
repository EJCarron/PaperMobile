using System;
using System.Collections.Generic;
using Xamarin.Forms;
using The_Model;
using System.Collections.ObjectModel;
using PaperGeneral;

namespace Paper {
    public partial class AssignmentView : ContentView {
        IModelStore model;
        int assignmentId;
        int userId;

        public AssignmentView() {
            InitializeComponent();
        }

        private string getUserDataKey(){

            return DataKeys.getUserKey(userId);
        }

        public void register(IModelStore model, int assignmentId, int userId){

            this.model = model;
            this.assignmentId = assignmentId;
            this.userId = userId;


            model.addWatcher(viewUpdated, this.getUserDataKey());
        }

        public void viewUpdated(EModelUpdateType updateType){

            //Get data from Model
            ModelAssignment assignment = this.model.getItem<ModelUser>(this.getUserDataKey()).getAssignemt(assignmentId);

            this.name_Label.Text = assignment.name;
            this.dueDate_Label.Text = assignment.dueDate.ToString();
            this.description_Label.Text = assignment.description;
        }
    }
}
