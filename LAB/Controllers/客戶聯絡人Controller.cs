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
    public class 客戶聯絡人Controller : BaseController
    {
        [HandleError(ExceptionType = typeof(ArgumentNullException), View = "ErrorNull")]
        // GET: 客戶聯絡人
        public ActionResult Index(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException();
            }

            if (s == "null")
            {
                throw new NoNullAllowedException();
            }

            
            var 客戶聯絡人 = RepoContact.All().Include(客 => 客.客戶資料);
            return View(客戶聯絡人.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = RepoContact.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(RepoCust.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,IsDeleted")] 客戶聯絡人 客戶聯絡人)
        {
            //
            if (ModelState.IsValid)
            {
                RepoContact.Add(客戶聯絡人);
                RepoContact.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(RepoCust.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = RepoContact.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(RepoCust.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var contact = RepoContact.Find(id);

            // "Id,客戶Id,職稱,姓名,Email,手機,電話,IsDeleted"
            //if (ModelState.IsValid)
            if (TryUpdateModel(contact, "職稱,姓名,Email,手機,電話".Split(',')))
            {
                //RepoContact.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                RepoContact.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(RepoCust.All(), "Id", "客戶名稱", contact.客戶Id);
            return View(contact);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = RepoContact.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = RepoContact.Find(id);
            RepoContact.Delete(客戶聯絡人);
            RepoContact.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Export()
        {
            using (var ms = ExcelUtility.RenderListToExcel(RepoContact.All().Include(c => c.客戶資料).Select(c => new
            {
                職稱 = c.職稱,
                姓名 = c.姓名,
                Email = c.Email,
                手機 = c.手機,
                電話 = c.電話,
                客戶名稱 = c.客戶資料.客戶名稱
            }).ToList()))
            {
                return File((ms as MemoryStream).ToArray(), "application/vnd.ms-excel", "export.xls");
            }

        }
    }

}
