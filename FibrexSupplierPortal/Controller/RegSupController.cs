using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FibrexSupplierPortal.Models;
using FSPBAL;

namespace FibrexSupplierPortal.Controller
{    
    public class RegSupController : ApiController
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        //public IEnumerable<RegSupModel> GetAllProducts()
        //{ 
             
        //}
    }
}
