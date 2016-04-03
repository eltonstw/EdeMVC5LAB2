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
    public class 客戶分類Controller : BaseController
    {                
        // GET: 客戶分類
        public ActionResult Index()
        {
            return View(RepoCategory.All().ToList());
        }

        // GET: 客戶分類/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類 客戶分類 = RepoCategory.Find(id);
            if (客戶分類 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類);
        }

        // GET: 客戶分類/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶分類/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryName")] 客戶分類 客戶分類)
        {
            if (ModelState.IsValid)
            {
                RepoCategory.Add(客戶分類);
                RepoCategory.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶分類);
        }

        // GET: 客戶分類/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類 客戶分類 = RepoCategory.Find(id);
            if (客戶分類 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類);
        }

        // POST: 客戶分類/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryName")] 客戶分類 客戶分類)
        {
            if (ModelState.IsValid)
            {
                RepoCategory.UnitOfWork.Context.Entry(客戶分類).State = EntityState.Modified;
                RepoCategory.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶分類);
        }

        // GET: 客戶分類/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶分類 客戶分類 = RepoCategory.Find(id);
            if (客戶分類 == null)
            {
                return HttpNotFound();
            }
            return View(客戶分類);
        }

        // POST: 客戶分類/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶分類 客戶分類 = RepoCategory.Find(id);
            RepoCategory.Delete(客戶分類);
            RepoCategory.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }


        public ActionResult Export()
        {
            using (var ms = ExcelUtility.RenderListToExcel(RepoCategory.All().Select(c=>new {Category = c.CategoryName}).ToList()))
            {
                return File((ms as MemoryStream).ToArray(), "application/vnd.ms-excel", "export.xls");
            }
                                   
        }
    }
}
