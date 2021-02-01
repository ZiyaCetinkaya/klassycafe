using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class MenuItemDataAccess
    {
        GenericRepo<MENUITEM> menuItemRepo;
        GenericRepo<MENUCATEGORY> menuCategoryRepo;
        GenericRepo<STATE> stateRepo;
        klassycafeEntities entity;
        internal List<MENUITEMLIST> GetAllForList()
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                stateRepo = new GenericRepo<STATE>(entity);
                menuCategoryRepo = new GenericRepo<MENUCATEGORY>(entity);

                List<MENUITEM> mis = menuItemRepo.Select().ToList();
                List<MENUITEMLIST> milist = new List<MENUITEMLIST>();

                foreach (MENUITEM mi in mis)
                {
                    milist.Add(new MENUITEMLIST()
                    {
                        item = mi,
                        stateName = stateRepo.FindByID(mi.STATE_ID).NAME,
                        categoryName = menuCategoryRepo.FindByID(mi.MENUCATEGORY_ID).TITLE
                    });
                }

                return milist;
            }
        }

        internal List<MENUITEM> GetForMenuSlider()
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                return menuItemRepo.Select(mc => mc.STATE_ID == 1 && (bool)mc.MENUSLIDER_SHOW).ToList();
            }
        }

        internal void Add(MENUITEM menuitem)
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                menuItemRepo.Insert(menuitem);
                menuItemRepo.Save();
            }
        }

        internal List<MENUITEM> GetActives()
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                return menuItemRepo.Select(mc => mc.STATE_ID == 1).ToList();
            }
        }

        internal MENUITEMLIST GetByIdForList(int id)
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                stateRepo = new GenericRepo<STATE>(entity);
                menuCategoryRepo = new GenericRepo<MENUCATEGORY>(entity);

                MENUITEM menuitem = menuItemRepo.FindByID(id);
                STATE state = stateRepo.FindByID(menuitem.STATE_ID);
                MENUCATEGORY menucategory = menuCategoryRepo.FindByID(menuitem.MENUCATEGORY_ID);

                return new MENUITEMLIST()
                {
                    item = menuitem,
                    stateName = state.NAME,
                    categoryName = menucategory.TITLE
                };
            }
        }

        internal MENUITEM GetById(int id)
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                return menuItemRepo.FindByID(id);
            }
        }

        internal void Edit(MENUITEM menuitem)
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                menuItemRepo.Update(menuitem);
                menuItemRepo.Save();
            }
        }

        internal void Remove(int id)
        {
            using (entity = new klassycafeEntities())
            {
                menuItemRepo = new GenericRepo<MENUITEM>(entity);
                menuItemRepo.Delete(id);
                menuItemRepo.Save();
            }
        }
    }
}