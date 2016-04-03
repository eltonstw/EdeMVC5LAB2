using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LAB.Models;

namespace LAB.Controllers
{
    public class 客戶資料Controller : BaseController
    {

        // GET: 客戶資料
        public ActionResult Index()
        {
            var 客戶資料 = RepoCust.All().Include(客 => 客.客戶分類);
            return View(客戶資料.ToList());
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 cust = RepoCust.Find(id);
            if (cust == null)
            {
                return HttpNotFound();
            }

            return View(cust);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類Id = new SelectList(RepoCategory.All(), "Id", "CategoryName");
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                RepoCust.Add(客戶資料);
                RepoCust.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類Id = new SelectList(RepoCategory.All(), "Id", "CategoryName", 客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = RepoCust.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶分類Id = new SelectList(RepoCategory.All(), "Id", "CategoryName", 客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                RepoCust.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                RepoCust.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類Id = new SelectList(RepoCategory.All(), "Id", "CategoryName", 客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = RepoCust.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = RepoCust.Find(id);
            RepoCust.Delete(客戶資料);
            RepoCust.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Export()
        {
            using (var ms = ExcelUtility.RenderListToExcel(RepoCust.All().Include(c => c.客戶分類).Select(c => new
            {
                客戶名稱 = c.客戶名稱,
                統一編號 = c.統一編號,
                電話 = c.電話,
                傳真 = c.傳真,
                地址 = c.地址,
                Email = c.Email,
                CategoryName = c.客戶分類.CategoryName
            }).ToList()))
            {
                return File((ms as MemoryStream).ToArray(), "application/vnd.ms-excel", "export.xls");
            }

        }

        [HttpPost]
        public ActionResult Details(IList<BatchUpdateContact> data)
        {
            if (data == null || !data.Any())
            {
                return RedirectToAction("Details");
            }

            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var c = RepoContact.Find(item.Id);
                    c.手機 = item.手機;
                    c.電話 = item.電話;
                    c.職稱 = item.職稱;
                    
                }

                RepoContact.UnitOfWork.Commit();
                return RedirectToAction("Details");
            }

            
            ViewData.Model = RepoContact.Find(data[0].Id).客戶資料;
            return View();
        }
    }
}
