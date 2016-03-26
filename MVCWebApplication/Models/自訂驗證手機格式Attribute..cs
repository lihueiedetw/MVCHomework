using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace MVCWebApplication.Models
{
    public class 自訂驗證手機格式Attribute: DataTypeAttribute
    {
        public 自訂驗證手機格式Attribute() : base(DataType.Text)
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            //has input
            if (value is String)
            {
                //手機格式驗證：前4個數字-後6個數字
                //IsMatch本身就是bool的回傳值，所以利用直接回傳這個值，就可以表示是否已經通過驗證                
                return Regex.IsMatch(value.ToString(), @"\d{4}-\d{6}$");
                
            }            
            else
                return false;
            
        }

    }
}