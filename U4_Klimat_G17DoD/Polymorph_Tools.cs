using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U4_Klimat_G17DoD
{
    public class Polymorph_Tools
    {
        //Polymorph verktyg för observer
        public int? SelectedObserverID = 0;
        public string SelectedFirstName = "";
        public string SelectedLastName = "";
        public string SelectedFullName = "";

        //Polymorph verktyg för observation
        public int? SelectedObservationID = 0;
        public DateTime? SelectedObservationDate;
        public int? SelectedObserver_ID = 0;
        public int? SelectedGeolocation_ID = 0;

        // Polymorph verktyg för measurement
        public int? SelectedMeasurementID = 0;
        public double? SelectedValue = 0;
        public int? SelectedObservation_ID = 0;
        public int? SelectedMeasurementCategory_ID = 0;




        public void SetSelectedObserverValues(string SetFirstName, string SetLastName, int? SetId)
        {
            //Metod som sätter alla nya värden vid dubbelklick
            SelectedFirstName = SetFirstName;
            SelectedLastName = SetLastName;
            SelectedFullName += SetFirstName;
            SelectedFullName += " ";
            SelectedFullName += SetLastName;
            SelectedObserverID = SetId;
        }

        public void SetSelectedObserverValues(int? SetId, DateTime? SetDate, int? Observer_ID, int? Geolocation_ID)
        {
            //Metod som sätter alla nya värden vid dubbelklick
            SelectedObservationID = SetId;
            SelectedObservationDate = SetDate;
            SelectedObserver_ID = Observer_ID;
            SelectedGeolocation_ID = Geolocation_ID;
        }






    }
}
