using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace zuoye
{
     class ListRooms
    {
        public void ShowListRooms() {
            string room = "select * from [Room]";
            connection.Open();
            SqlCommand SC = new SqlCommand(room, connection);
            SqlDataReader testReader = SC.ExecuteReader();
            Console.WriteLine("--- RoomList ---");
            while (testReader.Read())
            {
                Console.WriteLine(@"{0}", testReader["RoomID"]); //Give RoomID a space
            }
            connection.Close();
            Console.WriteLine("Finish");
        }
    }
}
