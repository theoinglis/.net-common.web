using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Db.Repository;
using Common.Db.UnitOfWork;

namespace Common.Web.Controllers
{
    public class ApiController<TEntity, TId> : ApiController where TEntity : class
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected IRepository<TEntity> Repository;

        protected ApiController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = unitOfWork.CreateRepository<TEntity>();
        }

        public virtual string Name { get { return typeof(TEntity).Name; } }

        public virtual IEnumerable<TEntity> Get()
        {
            return Repository.Get();
        }

        public virtual TEntity Get(TId id)
        {
            var item = Repository.GetById(id);

            if (item == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Could not find the required {0}", Name)));
            }

            return item;
        }

        public virtual HttpResponseMessage Post(TEntity item)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Repository.Insert(item);
            UnitOfWork.Save();

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public virtual HttpResponseMessage Put(TId id, TEntity item)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Repository.Update(item);
            UnitOfWork.Save();

            return Request.CreateResponse(HttpStatusCode.OK, item);

        }

        public virtual HttpResponseMessage Delete(TId id)
        {
            var item = Repository.GetById(id);

            if (item == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Could not find the required {0}", Name));
            }

            Repository.Delete(item);
            UnitOfWork.Save();

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}
