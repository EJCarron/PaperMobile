using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using General;
using PaperGeneral;

namespace Paper {
    public partial class SubscriptionSearchPage : ContentPage {
        IHomeWorkModel model;
        int studentId;
        public SubscriptionSearchPage(int studentId) {
            this.model = ((App)Application.Current).model;
            this.studentId = studentId;
            InitializeComponent();



            SubsriptionSearchPage_Controler controler = new SubsriptionSearchPage_Controler(
                this.confirmSub_Button,
                this.studentId,
                subName_Entry,
                this.model,
                this,
                this.confirmingText
            );
        }
    }

    public class SubsriptionSearchPage_Controler{
        Button confirmSub_Button;
        int studentId;
        Entry subName_Entry;
        IHomeWorkModel model;
        Page page;
        Label confirmingText;

        public SubsriptionSearchPage_Controler(
            Button confirmSub_Button,
            int studentId,
            Entry subName_Entry,
            IHomeWorkModel model,
            Page page,
            Label confirmingText
        ){

            this.confirmingText = confirmingText;           
            this.confirmSub_Button = confirmSub_Button;
            this.studentId = studentId;
            this.subName_Entry = subName_Entry;
            this.model = model;
            this.page = page;
            this.confirmSub_Button.Clicked += Confirm_Button_Clicked;
        }

        async void Confirm_Button_Clicked(object sender, EventArgs e) {

            this.confirmingText.Text = "Subscription attempt sent";

            this.confirmSub_Button.IsEnabled = false;

            Task<Response> tConfirm = this.model.SubscribeStudentToClassRoom(studentId, subName_Entry.Text);

            await Task.WhenAll(tConfirm);

            if (tConfirm.Result.state == ResponseState.ok) {
                this.confirmingText.Text = "";
                this.confirmSub_Button.IsEnabled = true;

                await ((App)Application.Current).MainPage.DisplayAlert("Success", "YOU ARE NOW SUBSCRIBED!!", "Ok");

            }else if (tConfirm.Result.state == ResponseState.error){
				this.confirmingText.Text = "";
				this.confirmSub_Button.IsEnabled = true;
                string message = tConfirm.Result.message;
                await ((App)Application.Current).MainPage.DisplayAlert("Sorry", message, "Ok");
                //await this.page.DisplayAlert("Sorry",message,"Ok");


            }
        }
    }
}
