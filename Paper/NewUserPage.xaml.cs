using System;
using System.Collections.Generic;
using Paper;
using PaperGeneral;
using The_Model;


using Xamarin.Forms;

namespace Paper {
    public partial class NewUserPage : ContentPage {
        IHomeWorkModel model;

        public NewUserPage(IHomeWorkModel model) {
            InitializeComponent();
            this.model = model;


            NewUserPageControler controler = new NewUserPageControler(this.type_Picker, this.go_Button, this.model, this.userName_Entry, this.email_Entry, this.password_Entry, this);
        }
    }


    public class NewUserPageControler {
        Picker type_Picker;
        Button go_Button;
        IHomeWorkModel model;
        Entry name_Entry;
        Entry email_Entry;
        Entry password_Entry;
        Page page;

        public NewUserPageControler(Picker type_Picker, Button go_Button, IHomeWorkModel model, Entry name_Entry, Entry email_Entry, Entry password_Entry, Page page) {
            this.type_Picker = type_Picker;
            this.go_Button = go_Button;
            this.model = model;
            this.name_Entry = name_Entry;
            this.email_Entry = email_Entry;
            this.password_Entry = password_Entry;
            this.page = page;

            this.go_Button.Clicked += Go_Button_Clicked;
        }

        async void Go_Button_Clicked(object sender, EventArgs e) {
            int selectedIndex = this.type_Picker.SelectedIndex;

            if (selectedIndex == -1) {
                await this.page.DisplayAlert("", "You must select a user Type", "OK");

            } else {

                EModelUserType userType = EModelUserType.none;


                switch (selectedIndex) {
                    case 0:
                        userType = EModelUserType.student;
                        break;

                    case 1:
                        userType = EModelUserType.teacher;
                        break;
                }

                ServerResponse response = await this.model.createUser(this.name_Entry.Text, this.email_Entry.Text, this.password_Entry.Text, userType);

                if (!response.errorResponse.isOk) {
                    await this.page.DisplayAlert("Error", response.errorResponse.message, "OK");
                }

                LoginFetchObject login = response.loginObj;

                if (login != null) {

                    App.Current.Properties["userId"] = login.userId;

                    switch (login.userType) {

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
}
