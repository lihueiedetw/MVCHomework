using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVCWebApplication.Models
{   
	public  class 客戶資料VIEWRepository : EFRepository<客戶資料VIEW>, I客戶資料VIEWRepository
	{

	}

	public  interface I客戶資料VIEWRepository : IRepository<客戶資料VIEW>
	{

	}
}