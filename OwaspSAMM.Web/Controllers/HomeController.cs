using System.Web.Mvc;

namespace OwaspSAMM.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        UserDataBL userDataBL = new UserDataBL();

        public ActionResult Index()
        {
            logger.InfoFormat("User.Identity.Name: {0}", User.Identity.Name);

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            return View();
        }

        public ActionResult About()
        {
            logger.InfoFormat("About Page");
            ViewBag.Message = "OwaspSAMM application description page.";

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            return View();
        }

        public ActionResult Contact()
        {
            logger.InfoFormat("Contact Page");
            ViewBag.Message = "Contacts";

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            return View();
        }

        public ActionResult NotAuthorized(string message)
        {
            
            ViewBag.Title = "Not Authorized";
            ViewBag.Message = message.ToString();

            var userData = userDataBL.FindUser(User.Identity.Name);

            ViewBag.isAdmin = userData.IsAdministrator();
            ViewBag.isBUOwner = userData.IsBUOwner();
            ViewBag.UserName = userData.FullName;

            return View();
        }


    }
}