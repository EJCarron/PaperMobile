using System;
using System.Collections.Generic;
using Xamarin.Forms;
using The_Model;
using System.Collections.ObjectModel;
using PaperGeneral;

namespace Paper {
    public partial class ClassRoomListView : ContentView {
        IHomeWorkModel model;
        int teacherId;
        public ListView list {

            get{

                return this.classListView;
            }
        }
            
        public ClassRoomListView() {
            InitializeComponent();
			
        }

        async void OnArchive(object sender, EventArgs e) {
            

            string classRoomName = (string)((MenuItem)sender).CommandParameter;

            if (classRoomName != null) {
                await this.model.changeClassRoomArchive(classRoomName, true, teacherId);
            }
		}


        private string getTeacherDataKey(){

            return DataKeys.getUserKey(this.teacherId) ;
        }




        public void register(IHomeWorkModel model, int teacherId) {
			this.model = model;
            this.teacherId = teacherId;
		

            model.addWatcher(viewUpdated, this.getTeacherDataKey());

		}

         public void viewUpdated(EModelUpdateType updateType) {
            
            //Get data from model
            ModelUser teacher = this.model.getItem<ModelUser>(this.getTeacherDataKey());

			if (teacher != null) {
                //change data into list of classRoomViewCellData

                this.teacherName_Label.Text = teacher.name;


                Collection<ClassRoomViewCellData> classRoomDatas = new Collection<ClassRoomViewCellData>();

                Collection<ClasroomListItem> classRooms = teacher.makeClassRoomListItems();

                foreach(ClasroomListItem classRoom in classRooms){

                    classRoomDatas.Add(new ClassRoomViewCellData(classRoom.name,classRoom.nextDue));
                }

                //set datacells as item source for list
                this.classListView.ItemsSource = classRoomDatas;

            }
		}
    }
	




    public class ClassRoomViewCellData {
		public string classRoom_name { get; set; }
		public string nextDue { get; set; }

		public ClassRoomViewCellData(string classRoom_name, string nextDue) {

			this.classRoom_name = classRoom_name;
			this.nextDue = nextDue;
		}

        public string getClassRoomName(){
            return this.classRoom_name;
        }
	}

	public class ViewCell_ClassRoomView : ViewCell {
       

   //     public ViewCell_ClassRoomView(){

			//MenuItem deleteAction = new MenuItem { Text = "Delete", IsDestructive = true };
        //    deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
        //    deleteAction.Clicked += async (sender, e) =>{


        //    }

        //}
		public static readonly BindableProperty name_property =
			BindableProperty.Create("classRoom_name", typeof(string), typeof(ClassRoomViewCellData), "classRoom_name");
		public static readonly BindableProperty nextDue_property =
			BindableProperty.Create("nextDue", typeof(string), typeof(ClassRoomViewCellData), "nextDue");

		public string classRoom_name {
			get { return (string)GetValue(name_property); }
			set { SetValue(name_property, value); }
		}

		public string nextDue {
			get { return (string)GetValue(nextDue_property); }
			set { SetValue(nextDue_property, value); }
		}



	}
}
