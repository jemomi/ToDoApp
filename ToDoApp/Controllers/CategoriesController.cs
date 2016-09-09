using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [RoutePrefix("api/Categories")]
    public class CategoriesController : ApiController
    {
        private TodoModel db = new TodoModel();

        // GET: api/Categories
        public IQueryable<Category> GetCategory()
        {

            return db.Category;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //[ResponseType(typeof(Category))]
        [HttpPost]
        [Route("reget")]
        public IHttpActionResult GetCategory(List<Category> listCat)
        {
            List<Category> newListCat = new List<Category>();
            if (listCat == null)
            {
                return Ok(db.Category);
            }
            foreach (Category cat in listCat)
            {
                Category dbCat = db.Category.Find(cat.ID);
                if (dbCat != null)
                {
                    Category newCat = new Category()
                    {
                        ID = dbCat.ID,
                        Name = dbCat.Name
                    };
                    if (!newListCat.Contains(newCat))
                    {
                        newListCat.Add(newCat);
                    }
                }
            }
            foreach (Category cat in listCat)
            {
                foreach (ToDo todo in cat.ToDo)
                {
                    ToDo dbToDo = db.ToDo.Find(todo.ID);
                    if (dbToDo != null)
                    {
                        Category newCat = newListCat.Find(p => p.ID == dbToDo.CatID);
                        newCat.ToDo.Add(dbToDo);
                    }
                    else
                    {
                        //Item not in DB
                    }
                }
            }
            //Category category = db.Category.Find(id);

            return Ok(newListCat);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.ID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Category.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.ID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.ToDo.RemoveRange(category.ToDo);

            db.Category.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Category.Count(e => e.ID == id) > 0;
        }
    }
}