using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace zuoye
{
    class StaffMenu
    {
        public void ShowStaffMenu()
        {
            var menu = new Menu();
            string connString = "Data Source=wdt2019.australiasoutheast.cloudapp.azure.com;Initial Catalog=s3625699;Persist Security Info=True;User ID=s3625699;Password=abc123";
            SqlConnection connection = new SqlConnection(connString);
            while (true)
            {
                Console.WriteLine(
@"Staff menu:
================
1. Room availability
2. List staff
3. Create slot
4. Remove slot
5. Exit

Enter an option: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out var list) || !list.IsWithinRange(1, 5))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    break;
                }

                switch (list)
                {
                    case 1: //Room availability
                        Console.WriteLine("--- Room Availability ---");
                        Console.Write("Please enter date for slot (yyyy-mm-dd):");
                        String roomava = Console.ReadLine();
                        DateTime time2;
                        if ((DateTime.TryParse(roomava, out time2)))
                        {
                            Console.WriteLine(@"
Rooms available on:" + roomava);
                            connection.Open();
                            string roomID = "select RoomID from [Room] where RoomID in (select RoomID from [Slot] where  StartTime like '%\" + date2 + \"%' group by RoomID having count(*) < 2 )  or RoomID in (select RoomID from [Slot] where StartTime not like '%\" + roomava + \"%' group by RoomID ) or RoomID in (SELECT RoomID FROM [Room] WHERE RoomID  NOT IN(SELECT RoomID FROM [Slot]))";
                            SqlCommand Ava = new SqlCommand(roomID, connection);
                            SqlDataReader roomReader = Ava.ExecuteReader();
                            //check room availity
                            Console.WriteLine("Room Name: ");
                            while (roomReader.Read())
                            {
                                Console.WriteLine(@"{0}", roomReader["RoomID"]);
                            }
                            connection.Close();
                            Console.WriteLine("Finish");
                            ShowStaffMenu();
                        }
                        else
                        {
                            Console.WriteLine("Room is not avaliable now. Please try another one.");
                            ShowStaffMenu();
                        }
                        break;
                    case 2: //List staff
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("select UserID,Name,Email from [User] where len(UserID)=6", connection);
                        SqlDataReader staffReader = cmd.ExecuteReader();
                        //check staff ID(start with e and length=6)
                        Console.WriteLine("--- List Staff ---");
                        Console.WriteLine("\tID                  Name                Email");
                        while (staffReader.Read())
                        {
                            Console.WriteLine("{0};      {1};      {2}", staffReader["UserID"], staffReader["Name"], staffReader["Email"]);

                        }
                        connection.Close();
                        ShowStaffMenu();
                        break;
                    case 3: //Create slot
                        Console.WriteLine("--- Create Slot ---");
                        Console.Write("Please give a name: ");
                        string c3 = Console.ReadLine();
                        connection.Open();
                        SqlCommand cm = new SqlCommand("select RoomID from [Room] where RoomID='" + c3 + "'", connection);
                        SqlDataReader mysdr = cm.ExecuteReader();
                        if (mysdr.HasRows)
                        {
                            connection.Close();
                            Console.Write("Enter date for slot (dd-mm-yyyy):");
                            String date = Console.ReadLine();
                            DateTime time;
                            if ((DateTime.TryParse(date, out time)))
                            {
                                Console.Write("Enter time for slot (hh:mm):");
                                String min = Console.ReadLine();
                                Regex reg = new Regex("^[0-1][0-4]:[0-5][0-9]$");
                                Match IDr = reg.Match(min);
                                if (IDr.Success)
                                {
                                    Console.Write("Enter Staff ID:");
                                    string ID = Console.ReadLine();
                                    connection.Open();
                                    SqlCommand IDs = new SqlCommand("select UserID from [User] where RoomID='" + ID + "'", connection);
                                    SqlDataReader IDc = cm.ExecuteReader();
                                    if (IDc.HasRows)
                                    {
                                        connection.Close();
                                        connection.Open();
                                        DateTime DT = Convert.ToDateTime(date + " " + min);
                                        SqlCommand cmdes = new SqlCommand("insert into [Slot](RoomID,StartTime,StaffID) values ('" + c3 + "','" + DT + "','" + ID + "')", connection);
                                        cmdes.ExecuteNonQuery();
                                        Console.Write(@"

Successful");
                                        connection.Close();
                                    }
                                    else
                                    {
                                        Console.Write("False");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Unavaliable time");
                                    ShowStaffMenu();
                                }

                            }
                            else
                            {
                                Console.WriteLine("Unavaliable time");
                                ShowStaffMenu();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unavaliable Room ID. Please select from (A/B/C/D)");
                            ShowStaffMenu();
                        }
                        break;
                    case 4: //Remove slot
                        Console.WriteLine("--- Remove Slot ---");
                        connection.Open();
                        Console.Write("Enter room name:");
                        string RM = Console.ReadLine();
                        SqlCommand cmed4 = new SqlCommand("select RoomID from [Room] where RoomID='" + RM + "'", connection);
                        SqlDataReader mysdre4 = cmed4.ExecuteReader();
                        if (mysdre4.HasRows)
                        {
                            connection.Close();
                            Console.Write("Enter date for slot (dd-mm-yyyy):");
                            String remove = Console.ReadLine();
                            DateTime remove1;
                            if ((DateTime.TryParse(remove, out remove1)))
                            {
                                Console.Write("Enter time for slot (hh:mm):");
                                String remove2 = Console.ReadLine();
                                Regex regex = new Regex("^[0-1][0-4]:[0-5][0-9]$");
                                Match match = regex.Match(remove2);
                                if (match.Success)
                                {
                                    DateTime DT = Convert.ToDateTime(remove + " " + remove2);
                                    connection.Open();
                                    SqlCommand remove3 = new SqlCommand("select BookedInStudentID from [Slot] where RoomID = '" + RM + "'and StartTime = '" + DT + "'", connection);
                                    SqlDataReader idreader = remove3.ExecuteReader();
                                    if (idreader.HasRows)
                                    {
                                        connection.Close();
                                        Console.Write("Invalid option");
                                        ShowStaffMenu();
                                    }
                                    else
                                    {
                                        connection.Close();
                                        connection.Open();
                                        SqlCommand delet = new SqlCommand("delete from [Slot] where RoomID = '" + RM + "' and StartTime ='" + DT + ":00" + "' ", connection);
                                        delet.ExecuteNonQuery();
                                        Console.Write("Slot removed successfully.");
                                        connection.Close();
                                        ShowStaffMenu();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Unavaliable time");
                                    ShowStaffMenu();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unavaliable time");
                                ShowStaffMenu();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unavaliable Room ID. Please select from (A/B/C/D)");
                            ShowStaffMenu();
                        }
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