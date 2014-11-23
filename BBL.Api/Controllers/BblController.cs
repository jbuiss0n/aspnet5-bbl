using BBL.Api.Model;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BBL.Api.Controllers
{
    [Route("v1.0/bbl")]
    public class BblController : Controller
    {
        private static readonly IList<BblModel> _repository;

        static BblController()
        {
            _repository = new List<BblModel>
            {
                new BblModel { Id = 1, Name = "Bbl1" },
                new BblModel { Id = 2, Name = "Bbl2" },
                new BblModel { Id = 3, Name = "Bbl3" },
            };
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(_repository);
        }

        [HttpGet("{id:int}", Name = "GetByIdRoute")]
        public IActionResult Get(int id)
        {
            var bbl = new BblModel { Id = id, Name = "Bbl" + id };

            return new ObjectResult(bbl);
        }

        [HttpPost]
        public IActionResult Post([FromBody]BblModel model)
        {
            return Create(model);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]BblModel model)
        {
            var exists = _repository.FirstOrDefault(b => b.Id == id);
            if (exists != null)
                _repository.Remove(exists);

            return Create(model);
        }

        private ObjectResult Create(BblModel model)
        {
            model.Id = _repository.Max(b => b.Id) + 1;
            _repository.Add(model);

            var location = Url.RouteUrl("GetByIdRoute", new { id = model.Id }, Request.Scheme, Request.Host.ToUriComponent());
            Context.Response.StatusCode = 201;
            Context.Response.Headers["Location"] = location;

            return new ObjectResult(model);
        }
    }
}