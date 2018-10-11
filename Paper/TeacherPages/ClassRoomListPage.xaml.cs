using System;
using System.Collections.Generic;
using The_Model;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PaperGeneral;
using System.Threading.Tasks;

namespace Paper {
    public partial class TeacherPage : ContentPage {
        IHomeWorkModel model;

        public TeacherPage(int teacherId) {

            this.model = ((App)Application.Current).model;
            //this.model.initializeForTeacher(teacherId);

            InitializeComponent();

            this.classRoomListView.register(this.model, teacherId);

            TeacherPageControler controler = new TeacherPageControler(addClassRoom_button, classRoomListView, teacherId, this, this.logout_Button, this.model);
        }

       



    }


    public class TeacherPageControler{
        int teacherId;
        Page page;
        Button logout_Button;
        IHomeWorkModel model;

        public TeacherPageControler(
            Button addClassRoom_button, ClassRoomListView classRoomListView, int teacherId, Page page, Button logout_Button, IHomeWorkModel model){

                addClassRoom_button.Clicked += addClassRoom_buttonClick;
                classRoomListView.list.ItemTapped += ClassRoomListView_tapped;
                this.teacherId = teacherId;
            this.page = page;
            this.logout_Button = logout_Button;
            this.model = model;
            this.logout_Button.Clicked += Logout_Button_Clicked;
        }

        async void addClassRoom_buttonClick(object sender, EventArgs e) {
            
            await this.page.Navigation.PushAsync(new CreateClassRoomPage(this.teacherId));
		}

        async void ClassRoomListView_tapped(object sender, ItemTappedEventArgs e){

            string classRoomName = ((ClassRoomViewCellData)e.Item).getClassRoomName();

            await this.page.Navigation.PushAsync(new ClassRoomPage(classRoomName,teacherId));


        }

        void Logout_Button_Clicked(object sender, EventArgs e) {


            Application.Current.Properties.Remove("userId");
            Application.Current.Properties.Remove("usertype");
            App.Current.MainPage = new NavigationPage(new LoginPage(this.model));
        }
         
    } 
}
