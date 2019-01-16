using System;
using System.Data.SqlClient;


namespace zuoye
{
     class Menu
    {
        public void ShowMenu()
        {
            while (true)
            {
                string connString = "Data Source=wdt2019.australiasoutheast.cloudapp.azure.com;Initial Catalog=s3625699;Persist Security Info=True;User ID=s3625699;Password=abc123";
                SqlConnection connection = new SqlConnection(connString);
                //gte connect to slots
                Console.WriteLine(
@"========================================================
Welcome to Appointment Scheduling and Reservation System
========================================================

Main menu:
1. List rooms
2. List slots
3. Staff menu
4. Student menu
5. Exit

Enter an option: ");

                string ch = Console.ReadLine();
                Console.WriteLine();


                if (!int.TryParse(ch, out var option) || !option.IsWithinRange(1, 5))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }
                switch (option)
                {
                    case 1: //List rooms
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
                        ShowMenu();
                        break;

                    case 2: //calling method
                        var listslots = new ListSlots();
                        listslots.ShowListSlots();
                        break;
                    case 3://calling method
                        var staffmenu = new StaffMenu();
                        staffmenu.ShowStaffMenu();
                        break;
                    case 4:////calling method
                        var studentmenu = new StudentMenu();
                        studentmenu.ShowStudentMenu();
                        break;
                    case 5:
                        return;
                    default:
                        throw new Exception();

                }

            }

        }


    }
}
