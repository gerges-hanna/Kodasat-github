using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kodasat.Controllers
{
    public class MyLibrary : Controller
    {
        //pre:  must but in page you don't want the browser back to him again && make if statment with Session check 
        //Post: This is used to clear Cache And Heade (To Handl back button in browser)
        //ToCall: just call the object and action and write (Response) inside functions ==> myLibrary.PreventBackButtonBrowser(Response);
        public void PreventBackButtonBrowser(HttpResponseBase response)
        {
            response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            
            response.ClearHeaders();
            response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            response.AddHeader("Pragma", "no-cache");
        }
    }
}