using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class PortfolioController : Controller
    {

        // Reference to the database
        private MyDbContext db = new MyDbContext();

        // GET: Portfolio
        public ActionResult Index()
        {
            // Display the projects of the portfolio
            if (User.Identity.IsAuthenticated)
            {
                // offer a link to the admin panel
                // return View("Admin Link");

                return View(db.Projects.ToList());
            }
            else
            {
                return View(db.Projects.ToList());
            }

        }

        // GET: Portfolio/Admin
        public ActionResult Admin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(db.Projects.ToList());
            }
            else
            {
                return RedirectToAction("");
            }
        }

        // GET: Portfolio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Portfolio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Portfolio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Type,Description")] Project project, HttpPostedFileBase[] files)
        {

            // Handle project info
            try
            {

                // Loop for multiple files
                foreach (HttpPostedFileBase file in files)
                {
                    // Get the file name
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    // Save the file in a server folder
                    file.SaveAs(Server.MapPath("~/Content/Files/ProjectImages/" + filename));
                    string filepathToSave = "Content/Files/ProjectImages/" + filename;

                    // Add project info into the 'Projects' database
                    if (ModelState.IsValid)
                    {
                        db.Projects.Add(project);
                        db.SaveChanges();
                    }
                }

                ViewBag.Message = "Project craeted successfully.";
            }
            catch
            {
                ViewBag.Message = "Error while creating the project.";
                return View(project);
            }

            // Return to the project list
            return View("Index", db.Projects.ToList());
        }

        // GET: Portfolio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Portfolio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Type,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Portfolio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Portfolio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
