using System.Web.Mvc;
using AutoMapper;
using Blog.Web.Helpers;
using DreamSongs.MongoRepository;
using PagedList;

namespace Blog.Web.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class EntityController<T> : Controller where T : Entity, new()
    {
        protected readonly IRepository<T> Repository;

        public EntityController()
        {
            Repository = DependencyResolver.Current.GetService<IRepository<T>>();

            Mapper.CreateMap<T, T>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }

        public virtual ActionResult Index(
            string q,
            int page = 1,
            int pageSize = 20)
        {
            var pagedEntities = Repository.All().ToPagedList(page, pageSize);

            return View("Index", pagedEntities); // todo pading
        }

        public virtual ActionResult Grid()
        {
            return View("Grid", Repository.All());
        }

        public virtual ActionResult Details(string id)
        {
            var entity = Repository.GetById(id);
            if (entity == null)
                return new HttpNotFoundResult();

            return View("Details", entity);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            var inputModel = new T();
            return View("Create", inputModel);
        }

        [HttpGet]
        public virtual ActionResult Copy(string id)
        {
            var part = Repository.GetById(id);
            var inputModel = new T();
            Mapper.CreateMap<T, T>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.Map(part, inputModel);

            return View("Create", inputModel);
        }

        [HttpPost]
        public virtual ActionResult Create(T inputModel)
        {
            if (ModelState.IsValid)
            {
                Repository.Add(inputModel);
                return RedirectToAction("Index");
            }

            return View("Create", inputModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(string id)
        {
            var entity = Repository.GetById(id);
            if (entity == null)
                return new HttpNotFoundResult();
            return View("Edit", entity);
        }

        [HttpPost]
        public virtual ActionResult Edit(string id, T inputModel)
        {
            var entity = Repository.GetById(id);
            if (entity == null)
                return new HttpNotFoundResult();

            if (ModelState.IsValid)
            {
                Mapper.Map(inputModel, entity);

                Repository.Update(entity);
                this.FlashInfo("Updated {0} with Id {1}".Fmt(typeof(T).Name, id));

                return RedirectToAction("Details", new { entity.Id });
            }

            return View("Edit", inputModel);
        }


        [HttpGet]
        public virtual ActionResult Delete(string id)
        {
            var entity = Repository.GetById(id);
            if (entity == null)
                return new HttpNotFoundResult();
            return View("Delete", entity);
        }

        [HttpPost]
        [ActionName("Delete")]
        public virtual ActionResult DeleteEntity(string id)
        {
            var entity = Repository.GetById(id);
            if (entity == null)
                return new HttpNotFoundResult();

            Repository.Delete(entity);

            this.FlashInfo("Deleted entity {0}".Fmt(entity.Id));

            return RedirectToAction("Index");
        }
    }
}
