namespace MVCWebApplication.Models
{
    using Controllers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶聯絡人Controller 客戶聯絡人Con = new 客戶聯絡人Controller();

            //驗證Email是否有重複  
            bool ret = 客戶聯絡人Con.驗證聯絡Email重複(this.Id, this.客戶Id, this.Email);
            if (ret)  //表示重複
            {
                yield return new ValidationResult("同一客戶中，聯絡人Email重複!", new string[] { "Email" });
            }            
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress(ErrorMessage = "Email格式不正確，請重新輸入")]        
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [自訂驗證手機格式(ErrorMessage = "手機格式不正確，需為此格式，如：0911-123456")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
