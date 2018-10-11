using System;
using System.Collections.Generic;
using The_Model;
using PaperGeneral;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Paper {


    public partial class testingPage : ContentPage {

        CommandCentre theCommandCentre = new CommandCentre();
        IHomeWorkModel model;
        Collection<SwitchCell> switches = new Collection<SwitchCell>();

        public testingPage(IHomeWorkModel model) {
            InitializeComponent();
            this.model = model;

            switches.Add(this.addAssignment_Switch);
            switches.Add(this.addClassRoom_Switch);
            switches.Add(this.createStudent_Switch);
            switches.Add(this.createTeacher_Switch);

            switches.Add(this.editAssignment_Switch);
            switches.Add(this.editStudent_Switch);
            switches.Add(this.editTeacher_Switch);

            switches.Add(this.archiveStudent_Switch);
            switches.Add(this.archiveTeacher_Switch);
            switches.Add(this.archiveClassRoom_Switch);
            switches.Add(this.archiveAssignment_Switch);

            switches.Add(this.unArchiveStudent_Switch);
            switches.Add(this.unArchiveTeacher_Switch);
            switches.Add(this.unArchiveAssignment_Switch);
            switches.Add(this.unArchiveClassRoom_Switch);

            switches.Add(this.sub_Switch);
            switches.Add(this.unsub_Switch);
            switches.Add(this.markDone_Switch);
            switches.Add(this.undoMarkDone_Switch);

            switches.Add(this.initStudent_Switch);
            switches.Add(this.initTeacher_Switch);




            //Setting up CommandCentre ---------------------


            this.theCommandCentre.addCommand(this.createTeacher_Switch.Text, new CreateTeacher(model, "Clark", "Clark@Test", "1234"));
            this.theCommandCentre.addCommand(this.addClassRoom_Switch.Text, new AddClassRoomToTeacher(model, "maths16", 1));
            //remember to make unique NAME EACH TIME!!!!1
            this.theCommandCentre.addCommand(this.addAssignment_Switch.Text, new AddAssignmentToClassRoom(
                    model,
                    "testAssignment7",
                    new DateTime(1, 1, 1),
                    "teeeeeest",
                    false,
                    "maths16",
                    1
            ));
            this.theCommandCentre.addCommand(this.editAssignment_Switch.Text, new EditAssignment(
                    model,
                    "testAssignment7",
                    new DateTime(1, 1, 1),
                    "lorsalh",
                    false,
                    1,
                    1
                ));


            this.theCommandCentre.addCommand(this.createStudent_Switch.Text, new CreateStudent(model, "Ed","ed@Test", "1234"));





            this.theCommandCentre.addCommand(this.editStudent_Switch.Text, new EditStudent(model,3, "Ed", "Ed@Test", "54321"));
            this.theCommandCentre.addCommand(this.editTeacher_Switch.Text, new EditTeacher(model, 1, "Brown", "Brown@Test", "4321"));

            this.theCommandCentre.addCommand(this.archiveClassRoom_Switch.Text, new ArchiveClassRoom(model, "hist8", 1));
            this.theCommandCentre.addCommand(this.archiveAssignment_Switch.Text, new ArchiveAssignment(model, 34, 1));
            this.theCommandCentre.addCommand(this.archiveStudent_Switch.Text, new ArchiveStudent(model, 3));
            this.theCommandCentre.addCommand(this.archiveTeacher_Switch.Text, new ArchiveTeacher(model, 1));

            this.theCommandCentre.addCommand(this.unArchiveClassRoom_Switch.Text, new UnArchiveClassRoom(model, "hist8", 1));
            this.theCommandCentre.addCommand(this.unArchiveAssignment_Switch.Text, new UnArchiveAssignment(model, 34, 1));
            this.theCommandCentre.addCommand(this.unArchiveStudent_Switch.Text, new UnArchiveStudent(model, 3));
            this.theCommandCentre.addCommand(this.unArchiveTeacher_Switch.Text, new UnArchiveTeacher(model, 1));

            this.theCommandCentre.addCommand(this.sub_Switch.Text, new SubStudent(model, 4, "hist8"));
            this.theCommandCentre.addCommand(this.unsub_Switch.Text, new UnSubStudent(model, 4, "hist8"));
            this.theCommandCentre.addCommand(this.markDone_Switch.Text, new MarkAssignmentDone(model, 2, 3));
            this.theCommandCentre.addCommand(this.undoMarkDone_Switch.Text, new UndoMarkAssignmentDone(model, 2, 3));

            this.theCommandCentre.addCommand(this.initStudent_Switch.Text, new InitStudent(model, 1));
            this.theCommandCentre.addCommand(this.initTeacher_Switch.Text, new InitTeacher(model, 1));





            TestingPageControler controler = new TestingPageControler(this.switches, this.model, this.theCommandCentre, this.go_Button, this, this.fullTest_Button);

        }
    }


    public class TestingPageControler {
        Collection<SwitchCell> switches;
        IHomeWorkModel model;
        CommandCentre theCommandCentre;
        Button go_Button;
        Page page;
        Button fullTest_Button;

        public TestingPageControler(
            Collection<SwitchCell> switches,
            IHomeWorkModel model,
            CommandCentre theCommandCentre,
            Button go_Button,
            Page page,
            Button fullTest_Button
        ) {



            this.switches = switches;
            this.model = model;
            this.theCommandCentre = theCommandCentre;
            this.go_Button = go_Button;
            this.page = page;
            this.fullTest_Button = fullTest_Button;

            this.go_Button.Clicked += go_Button_Clicked;
            this.fullTest_Button.Clicked += fullTest_Button_Clicked;
        }

        async void go_Button_Clicked(object sender, EventArgs e) {

            Collection<string> commandKeys = new Collection<string>();

            commandKeys.Clear();

            foreach (SwitchCell testSwitch in this.switches) {

                if (testSwitch.On) {

                    commandKeys.Add(testSwitch.Text);
                }

            }

            string reply = await this.theCommandCentre.executeCommands(commandKeys);

            await this.page.DisplayAlert("Reply", reply, "OK");
        }


        async void fullTest_Button_Clicked(object sender, EventArgs e){

            string reply = await this.theCommandCentre.executeAllCommands();

            await this.page.DisplayAlert("Reply", reply, "Ok");

        }
    }
}

