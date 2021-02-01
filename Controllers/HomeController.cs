using klassycafe.DataAccess;
using klassycafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace klassycafe.Controllers
{
    public class HomeController : Controller
    {
        ReservationDataAccess ReservationDA = new ReservationDataAccess();

        [Route("~/")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reservation_Add(RESERVATION reservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    reservation.RESERVATIONSTATE_ID = 1;
                    ReservationDA.Add(reservation);
                    return Json(new { SuccessMsg = "Rezervation Added." });
                }
                catch (Exception ex)
                {
                    return Json(new { ErrorMsg = "Try Again." });
                }
            }
            else
            {
                return Json(new { ErrorMsg = "Check All Fields." });
            }
        }
    }
}