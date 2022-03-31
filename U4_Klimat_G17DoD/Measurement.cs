using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U4_Klimat_G17DoD
{
    public class Measurement
    {

        Polymorph_Tools po = new Polymorph_Tools();
        Db_Repository db = new Db_Repository();
        public int? id { get; set; }
        public double? value { get; set; }
        public int? observation_id { get; set; }

        public int? category_id { get; set; }


        public override string ToString()
        {
            if (category_id == 10)
            {
                return $"ID: {id} Antal: {value} Observation_ID: {observation_id} Fjällripa med vinterdräkt. Category_ID: {category_id}";
            }
            if (category_id == 11)
            {
                return $"ID: {id} Antal: {value} Observation_ID: {observation_id} Fjällripa med sommardräkt. Category_ID: {category_id}";
            }
            if (category_id == 5)
            {
                return $"ID: {id} Celcius: {value} Observation_ID: {observation_id} Temperatur. Category_ID: {category_id}";
            }
            if (category_id == 8)
            {
                return $"ID: {id} Centimeter: {value} Observation_ID: {observation_id} Snödjup. Category_ID: {category_id}";
            }



            return $"ID: {id} Value: {value} Observation_ID: {observation_id} Category_id: {category_id}";
        }












    }
}
