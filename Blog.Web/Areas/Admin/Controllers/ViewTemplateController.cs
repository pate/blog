using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.Areas.Admin.Models;
using DreamSongs.MongoRepository;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Authorize] // admin?
    public class ViewTemplateController : Controller
    {
        protected readonly IRepository<ViewTemplate> Templates;
        public ViewTemplateController(IRepository<ViewTemplate> templateRepo)
        {
            Templates = templateRepo;
        }

        public ActionResult Index()
        {
            return View(Templates.All());
        }

        public ActionResult Details(string id)
        {
            var view = Templates.GetById(id);
            if (view == null)
                return HttpNotFound("no such view template");

            return View(view);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var input = new ViewTemplateInputModel();
            return View(input);
        }

        [HttpPost]
        public ActionResult Create(ViewTemplateInputModel input)
        {
            if (ModelState.IsValid)
            {
                var template = Mapper.Map<ViewTemplate>(input);
                template = Templates.Add(template);

                return RedirectToAction("Details", new { id = template.Id });
            }

            return View(input);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var template = Templates.GetById(id);
            if (template == null)
                return HttpNotFound("No such template");

            var input = Mapper.Map<ViewTemplateInputModel>(template);

            return View(input);
        }

        [HttpPost]
        public ActionResult Edit(string id, ViewTemplateInputModel input)
        {
            var template = Templates.GetById(id);
            if (id == null)
                return HttpNotFound("No such view template");

            if (ModelState.IsValid)
            {
                Mapper.Map(input, template);
                Templates.Update(template);

                return RedirectToAction("Details", new { id = template.Id });
            }

            return View(input);
        }

        public ActionResult Test(string id)
        {
            var template = Templates.GetById(id);
            if (id == null)
                return HttpNotFound("No such view template");

            return View(template.ViewPath);
        }
    }
}
