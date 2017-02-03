using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppInsightsMVCFilters;

namespace WebAPIIntegrationTests.Controllers
{
    public class WebApiController : ApiController
    {
        [TrackRequests("GETAction")]
        public int Get(int id)
        {
            return id;
        }
    }
}
