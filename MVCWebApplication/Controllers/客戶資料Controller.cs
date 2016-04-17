using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCWebApplication.Models;
using PagedList;

namespace MVCWebApplication.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index(string searchStr, string cusCategory, int page=1)
        {
            //var data = db.客戶資料.AsQueryable().Where(p => p.是否已刪除 == false);
            var data = repo客戶資料.All();  //只顯示未刪除的            

            //老師上課介紹的作法
            var 客戶分類List = data.Select(p => p.客戶分類).Distinct().OrderBy(p => p).ToList();
            ViewBag.cusCategory = new SelectList(data.Select(p => p.客戶分類).Distinct());

            //SelectList selectList = new SelectList(data.Select(p => p.客戶分類).Distinct());            
            //ViewBag.CategorySelectList = selectList;
            
            if (!String.IsNullOrEmpty(cusCategory))
            {
                data = data.Where(p => p.客戶分類 == cusCategory);
            }

            if (!String.IsNullOrEmpty(searchStr))
            {                
                data = data.Where(p => p.客戶名稱.Contains(searchStr) || p.統一編號.Contains(searchStr));
            }
            //return View(db.客戶資料.ToList());
            //return View("Index", data);

            //當實際的資料頁數小於前一次的頁數時，要以實際的資料頁數為主            
            int realPage = (data.Count() / 10 )+ 1;            
            //ViewBag.realPage = realPage;
            //ViewBag.Page = page;

            if (page > realPage)
                page = realPage;

            var 客戶分類data = data.OrderBy(p => p.客戶名稱).ToPagedList(page, 10);            
            return View("Index", 客戶分類data);
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //db.客戶資料.Add(客戶資料);
                //db.SaveChanges();
                repo客戶資料.Add(客戶資料);
                repo客戶資料.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(客戶資料).State = EntityState.Modified;
                //db.SaveChanges();

                repo客戶資料.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repo客戶資料.UnitOfWork.Commit();

                TempData["EditProductsSuccessMsg"] = 客戶資料.客戶名稱 + "修改成功";

                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
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
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo客戶資料.Find(id);
            //不要真的刪除，將欄位「是否已刪除」改為true
            客戶資料.是否已刪除 = true;

            foreach (var 客戶聯絡人item in 客戶資料.客戶聯絡人.ToList())
            {
                //有FK的資料表，所以也要把對應FK的資料刪除掉
                客戶聯絡人item.是否已刪除 = true;                
            }
            foreach (var 客戶銀行item in 客戶資料.客戶銀行資訊.ToList())
            {
                //有FK的資料表，所以也要把對應FK的資料刪除掉
                客戶銀行item.是否已刪除 = true;
            }

            //db.客戶資料.Remove(客戶資料);
            //db.SaveChanges();
            repo客戶資料.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                repo客戶資料.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult StaticsCustomerData()
        {
            //var data = db.客戶資料VIEW.OrderByDescending(p => p.客戶名稱).ToList();            
            var data = repo客戶資料View.All().OrderByDescending(p => p.客戶名稱).ToList();
            return View(data);
        }
               
    }
}
