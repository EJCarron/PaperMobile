using System;
using System.Collections.Generic;
using The_Model;
using PaperGeneral;
using Paper;

using Xamarin.Forms;

namespace Paper {
    public partial class LoginPage : ContentPage {
        IHomeWorkModel model;


        public LoginPage(IHomeWorkModel model) {
            InitializeComponent();

            this.model = model;


            LoginPageControler controler = new LoginPageControler(this.model, this.email_Entry, this.password_Entry, this.login_Button, this.newUser_Button, this);
        }

    }


    public class LoginPageControler {
        IHomeWorkModel model;
        Entry email;
        Entry password;
        Button login_Button;
        Button newUser_Button;
        Page page;

        public LoginPageControler(IHomeWorkModel model, Entry email, Entry password, Button login_Button, Button newUser_Button, Page page){
            this.model = model;
            this.email = email;
            this.password = password;
            this.login_Button = login_Button;
            this.newUser_Button = newUser_Button;
            this.page = page;
            this.login_Button.Clicked += Login_Button_Clicked;
            this.newUser_Button.Clicked += NewUser_Button_Clicked;
        }

        async void NewUser_Button_Clicked(object sender, EventArgs e) {

            await ((App)Application.Current).MainPage.Navigation.PushAsync(new NewUserPage(this.model));

        }

        async void Login_Button_Clicked(object sender, EventArgs e) {
            
            ServerResponse response = await this.model.attemptLogin(this.email.Text, this.password.Text);

            if(!response.errorResponse.isOk){

                await this.page.DisplayAlert("Sorry", response.errorResponse.message, "Ok");
            }

            LoginFetchObject login = response.loginObj;

            if (login != null){

                App.Current.Properties["userId"] = login.userId;

                switch (login.userType){
                    
                    case EModelUserType.student:
                        App.Current.Properties["usertype"] = 2;
                        App.Current.MainPage = new studentBaseTabPage(login.userId);
                        break;

                    case EModelUserType.teacher:
                        App.Current.Properties["usertype"] = 1;
                        App.Current.MainPage = new NavigationPage(new TeacherPage(login.userId));
                        break;

                }

                    

            }

        }
    }
}
