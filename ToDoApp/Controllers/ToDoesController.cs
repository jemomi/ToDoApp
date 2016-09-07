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
    public class ToDoesController : ApiController
    {
        private TodoModel db = new TodoModel();

        //// GET: api/ToDoes
        //public IQueryable<ToDo> GetToDo()
        //{
        //    return db.ToDo;
        //}

        //// GET: api/ToDoes/5
        //[ResponseType(typeof(ToDo))]
        //public IHttpActionResult GetToDo(int id)
        //{
        //    ToDo toDo = db.ToDo.Find(id);
        //    if (toDo == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(toDo);
        //}

        // PUT: api/ToDoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutToDo(int id, ToDo toDo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toDo.ID)
            {
                return BadRequest();
            }

            db.Entry(toDo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok(toDo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
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

        // POST: api/ToDoes
        [ResponseType(typeof(ToDo))]
        public IHttpActionResult PostToDo(ToDo toDo)
        {
            toDo.Created = DateTime.Now.Date;
            toDo.Finished = false;

            toDo.Category = db.Category.Where(p => p.ID == toDo.CatID).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ToDo.Add(toDo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = toDo.ID }, toDo);
        }

        // DELETE: api/ToDoes/5
        [ResponseType(typeof(ToDo))]
        public IHttpActionResult DeleteToDo(int id)
        {
            ToDo toDo = db.ToDo.Find(id);
            if (toDo == null)
            {
                return NotFound();
            }

            db.ToDo.Remove(toDo);
            db.SaveChanges();

            return Ok(toDo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ToDoExists(int id)
        {
            return db.ToDo.Count(e => e.ID == id) > 0;
        }
    }
}