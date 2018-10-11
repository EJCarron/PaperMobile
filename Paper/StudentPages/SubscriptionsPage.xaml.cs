using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PaperGeneral;

namespace Paper {
    public partial class SubscriptionsPage : ContentPage {
        IHomeWorkModel model;

        public SubscriptionsPage(int studentId) {
            this.model = ((App)Application.Current).model;

            InitializeComponent();

            this.subscriptionListView.register(this.model, studentId);

            SubscriptionPageControler controler = new SubscriptionPageControler(
                studentId,
                this.addSubscription_Button,
                this,
                this.model,
                this.logout_Button
            );
        }
    }

    public class SubscriptionPageControler{
        int studentId;
        Button addSubscription_Button;
        Page page;
        IHomeWorkModel model;
        Button logout_Button;

        public SubscriptionPageControler(int studentId, Button addSubscription_Button, Page page, IHomeWorkModel model, Button logout_Button){
            addSubscription_Button.Clicked += AddSubscription_Button_Clicked;

            this.studentId = studentId;
            this.addSubscription_Button = addSubscription_Button;
            this.page = page;
            this.model = model;
            this.logout_Button = logout_Button;

            this.logout_Button.Clicked += Logout_Button_Clicked;
        }

		async void AddSubscription_Button_Clicked(object sender, EventArgs e) {

            await this.page.Navigation.PushAsync(new SubscriptionSearchPage(this.studentId));

            //ServerResponse response = await this.model.fetchDataBaseObject(new PaperGeneral.ClassRoomFetchObject(1, "hist8"), PaperGeneral.ECmdObjType.classRoomFetchObj);

            //if (response.fetchObject == null){

            //    await this.page.DisplayAlert("ERROR", response.errorResponse.message, "OK");
            //}else{

            //    await this.page.DisplayAlert("WOOOO", response.fetchObject.render(), "OK");
            //}
		}


        void Logout_Button_Clicked(object sender, EventArgs e) {
            Application.Current.Properties.Remove("userId");
            Application.Current.Properties.Remove("usertype");
            App.Current.MainPage = new NavigationPage(new LoginPage(this.model));
        }
    }
}
