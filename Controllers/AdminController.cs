using klassycafe.DataAccess;
using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace klassycafe.Controllers
{
    public class AdminController : Controller
    {
        UserDataAccess userDA = new UserDataAccess();
        MenuItemDataAccess menuItemDA = new MenuItemDataAccess();
        MenuCategoryDataAccess menuCategoryDA = new MenuCategoryDataAccess();
        StateDataAccess stataDA = new StateDataAccess();
        ChefDataAccess chefDA = new ChefDataAccess();
        ContactDataAccess contactDA = new ContactDataAccess();
        ReservationDataAccess reservationDA = new ReservationDataAccess();
        ReservationStateDataAccess reservationStateDA = new ReservationStateDataAccess();

        [Route("admin")]
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            return View();
        }

        public ActionResult _TodaysReservations()
        {
            List<RESERVATIONLIST> todaysReservations = reservationDA.GetTodayList();
            return PartialView(todaysReservations);
        }

        public ActionResult _LastReservations()
        {
            List<RESERVATIONLIST> lastReservations = reservationDA.GetLast(10);
            return PartialView(lastReservations);
        }

        #region Account

        [Route("login")]
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public ActionResult LoginForm(USER user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userDA.IsLoginOK(user))
                    {
                        USER _user = userDA.GetByEmail(user.EMAIL);
                        Session["UserId"] = _user.ID;
                        Session["UserEmail"] = _user.EMAIL;
                        Session["UserName"] = _user.NAME;

                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.Error = "Wrong Email or Password.";
                        return View("Login", user);
                    }
                }
                else
                {
                    ViewBag.Error = "Check your email/password.";
                    return View("Login", user);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Check your email/password.";
                return View("Login", user);
            }
        }

        [Route("logout")]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        #endregion

        #region MenuItem

        [Route("menu-items")]
        public ActionResult MenuItems()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            List<MENUITEMLIST> menuitems = menuItemDA.GetAllForList();
            return View(menuitems);
        }

        [Route("add-menu-item")]
        public ActionResult MenuItem_Add()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            PopulateStateDropdownList();
            PopulateMenuCategoryDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult AddMenuItemForm(MENUITEM menuitem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/menuitem/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        menuitem.IMAGE = dosyaAdi + uzanti;
                    }
                    else
                    {
                        ViewBag.Error = "Add Image.";
                        return View("MenuItem_Add", menuitem);
                    }

                    menuItemDA.Add(menuitem);
                    return RedirectToAction("MenuItems", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateMenuCategoryDropdownList(menuitem.MENUCATEGORY_ID);
                    PopulateStateDropdownList(menuitem.STATE_ID);
                    return View("MenuItem_Add", menuitem);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateMenuCategoryDropdownList(menuitem.MENUCATEGORY_ID);
                PopulateStateDropdownList(menuitem.STATE_ID);
                return View("MenuItem_Add", menuitem);
            }
        }

        [Route("menu-item/{id}")]
        public ActionResult MenuItem(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            MENUITEMLIST mi = menuItemDA.GetByIdForList(ID);
            return View(mi);
        }

        [Route("edit-menu-item/{id}")]
        public ActionResult MenuItem_Edit(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            MENUITEM mi = menuItemDA.GetById(ID);
            PopulateStateDropdownList(mi.STATE_ID);
            PopulateMenuCategoryDropdownList(mi.MENUCATEGORY_ID);
            return View(mi);
        }

        [HttpPost]
        public ActionResult EditMenuItemForm(MENUITEM menuitem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MENUITEM mi = menuItemDA.GetById(menuitem.ID);
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Content/images/menuitem/" + mi.IMAGE)))
                            System.IO.File.Delete(Server.MapPath("~/Content/images/menuitem/" + mi.IMAGE));

                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/menuitem/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        menuitem.IMAGE = dosyaAdi + uzanti;
                    }
                    else
                        menuitem.IMAGE = mi.IMAGE;

                    menuItemDA.Edit(menuitem);
                    return RedirectToAction("MenuItems", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateMenuCategoryDropdownList(menuitem.MENUCATEGORY_ID);
                    PopulateStateDropdownList(menuitem.STATE_ID);
                    return View("MenuItem_Edit", menuitem);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateMenuCategoryDropdownList(menuitem.MENUCATEGORY_ID);
                PopulateStateDropdownList(menuitem.STATE_ID);
                return View("MenuItem_Edit", menuitem);
            }
        }

        [Route("remove-menu-item/{id}")]
        public ActionResult MenuItem_Remove(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            MENUITEMLIST mi = menuItemDA.GetByIdForList(ID);
            return View(mi);
        }

        [HttpPost]
        public ActionResult RemoveMenuItemForm(MENUITEMLIST menuitem)
        {
            try
            {
                menuItemDA.Remove(menuitem.item.ID);
                return RedirectToAction("MenuItems", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                return View("MenuItem_Remove", menuitem);
            }
        }

        #endregion

        #region Chef

        [Route("chefs")]
        public ActionResult Chefs()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            List<CHEFLIST> chefs = chefDA.GetAllForList();
            return View(chefs);
        }

        [Route("add-chef")]
        public ActionResult Chef_Add()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            PopulateStateDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult AddChefForm(CHEF chef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/chef/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        chef.PICTURE = dosyaAdi + uzanti;
                    }
                    else
                    {
                        ViewBag.Error = "Add Image.";
                        PopulateStateDropdownList();
                        return View("Chef_Add", chef);
                    }

                    chefDA.Add(chef);
                    return RedirectToAction("Chefs", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(chef.STATE_ID);
                    return View("Chef_Add", chef);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(chef.STATE_ID);
                return View("Chef_Add", chef);
            }
        }

        [Route("chef/{id}")]
        public ActionResult Chef(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            CHEFLIST chef = chefDA.GetByIdForList(ID);
            return View(chef);
        }

        [Route("edit-chef/{id}")]
        public ActionResult Chef_Edit(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            CHEF chef = chefDA.GetById(ID);
            PopulateStateDropdownList(chef.STATE_ID);
            return View(chef);
        }

        [HttpPost]
        public ActionResult EditChefForm(CHEF chef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CHEF c = chefDA.GetById(chef.ID);
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Content/images/chef/" + chef.PICTURE)))
                            System.IO.File.Delete(Server.MapPath("~/Content/images/chef/" + chef.PICTURE));

                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/chef/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        chef.PICTURE = dosyaAdi + uzanti;
                    }
                    else
                        chef.PICTURE = c.PICTURE;

                    chefDA.Edit(chef);
                    return RedirectToAction("Chefs", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(chef.STATE_ID);
                    return View("Chef_Edit", chef);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(chef.STATE_ID);
                return View("Chef_Edit", chef);
            }
        }

        [Route("remove-chef/{id}")]
        public ActionResult Chef_Remove(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            CHEF c = chefDA.GetById(ID);
            return View(c);
        }

        [HttpPost]
        public ActionResult RemoveChefForm(CHEF chef)
        {
            try
            {
                chefDA.Remove(chef.ID);
                return RedirectToAction("Chefs", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(chef.STATE_ID);
                return View("Chef_Remove", chef);
            }
        }

        #endregion

        #region Contact

        [Route("contact-info")]
        public ActionResult Contact()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            CONTACT settings = contactDA.Get();
            return View(settings);
        }

        public ActionResult EditContactForm(CONTACT settings)
        {
            contactDA.Update(settings);
            return View("Index");
        }

        #endregion

        #region Reservation

        [Route("reservations")]
        public ActionResult Reservations()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            List<RESERVATIONLIST> reservations = reservationDA.GetAllForList().OrderByDescending(r => r.reservation.CREATED_DATE).ToList();
            return View(reservations);
        }

        [Route("add-reservation")]
        public ActionResult Reservation_Add()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            PopulateReservationStateDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult AddReservationForm(RESERVATION reservation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reservationDA.Add(reservation);
                    return RedirectToAction("Reservations", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateReservationStateDropdownList(reservation.RESERVATIONSTATE_ID);
                    return View("Reservation_Add", reservation);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateReservationStateDropdownList(reservation.RESERVATIONSTATE_ID);
                return View("Reservation_Add", reservation);
            }
        }

        [Route("reservation/{id}")]
        public ActionResult Reservation(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            RESERVATIONLIST reservation = reservationDA.GetByIdForList(ID);
            return View(reservation);
        }

        [Route("edit-reservation/{id}")]
        public ActionResult Reservation_Edit(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            RESERVATION reservation = reservationDA.GetById(ID);
            PopulateReservationStateDropdownList(reservation.RESERVATIONSTATE_ID);

            return View(reservation);
        }

        [HttpPost]
        public ActionResult EditReservationForm(RESERVATION reservation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reservationDA.Edit(reservation);
                    return RedirectToAction("Reservations", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateReservationStateDropdownList(reservation.RESERVATIONSTATE_ID);
                    return View("Reservation_Add", reservation);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateReservationStateDropdownList(reservation.RESERVATIONSTATE_ID);
                return View("Reservation_Add", reservation);
            }
        }

        [Route("remove-reservation/{id}")]
        public ActionResult Reservation_Remove(int ID)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            RESERVATION r = reservationDA.GetById(ID);
            return View(r);
        }

        [HttpPost]
        public ActionResult RemoveReservationForm(RESERVATION reservation)
        {
            try
            {
                reservationDA.Remove(reservation.ID);
                return RedirectToAction("Reservations", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateReservationStateDropdownList(reservation.RESERVATIONSTATE_ID);
                return View("Reservation_Add", reservation);
            }
        }

        #endregion

        private void PopulateStateDropdownList(object selectedState = null)
        {
            var statesQuery = stataDA.GetAll();
            ViewBag.STATE_ID = new SelectList(statesQuery, "ID", "NAME", selectedState);
        }

        private void PopulateMenuCategoryDropdownList(object selectedMenuCategory = null)
        {
            var categoryQuery = menuCategoryDA.GetActives();
            ViewBag.MENUCATEGORY_ID = new SelectList(categoryQuery, "ID", "TITLE", selectedMenuCategory);
        }

        private void PopulateReservationStateDropdownList(object selectedReservationState = null)
        {
            var reservationStateQuery = reservationStateDA.GetAll();
            ViewBag.RESERVATIONSTATE_ID = new SelectList(reservationStateQuery, "ID", "NAME", selectedReservationState);
        }
    }
}