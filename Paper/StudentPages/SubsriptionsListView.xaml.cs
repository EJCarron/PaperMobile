using System;
using System.Collections.Generic;
using Xamarin.Forms;
using The_Model;
using System.Collections.ObjectModel;
using PaperGeneral;

namespace Paper {
    public partial class SubscriptionsListView : ContentView {
        IHomeWorkModel model;
        int studentId;
         

        public async void refreshCommand(object sender, EventArgs e){
           
            await this.model.initializeForStudent(this.studentId);
            this.subscriptionsListView.EndRefresh();
        }

        public SubscriptionsListView() {
            InitializeComponent();
        }

        public async void unsubFromClassRoom(object sender, EventArgs e){
            string classRoomName = (string)((MenuItem)sender).CommandParameter;

            await this.model.unsubscribeStudent(studentId, classRoomName);

        }

		public void register(IHomeWorkModel model, int studentId) {
			this.model = model;
            this.studentId = studentId;


            model.addWatcher(viewUpdated, DataKeys.getUserKey(studentId));

		}

		public void viewUpdated(EModelUpdateType updateType) {
            //Get data from model
            ModelUser student = model.getItem<ModelUser>(DataKeys.getUserKey(studentId));

			//change data into list of classRoomViewCellData

			Collection<SubscriptionViewCellData> subscriptions = new Collection<SubscriptionViewCellData>();

            Collection<Students_SubscriptionListItem> subscriptionsListItems = student.makeSubscriptionListItems();

            foreach (Students_SubscriptionListItem subscription in subscriptionsListItems) {

                SubscriptionViewCellData viewCell = new SubscriptionViewCellData(subscription.name, subscription.nextDue);

                subscriptions.Add(viewCell);
			}

			//set datacells as item source for list
			this.subscriptionsListView.ItemsSource = subscriptions;


		}
    }

    public class SubscriptionViewCellData{
        public string subscription_name { get; set; }
        public string nextDue { get; set; }

        public SubscriptionViewCellData (string subscription_name, string nextDue){
            this.subscription_name = subscription_name;
            this.nextDue = nextDue;

        }
    }

    public class ViewCell_SubscriptionView : ViewCell{

		public static readonly BindableProperty name_property =
			BindableProperty.Create("subscription_name", typeof(string), typeof(AssignmentViewCellData), "subscription_name");
		public static readonly BindableProperty nextDue_property =
			BindableProperty.Create("nextDue", typeof(string), typeof(AssignmentViewCellData), "nextDue");
		
		


        public string subscription_name {
			get { return (string)this.GetValue(name_property); }
			set { this.SetValue(name_property, value); }
		}

		public string nextDue {
			get { return (string)this.GetValue(nextDue_property); }
			set { this.SetValue(nextDue_property, value); }
		}

    }
}
