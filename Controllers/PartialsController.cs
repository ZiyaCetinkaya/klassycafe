using klassycafe.DataAccess;
using klassycafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace klassycafe.Controllers
{
    public class PartialsController : Controller
    {
        ChefDataAccess chefDA = new ChefDataAccess();
        ContactDataAccess contactDA = new ContactDataAccess();
        MenuItemDataAccess menuItemDA = new MenuItemDataAccess();
        MenuCategoryDataAccess menuCategoryDA = new MenuCategoryDataAccess();

        public ActionResult _Footer()
        {
            CONTACT contact = contactDA.Get();
            return PartialView(contact);
        }

        public ActionResult _TopSlider()
        {
            return PartialView();
        }

        public ActionResult _AboutUs()
        {
            return PartialView();
        }

        public ActionResult _MenuSlider()
        {
            List<MENUITEM> sliderItems = menuItemDA.GetForMenuSlider();
            return PartialView(sliderItems);
        }

        public ActionResult _Chefs()
        {
            List<CHEF> chefs = chefDA.GetActives();
            return PartialView(chefs);
        }

        public ActionResult _Reservation()
        {
            CONTACT contact = contactDA.Get();
            return PartialView(contact);
        }

        public ActionResult _ReservationForm()
        {
            return PartialView();
        }

        public ActionResult _Menu()
        {
            List<MENUCATEGORY> mcs = menuCategoryDA.GetActives();
            List<MENUITEM> mis = menuItemDA.GetActives();

            return PartialView(new MENUINDEX() { CATEGORIES = mcs, ITEMS = mis });
        }
    }
}