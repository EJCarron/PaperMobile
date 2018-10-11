using Xamarin.Forms;
using The_Model;
using System;
using System.Collections.Generic;
namespace Paper {
    public partial class App : Application {
        public IHomeWorkModel model = new HttpHomeWorkModel();


        public App() {
            InitializeComponent();

            // this.MainPage = new testingPage(model);

            // this.MainPage = new NavigationPage( new TeacherPage(1) );
            //this.MainPage = new studentBaseTabPage(3);


        }

        protected override void OnStart() {

            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey("userId")){

                int id = (int)properties["userId"];
                int type = (int)properties["usertype"];


                switch(type){

                    case 1:
                        this.MainPage = new NavigationPage(new TeacherPage(id));
                        this.model.initializeForTeacher(id);
                        break;
                     
                    case 2:
                        this.MainPage = new studentBaseTabPage(id);
                        this.model.initializeForStudent(id);
                        break;
                }

            }else{
                this.MainPage = new NavigationPage(new LoginPage(this.model));
            }


        }

        protected override void OnSleep() {
            
        }

        protected override void OnResume() {

            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey("userId")) {

                int id = (int)properties["userId"];
                int type = (int)properties["usertype"];


                switch (type) {

                    case 1:
                        this.MainPage = new NavigationPage(new TeacherPage(id));
                        break;

                    case 2:
                        this.MainPage = new studentBaseTabPage(id);
                        break;
                }

            } else {
                this.MainPage = new NavigationPage(new LoginPage(this.model));
            }
        }

    }




}
