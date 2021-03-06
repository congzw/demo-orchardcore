﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NbSites.Base.Data;
using NbSites.Base.Data.Products;

namespace NbSites.Base.Api
{
    [ApiExplorerSettings(GroupName = "Base-Products")]
    [Route("~/Api/Base/Product/[action]")]
    [ApiController]
    public class ProductApi : ControllerBase
    {
        [HttpGet]
        public string GetInfo()
        {
            return this.GetType().FullName;
        }

        [HttpGet]
        public Product GetProduct([FromServices] BaseDbContext dbContext)
        {
            return dbContext.Products.FirstOrDefault();
        }
    }
}
