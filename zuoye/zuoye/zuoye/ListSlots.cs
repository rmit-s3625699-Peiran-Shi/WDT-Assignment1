using System;
using System.Data.SqlClient;


namespace zuoye
{
    class ListSlots
    {
        public void ShowListSlots()
        {
            var menu = new Menu();
            string connString = "Data Source=wdt2019.australiasoutheast.cloudapp.azure.com;Initial Catalog=s3625699;Persist Security Info=True;User ID=s3625699;Password=abc123";
            SqlConnection connection = new SqlConnection(connString);
            Console.WriteLine("--- List Slots ---");
            Console.WriteLine("Enter date (yyyy-mm-dd): ");
            string date = Console.ReadLine();
            if (DateTime.TryParse(date, out DateTime time))
            {
                Console.WriteLine("Slot on " + date + ":");
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("select StartTime from [Slot] where StartTime='" + date + "'", connection);
                SqlDataReader mysdr = sqlCommand.ExecuteReader();
                if (mysdr.HasRows)
                {
                    connection.Close();
                    connection.Open();
                    SqlCommand SC = new SqlCommand("select RoomID,StartTime,StaffID,BookedInStudentID from [Slot]", connection);
                    SqlDataReader listreader = SC.ExecuteReader();
                    while (listreader.Read())
                    {
                        Console.WriteLine("Room name            Start time               Staff ID              Bookings");
                        Console.WriteLine("{0}                  {1}                      {2}                   {3}", listreader["RoomID"], listreader["StartTime"], listreader["StaffID"], listreader["BookedInStudentID"]);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date.");
                    connection.Close();
                    {
                        menu.ShowMenu();
                    }
                }
            }
            else
            {
                Console.WriteLine("Wrong date type. Please try again.");
                ShowListSlots();
            }
        }
    }
}
