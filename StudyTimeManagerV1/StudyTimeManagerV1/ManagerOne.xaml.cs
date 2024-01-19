using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using ClassLibraryStudy;  // references class library used


namespace StudyTimeManagerV1
{
    /// <summary>
    /// Interaction logic for ManagerOne.xaml
    /// </summary>
    public partial class ManagerOne : Window
    {
        // lists created to use Module and Semester classes

        List<Module> modules = new List<Module>();
        List<Semester> semesters = new List<Semester>();

        // lists created to store values for calculation of self study hours

        List<int> creditsList = new List<int>();
        List<int> classHoursList = new List<int>();

        // date variables
        DateTime startDateSemester;
        DateTime workedDateSemester;
        

        // counter to iterate through datagrid

        int i = 0;

        public ManagerOne()
        {
            InitializeComponent();
        }

        private void btnAddModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // validation
                if (tbName.Text.Length == 0 || tbCode.Text.Length == 0 || tbCredits.Text.Length == 0 || tbClassHours.Text.Length == 0)
                {
                    MessageBox.Show("Fields cannot be left blank");
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to add this module?", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        // store user input (code, name, credits, class hours) using Module class (as a list)
                        modules.Add(new Module
                        {
                            modCode = tbCode.Text,
                            modName = tbName.Text,
                            modCredits = Convert.ToInt32(tbCredits.Text),
                            modClassHours = Convert.ToInt32(tbClassHours.Text)
                        });

                        // add module info to data grid
                        dgModuleInfo.Items.Add(modules[i]);
                        // i is incremented to navigate to the next module
                        i++;
                        
                        creditsList.Add((Convert.ToInt32(tbCredits.Text)));
                        classHoursList.Add((Convert.ToInt32(tbClassHours.Text)));

                        // show user that their module has been added
                        lblSuccessModule.Visibility = Visibility.Visible;

                    }
                }
            }
            catch (Exception)
            {
                // error message if input is invalid
                MessageBox.Show("Enter a number for Number of credits and Class hours.");
            }
        }

        private void btnDisplayList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // refresh data grid
                dgSelfStudy.Items.Clear();
                // refresh combo box
                cbxModules.Items.Clear();

                // validation 
                if (tbWeeks.Text.Length == 0 || dpStartDate.Text.Length == 0 || dgModuleInfo.Items.Count == 0)
                {
                    MessageBox.Show("Fields cannot be left blank (also make sure that you have added atleast one module).");
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to add this semester?", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        // store user input (number of weeks, start date)
                        int weeks = Convert.ToInt32(tbWeeks.Text);
                        startDateSemester = Convert.ToDateTime(dpStartDate.Text);

                        // show user the semester information that they have added
                        tbSemesterWeeks.Text = Convert.ToString(weeks);
                        tbSemesterStartDate.Text = Convert.ToString(startDateSemester.Date);

                        
                        for (int p = 0; p < modules.Count; p++)
                        {
                            // add module name and self study hours for each module using semester list
                            semesters.Add(new Semester
                            {
                                // ClassLibraryStudy.dll calc method is used to calculate the self study hours for each module
                                 modName = modules[p].modName, selfStudyHours = ClassLibraryStudy.Class1.calc(creditsList[p], weeks, classHoursList[p])
                        });
                            // populate data grid view with elements in semester list
                            dgSelfStudy.Items.Add(semesters[p]);

                            // populate combobox with module names
                            cbxModules.Items.Add(modules[p].modName);
                            
                        } // end for loop

                        // show user that their semester has been added
                        lblSuccessSemester.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {
                // error message if input is invalid
                MessageBox.Show("After you have added a module, enter a number for Number of weeks and choose a date for Start date.");
            }
        }

        private void tbCode_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void tbName_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void tbCredits_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void tbClassHours_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // clear text boxes
            tbClassHours.Text = "";
            tbCode.Text = "";
            tbCredits.Text = "";
            tbName.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // clear text box ad date picker
            tbWeeks.Text = "";
            dpStartDate.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void btnHoursSpentWorking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // refresh data grid
                dgHoursRem.Items.Clear();

                // validation 
                if (tbSemesterWeeks.Text.Length == 0 || cbxModules.Text.Length == 0 || tbHoursWorked.Text.Length == 0 || dpDateWorked.Text.Length == 0)
                {
                    MessageBox.Show("Fields cannot be left blank (also make sure that you have added atleast one module and add a semester).");
                }
                else
                {
                    if (MessageBox.Show("Select Yes to confirm your submission.", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        // store user input (module name, hours spent working, date worked)
                        string mod = cbxModules.Text;
                        int hoursWorking = Convert.ToInt32(tbHoursWorked.Text);
                        workedDateSemester = Convert.ToDateTime(dpDateWorked.Text);

                        // variables created for loop
                        int selfhours = 0;

                        // for loop used to iterate through semesters list
                        for (int j = 0; j < semesters.Count; j++)
                        {
                            // if statement used to find the index of the module chosen in combo box
                            if (semesters[j].modName.Contains(mod) == true)
                            {
                                selfhours = semesters[j].selfStudyHours;
                                
                                // subtract self study hours by user input to make it remaining hours
                                semesters[j].selfStudyHours = selfhours - hoursWorking;
                            }
                        }  // end for loop


                        // using linq to display hours remaining for each module
                        var hoursRem =
                        from s in semesters
                        orderby s.modName, s.selfStudyHours
                        select s;

                        foreach (var item in hoursRem)
                        {
                            dgHoursRem.Items.Add(item);
                        }

                    }
                }
            }
            catch (Exception)
            {
                // error message if input is invalid
                MessageBox.Show("Select a module, type in a number for Number of hours spent working and select a date.");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // close program
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // navigate back to home page
            MainWindow mw = new MainWindow();

            mw.Show();
            this.Close();
        }
    }
}

