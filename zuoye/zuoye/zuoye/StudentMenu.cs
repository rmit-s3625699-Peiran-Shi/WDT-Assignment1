using System;
using System.Data;
using System.Data.SqlClient;

namespace zuoye
{
    class StudentMenu
    {
        public void ShowStudentMenu()
        {
            var menu = new Menu();
            string connString = "Data Source=wdt2019.australiasoutheast.cloudapp.azure.com;Initial Catalog=s3625699;Persist Security Info=True;User ID=s3625699;Password=abc123";
            SqlConnection connection = new SqlConnection(connString);
            while (true)
            {
                Console.WriteLine(@"
Student menu:
================
1. Staff availability
2. List students
3. Make Booking
4. Cancel Booking
5. Exit

Enter an option: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out var list) || !list.IsWithinRange(1, 5))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                }
                switch (list)
                {

                    case 1: //Staff availability
                        Console.WriteLine("--- Staff Availability ---");
                        Console.WriteLine("Enter date for staff availability (dd-mm-yyyy): ");
                        string date = Console.ReadLine();
                        Console.WriteLine("Enter staff ID: ");
                        string ID = Console.ReadLine();
                        connection.Open();
                        SqlCommand cmde = new SqlCommand("select * from [Slot] where StartTime='" + date + "'and StaffID='" + ID + "'", connection);
                        SqlDataReader studentReader = cmde.ExecuteReader();

                        if (studentReader.HasRows)
                        {
                            while (studentReader.Read())
                            {
                                Console.WriteLine(
@"Staff" + ID + "availability on " + date + @":
Room name           Start time          End time  
{0}                 {1}              Within 1 hour ", studentReader["RoomID"], studentReader["StartTime"]);
                            }
                            connection.Close();
                            ShowStudentMenu();
                        }
                        else
                        {
                            Console.WriteLine("No Staff available. Please try another time.");
                            ShowStudentMenu();
                        }
                        break;
                    case 2: //List students
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("select UserID,Name,Email from [User] where len(UserID)=8", connection);
                        SqlDataReader testReader = cmd.ExecuteReader();
                        Console.WriteLine("--- List Students ---");
                        Console.WriteLine("\tID                  Name                Email");
                        while (testReader.Read())
                        {
                            Console.WriteLine("{0};      {1};      {2}", testReader["UserID"], testReader["Name"], testReader["Email"]);

                        }
                        connection.Close();
                        ShowStudentMenu();
                        break;
                    case 3: //Make Booking
                        Console.WriteLine("---  Make booking  ---");
                        Console.Write("Enter room ID (A/B/C/D):");
                        string roomName = Console.ReadLine();
                        Console.Write("Enter date for slot (yyyy-dd-mm):");
                        string dateSlot = Console.ReadLine();

                        Console.Write("Enter time for slot (hh:mm):");
                        string timeSlot = Console.ReadLine();
                        DateTime dt = Convert.ToDateTime(dateSlot + " " + timeSlot + ":00");
                        connection.Open();
                        string IDT = "select RoomID,StartTime from [Slot] where RoomID = '" + roomName + "' and StartTime = '" + dt + "'";
                        SqlCommand SCid = new SqlCommand(IDT, connection);
                        SqlDataReader mysdr1 = SCid.ExecuteReader();
                        if (mysdr1.HasRows)
                        {
                            connection.Close();
                            Console.Write("Enter student ID:");
                            string stuID = Console.ReadLine();
                            connection.Open();
                            string IDT1 = "select UserID from [User] where len(UserID)=8";
                            SqlCommand userID = new SqlCommand(IDT1, connection);
                            SqlDataReader UserID = userID.ExecuteReader();
                            if (UserID.HasRows)
                            {
                                connection.Close();
                                connection.Open();
                                string IDT2 = "select BookedInStudentID from [Slot] where StartTime = '" + dateSlot + "'and BookedInStudentID = '" + stuID + "'";
                                SqlCommand SC3 = new SqlCommand(IDT2, connection);
                                SqlDataReader mysdr2 = SC3.ExecuteReader();
                                if (mysdr2.HasRows)
                                {
                                    connection.Close();
                                    Console.Write("Invalid.");
                                    ShowStudentMenu();
                                }
                                else
                                {
                                    connection.Close();
                                    try
                                    {
                                        connection.Open();
                                        string test1 = "select BookedInStudentID from [Slot] where StartTime = '" + dt + "' and RoomID = '" + roomName + "'";
                                        SqlCommand test2 = new SqlCommand(test1, connection);
                                        SqlDataReader check3 = test2.ExecuteReader();
                                        if (check3.HasRows)
                                        {
                                            connection.Close();
                                            Console.Write("This slot is already booked. Please try another one.");
                                            ShowStudentMenu();
                                        }
                                        else
                                        {
                                            connection.Close();
                                            connection.Open();
                                            SqlCommand cm35 = new SqlCommand("UPDATE [Slot] SET BookedInStudentID ='" + stuID + "' WHERE StartTime ='" + dt + "'", connection);
                                            cm35.ExecuteNonQuery();
                                            Console.Write(@"

   Slot booked successfully.");
                                            connection.Close();
                                            ShowStudentMenu();
                                        }
                                    }
                                    catch (Exception test)
                                    {
                                        Console.WriteLine(test);
                                    }
                                }
                            }
                            else
                            {
                                connection.Close();
                                Console.Write("Invalid ID.");
                                ShowStudentMenu();
                            }
                        }
                        else
                        {
                            connection.Close();
                            Console.Write("Invalid.");
                            ShowStudentMenu();
                        }
                        break;
                    case 4: //Cancel Booking
                        Console.WriteLine("--- Cancel booking ---");
                        Console.Write("Enter room name:");
                        String cname = Console.ReadLine();
                        Console.Write("Enter date for slot (yyyy-dd-mm):");
                        String cdate = Console.ReadLine();
                        Console.Write("Enter time for slot (hh:mm):");
                        String ctime = Console.ReadLine();
                        DateTime cslot = Convert.ToDateTime(cdate + " " + ctime);
                        connection.Open();
                        SqlCommand cm5 = new SqlCommand("select RoomID,StartTime from [Slot] where RoomID = '" + cname + "' and StartTime = '" + cslot + "'", connection);
                        SqlDataReader mysdr4 = cm5.ExecuteReader();
                        if (mysdr4.HasRows)
                        {
                            connection.Close();
                            connection.Open();
                            string str = "UPDATE [Slot] SET BookedInStudentID = @BookedInStudentID where RoomID = '" + cname + "' and StartTime = '" + cslot + "'";
                            SqlParameter[] spar = new SqlParameter[]
                            {
                                new SqlParameter("@BookedInStudentID", SqlDbType.NVarChar,8)};
                            spar[0].Value = DBNull.Value;
                            SqlCommand comm = new SqlCommand(str, connection);
                            comm.Parameters.AddRange(spar);
                            int result = comm.ExecuteNonQuery();
                            Console.Write("Booking cancelled successfully.");
                            ShowStudentMenu();
                        }
                        else
                        {
                            Console.Write("Invalid");
                            ShowStudentMenu();
                        }
                        connection.Close();
                        break;
                    case 5:
                        menu.ShowMenu();
                        break;
                    default:
                        throw new Exception();
                }

            }
        }
        }
    }

