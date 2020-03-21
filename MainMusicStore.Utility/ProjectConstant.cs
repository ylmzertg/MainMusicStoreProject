using System;
using System.Collections.Generic;
using System.Text;

namespace MainMusicStore.Utility
{
    public static class ProjectConstant
    {
        public const string ResultNotFound = "Data Not Found";

        //----------------------------------------------------------//
        public const string Proc_CoverType_GetAll = "usp_GetCoverTypes";
        public const string Proc_CoverType_Get = "usp_GetCoverType";
        public const string Proc_CoverType_Delete = "usp_DeleteCoverType";
        public const string Proc_CoverType_Create = "usp_CreateCoverType";
        public const string Proc_CoverType_Update = "usp_UpdateCoverType";
        //----------------------------------------------------------//

        public const string Role_User_Indi = "Individual Customer";
        public const string Role_User_Comp = "Company Customer";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
        //----------------------------------------------------------//

        public const string shoppingCart = "ShoppingCart";

        //PaymentStatus----------------------------------------------------------//

        public const string PaymentStatusPending = "Pending";


        //OrderStatus----------------------------------------------------------//

        public const string StatusPending = "Pending";

        //---------------------------------------------------------------------//


        public static double GetPriceBaseOnQuantity(int quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
                return price;
            else
            {
                if (quantity < 100)
                    return price50;
                else
                    return price100;
            }
        }

        public static string ConvertToRawHtml(string description)
        {
            char[] array = new char[description.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < description.Length; i++)
            {
                char let = description[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
