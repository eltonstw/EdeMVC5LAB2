using System;
using System.Linq;
using System.Collections.Generic;
	
namespace LAB.Models
{   
	public  class 客戶資料檢視表Repository : EFRepository<客戶資料檢視表>, I客戶資料檢視表Repository
	{

	}

	public  interface I客戶資料檢視表Repository : IRepository<客戶資料檢視表>
	{

	}
}