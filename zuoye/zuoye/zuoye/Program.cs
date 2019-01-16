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
    public static class Program
    {
        public static bool IsWithinRange(this int value, int min, int max) => value >= min && value <= max;
        //Reference: InventoryPriceManagement
        public static object ListRoom { get; private set; }
        public static object ListSlots { get; private set; }
        public static object StaffMenu { get; private set; }
        public static object StudentMenu { get; private set; }

        static void Main(string[] args)
        {
            var menu = new Menu();
            menu.ShowMenu();

        }


    }
}