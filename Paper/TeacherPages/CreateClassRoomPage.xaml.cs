using System;
using System.Collections.Generic;
using The_Model;
using System.Threading.Tasks;
using PaperGeneral;

using Xamarin.Forms;

namespace Paper {
    public partial class CreateClassRoomPage : ContentPage {
        public CreateClassRoomPage(int teacherId) {
            InitializeComponent();

            IModelStore model = ((App)Application.Current).model;

            CreateClassRoomPageControler controler = new CreateClassRoomPageControler(confirm_button, newClassRoom_entry, teacherId);
        }
    }

    public class CreateClassRoomPageControler {
        Entry newClassRoom_entry;
        int teacherId;

        public CreateClassRoomPageControler(Button confirm_button, Entry newClassRoom_entry, int teacherId) {

            confirm_button.Clicked += confirm_buttonClicked;

            this.newClassRoom_entry = newClassRoom_entry;
            this.teacherId = teacherId;
        }

         async void confirm_buttonClicked(object sender, EventArgs e) {
            IHomeWorkModel model = ((App)Application.Current).model;

            string classRoomname = this.newClassRoom_entry.Text;

            if (classRoomname != null) {

                Task<Response> tAdd =  model.addClassRoomToTeacher(classRoomname, this.teacherId);

                Task<Page> tPop =  ((App)Application.Current).MainPage.Navigation.PopAsync();

                await Task.WhenAll(tPop, tAdd);
            }
        }
    }
}
