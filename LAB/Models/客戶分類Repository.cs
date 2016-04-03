using System;
using System.Linq;
using System.Collections.Generic;
	
namespace LAB.Models
{   
	public  class 客戶分類Repository : EFRepository<客戶分類>, I客戶分類Repository
	{
        public 客戶分類 Find(int? id)
        {
            return id.HasValue ? All().FirstOrDefault(c => c.Id == id) : null;
        }

        public override IQueryable<客戶分類> All()
        {
            return this.All(includeDeleted: false);
        }

        public IQueryable<客戶分類> All(bool includeDeleted = false)
        {
            return includeDeleted
                ? base.All()
                : base.All().Where(c => !c.IsDeleted);
        }

        public override void Delete(客戶分類 entity)
        {
            entity.IsDeleted = true;
        }
    }

	public  interface I客戶分類Repository : IRepository<客戶分類>
	{
	    客戶分類 Find(int? id);

        
	}
}