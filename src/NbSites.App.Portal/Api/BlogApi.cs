using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NbSites.App.Portal.Data;
using NbSites.App.Portal.Data.Blogs;
using NbSites.Base.Data;

namespace NbSites.App.Portal.Api
{
    [ApiExplorerSettings(GroupName = "App-Portal-Blog")]
    [Route("~/Api/App/Portal/Blog/[action]")]
    [ApiController]
    public class BlogApi : ControllerBase
    {
        [HttpGet]
        public string GetInfo()
        {
            return this.GetType().FullName;
        }

        [HttpGet]
        public IList<Blog> GetBlogs([FromServices] NbSitesDbContext dbContext)
        {
            return dbContext.Set<Blog>().ToList();
        }

        [HttpGet]
        public IList<Blog> GetBlogs2([FromServices] PortalDbContext dbContext)
        {
            return dbContext.Blogs.ToList();
        }

        [HttpGet]
        public string GetDbContext([FromServices] NbSitesDbContext dbContext, [FromServices] PortalDbContext dbContext2)
        {
            return "NbSitesDbContext:" + dbContext.GetHashCode() + ", PortalDbContext:" + dbContext2.GetHashCode() + ", PortalDbContext.NbSitesDbContext:" + dbContext2.NbSitesDbContext.GetHashCode();
        }
    }
}
