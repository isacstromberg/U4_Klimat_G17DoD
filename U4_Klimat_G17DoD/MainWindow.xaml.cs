using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace U4_Klimat_G17DoD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Db_Repository db = new Db_Repository();
        Polymorph_Tools po = new Polymorph_Tools();
        // Deklarar dessa ^ här för att slippa deklarera vid varje interaktion. Nu kan man ropa på tex Polymorph_Tools överallt i gränssnittet utan att behöva deklarera igen.
        // Man måste kunna välja båda alternativen. Vid val av fjällripa måste man välja päls / eller hare







        public MainWindow()
        {
            InitializeComponent();
            var db = new Db_Repository();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //lägg till observatör
            // Använder metoden db.AddObserver() för att lägga till observatör. Metoden returnerar även en bool, detta för att intyga om det gått bra eller dåligt.


            string Firstname_parameter = Firstname_input.Text;
            string Lastname_parameter = Lastname_input.Text;


            if (db.BlockNumbers(Firstname_parameter) == false)
            {
                MessageBox.Show("Du får enbart ge ett namn bestående av bokstäver");
                return;
            }

            if (db.BlockNumbers(Lastname_parameter) == false)
            {
                MessageBox.Show("Du får enbart ge ett namn bestående av bokstäver");
                return;
            }

            if (db.AddObserver(Firstname_parameter, Lastname_parameter) == true)
            {
                MessageBox.Show("Det gick bra den nya observatören har nu lagts till!!");
            }
            else
            {
                MessageBox.Show("Det gick inte bra, du måste ange rätt format. förnamn och efternamn måste bestå av text");
            }






        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            // Hämtar alla observatörer från min databas och presenterar dem i listbox, utan ID. ID hämtas men presenteras inte.
            // db.CheckIfObserverDBEmpty() använder för att kontrollera om antalet observatörer är noll, detta för att ge feedback om det gått dåligt.

            ListBox_Observer.ItemsSource = null;
            try
            {
                if (db.CheckIfObserverDBEmpty() == true)
                {
                    MessageBox.Show("Databasen är tömd på observatörer!!! Fyll på med nya observatörer och hämta observatörerna igen."); return;
                }
                ListBox_Observer.ItemsSource = db.GetObservers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }


        public void ListBox_Observer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // po.SetSelectedObserverValues() sätter nya värden vid dubbelklick på våra sk "polymorph" Observer-strängar som används som parametrar i olika metoder, meddelanden och listboxar.
            var ob = new Observer();
            Observer observer = ListBox_Observer.SelectedItem as Observer;

            db.observers.Add(observer);
            po.SetSelectedObserverValues(observer.firstname, observer.lastname, observer.id); lblObserver_Name.Content = " "; SelectUserInfo.Visibility = Visibility.Hidden;
            LstMeasurment.ItemsSource = null;
            lstObservations.ItemsSource = null;
            MessageBox.Show($"Du har nu valt {po.SelectedFirstName} {po.SelectedLastName} med id {po.SelectedObserverID}");
        }


        public void LstMeasurment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Här används ingen "Polymorph-metod" för att ändra värdet på klickad measurement utan här ändras polymorph int po.SelectedMeasurementID direkt i gränssnittet.
            var ob = new Observation();
            Measurement measurement = LstMeasurment.SelectedItem as Measurement;
            po.SelectedMeasurementID = measurement.id;
            po.SelectedMeasurementCategory_ID = measurement.category_id;
            po.SelectedValue = measurement.value;
            TxtEditValue.Text = $"Ange nytt mätvärde för mätpunkt {po.SelectedMeasurementID}";
            MessageBox.Show($"Du har nu valt att redigera mätpunkten med ID: {po.SelectedMeasurementID}.");
        }
        public void lstObservations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Här sätter "polymorph" metoden pb.SetSelectedObserverValues nya värden på "Polymorph properties" vid dubbelklick. Dessa används sedan som parametrar och meddelanden.

            var ob = new Observation();
            var db = new Db_Repository();
            Observation observation = lstObservations.SelectedItem as Observation;
            db.observations.Add(observation);
            po.SetSelectedObserverValues(observation.id, observation.date, observation.observer_id, observation.geolocation_id);
            db.GetMeasurementObservation_id();
            foreach (var item in db.GetMeasurementObservation_id())
            {
                if (item.observation_id == po.SelectedObservationID)
                {
                    po.SelectedObservationID = item.observation_id;
                }
            }
            string SelectedObservationIDString = po.SelectedObservationID.ToString();
            LstMeasurment.ItemsSource = null;
            LstMeasurment.ItemsSource = db.GetObservationReportedMeasurements(po.SelectedObservationID);
            TxtEditValue.Text = $"Du har nu valt att redigera mätpunkter från observation med ID: {SelectedObservationIDString}. Välj mätpunkt via dubbelklick för redigering";
            MessageBox.Show($"Du har nu valt observation {po.SelectedObservationID} {po.SelectedObservationDate} med observatören {po.SelectedObserver_ID}");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            bool ObserverToBeRemovedIsSignedIn = false;

            if (db.BlockLetters(Txt_Observer_ID.Text) == false)
            {
                MessageBox.Show("Du får enbart ange siffror");
                return;
            }
            int ID_parameter = Convert.ToInt32(Txt_Observer_ID.Text);
            string ID_parameter_string = ID_parameter.ToString();

            if (db.CheckIfObserverHasDoneObservation(ID_parameter) == true)
            {
                MessageBox.Show("Stopp! Denna observatör har genomfört observationer och kan därför inte tas bort."); return;
            }
            if (db.CheckIfInput_ID_MatchesObserverID(ID_parameter) == false)
            {
                MessageBox.Show("Stopp! Det finns ingen observatör i databasen som har det ID du angett"); return;
            }
            if (ID_parameter == po.SelectedObserverID)
            {
                ObserverToBeRemovedIsSignedIn = true;
                try
                {
                    db.RemoveObserver(ID_parameter);
                    MessageBox.Show($"Det gick bra, observatören med ID {ID_parameter_string} är nu bortagen! OBS du har raderat observatören som använts för att logga in, du kommer därför loggas ut. Logga in igen om du vill fortsätta använda appen.");
                    po.SelectedObserverID = 0;
                    lstObservations.ItemsSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }

            if (ObserverToBeRemovedIsSignedIn == false)
            {

                try
                {
                    db.RemoveObserver(ID_parameter);
                    MessageBox.Show($"Det gick bra, observatören med ID {ID_parameter_string} är nu bortagen!");
                    lstObservations.ItemsSource = null;
                    lstObservations.ItemsSource = db.GetObserverReportedObservation(po.SelectedObserverID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }

            ObserverToBeRemovedIsSignedIn = false;



        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {


            // db.SelectGeolocation() slumpar en geolocation. OBS om man raderar någon geolocation i databasen som används i programmet kraschar systemet. 
            // Att felsäkra detta prioriterades bort eftersom det inte var ett krav. Citat U4, konstruera applikation - "En observation ska ha en geografisk plats.  
            //Denna del av programmet behöver ni inte utveckla, plocka några av de testdata ni lagrat" slutcitat.
            // Metoden db.CatchCreatedObservationID(); hämtar det sista ID nummret i alla observationer, därmed den senast skapade. Används sedan som parameter.

            if (po.SelectedObserverID == 0)
            {
                MessageBox.Show("Du måste välja en observatör för att kunna registrera observation"); return;
            }
            if (ChkB_Fjällripa.IsChecked == false && ChkB_Snödjup.IsChecked == false && ChkB_Temperatur.IsChecked == false)
            {
                MessageBox.Show("För att kunna registrera en klimatobservation så måste du välja minst en kategori.");
                return;
            }

            //if (ChkB_Fjällripa.IsChecked == false && radSommardräkt == true)
            //{

            //}

            if (ChkB_Snödjup.IsChecked == true)
            {
                if (db.BlockLetters(txtValue.Text) == false)
                {
                    MessageBox.Show("Du får enbart ange siffror");
                    return;
                }

                double Value_Parameter = Convert.ToDouble(txtValue.Text);

                if (Value_Parameter < 0)
                {
                    MessageBox.Show("Om en klimatobservation med snödjup ska lämnas in måste snödjupet ha ett positivt värde!");
                    return;
                }
            }

            if (ChkB_Temperatur.IsChecked == true)
            {
                if (db.BlockLetters(txtValue_2.Text) == false)
                {
                    MessageBox.Show("Du får enbart ange siffror");
                    return;
                }
                double Value2_Parameter = Convert.ToDouble(txtValue_2.Text);
            }

            if (ChkB_Fjällripa.IsChecked == true && radSommardräkt.IsChecked == true)
            {
                if (db.BlockLetters(txt_Value3.Text) == false)
                {
                    MessageBox.Show("Du får enbart ange siffror");
                    return;
                }
                int Value3_Parameter = Convert.ToInt32(txt_Value3.Text);
                if (Value3_Parameter < 0)
                {
                    MessageBox.Show("Om en klimatobservation med fjällripor ska lämnas in måste antalet ha ett positivt värde!");
                    return;
                }
            }
            if (ChkB_Fjällripa.IsChecked == true && radVinterdräkt.IsChecked == true)
            {
                if (db.BlockLetters(txt_Value3.Text) == false)
                {
                    MessageBox.Show("Du får enbart ange siffror");
                    return;
                }
                int Value3_Parameter_2 = Convert.ToInt32(txt_Value3.Text);
                if (Value3_Parameter_2 < 0)
                {
                    MessageBox.Show("Om en klimatobservation med fjällripor ska lämnas in måste antalet ha ett positivt värde!");
                    return;
                }
            }

            if (ChkB_Fjällripa.IsChecked == true && radSommardräkt.IsChecked == false && radVinterdräkt.IsChecked == false)
            {
                MessageBox.Show("Om du ska registrera en observation med fjällripor måste du välja dräkt!");
                return;
            }

            if (ChkB_Fjällripa.IsChecked == false && radSommardräkt.IsChecked == true)
            {
                MessageBox.Show("Om du vill ange vilket dräkt fjällripan har måste du först markera att en fjällripa observerats."); return;
            }
            if (ChkB_Fjällripa.IsChecked == false && radVinterdräkt.IsChecked == true)
            {
                MessageBox.Show("Om du vill ange vilket dräkt fjällripan har måste du först markera att en fjällripa observerats."); return;
            }

            double? GetGeolocation = db.SelectGeolocation();

            db.RegisterObservation(po.SelectedObserverID, GetGeolocation);
            db.CatchCreatedObservationID();





            if (ChkB_Snödjup.IsChecked == true)
            {
                double Value_Parameter = Convert.ToDouble(txtValue.Text);
                db.RegisterMeasurement(Value_Parameter, db.CatchCreatedObservationID(), 8);
            }

            if (ChkB_Temperatur.IsChecked == true)
            {
                double Value2_Parameter = Convert.ToDouble(txtValue_2.Text);
                db.RegisterMeasurement(Value2_Parameter, db.CatchCreatedObservationID(), 5);
            }
            if (ChkB_Fjällripa.IsChecked == true && radSommardräkt.IsChecked == true)
            {
                int Value3_Parameter = Convert.ToInt32(txt_Value3.Text);
                db.RegisterMeasurement(Value3_Parameter, db.CatchCreatedObservationID(), 11);
            }
            if (ChkB_Fjällripa.IsChecked == true && radVinterdräkt.IsChecked == true)
            {
                int Value3_Parameter_2 = Convert.ToInt32(txt_Value3.Text);
                db.RegisterMeasurement(Value3_Parameter_2, db.CatchCreatedObservationID(), 10);
            }

            MessageBox.Show("Det gick bra, en ny observation har nu skapats");



        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = db.GetObserverReportedObservation(po.SelectedObserverID);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // db.BlockLetters felsäkrar programmet igenom att inte tillåta strängar.Skulle metoden inte köras skulle programmet krascha.
            // När metoden db.UpdateObservationMeasurement() körs.

            if (db.BlockLetters(TxtEditValue.Text) == false)
            {
                MessageBox.Show("Du får enbart ange siffror");
                return;
            }
            if ((po.SelectedMeasurementCategory_ID == 10))
            {
                if (Convert.ToInt32(TxtEditValue.Text) < 0)
                {
                    MessageBox.Show("Du kan inte ange ett negativt mätvärde vid observation av antal fjällripor");
                    return;
                }
            }

            if ((po.SelectedMeasurementCategory_ID == 11))
            {
                if (Convert.ToInt32(TxtEditValue.Text) < 0)
                {
                    MessageBox.Show("Du kan inte ange ett negativt mätvärde vid observation av antal fjällripor");
                    return;
                }
            }

            if ((po.SelectedMeasurementCategory_ID == 8))
            {
                if (Convert.ToInt32(TxtEditValue.Text) < 0)
                {
                    MessageBox.Show("Du kan inte ange ett negativt mätvärde vid observation av cm snödjup");
                    return;
                }
            }

            double EditedValue = Convert.ToDouble(TxtEditValue.Text);
            db.UpdateObservationMeasurement(EditedValue, po.SelectedMeasurementID);

            LstMeasurment.ItemsSource = null;
            LstMeasurment.ItemsSource = db.GetObservationReportedMeasurements(po.SelectedObservationID);
            MessageBox.Show($"Det gick bra, mätningsvärdet i mätpunkt med ID: {po.SelectedMeasurementID}  har ändrats från {po.SelectedValue} till {EditedValue}");

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (po.SelectedMeasurementCategory_ID == 10)
            {
                db.UpdateFjallripaSuit(11, po.SelectedMeasurementID);
                LstMeasurment.ItemsSource = null;
                LstMeasurment.ItemsSource = db.GetObservationReportedMeasurements(po.SelectedObservationID);
                MessageBox.Show("Det gick bra, fjällripan/fjällripornas dräkt är nu ändrad till sommardräkt");
            }
            if (po.SelectedMeasurementCategory_ID == 11)
            {
                db.UpdateFjallripaSuit(10, po.SelectedMeasurementID);
                LstMeasurment.ItemsSource = null;
                LstMeasurment.ItemsSource = db.GetObservationReportedMeasurements(po.SelectedObservationID);
                MessageBox.Show("Det gick bra, fjällripan/fjällripornas dräkt är nu ändrad till vinterdräkt");
            }
            if (po.SelectedMeasurementCategory_ID > 11 || po.SelectedMeasurementCategory_ID < 10)
            {
                MessageBox.Show("Du kan bara byta dräkt på en mätningspunkt som är av typen fjällripa!!"); return;
            }


            po.SelectedMeasurementCategory_ID = 0;
            LstMeasurment.ItemsSource = null;
            LstMeasurment.ItemsSource = db.GetObservationReportedMeasurements(po.SelectedObservationID);
            TxtEditValue.Text = "Klicka på en mätpunkt för att påbörja redigering";

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            ChkB_Fjällripa.IsChecked = false;
            ChkB_Snödjup.IsChecked = false;
            ChkB_Temperatur.IsChecked = false;
            radSommardräkt.IsChecked = false;
            radVinterdräkt.IsChecked = false;

        }

        private void SelectUserInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Du väljer observatör igenom att dubbelklicka på en hämtad observatör");
        }
    }
}
