using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class MenuCategoryDataAccess
    {
        GenericRepo<MENUCATEGORY> menuCategoryRepo;
        klassycafeEntities entity;
        internal List<MENUCATEGORY> GetAll()
        {
            using (entity = new klassycafeEntities())
            {
                menuCategoryRepo = new GenericRepo<MENUCATEGORY>(entity);
                return menuCategoryRepo.Select(mc => mc.STATE_ID == 1).ToList();
            }
        }

        internal List<MENUCATEGORY> GetActives()
        {
            using (entity = new klassycafeEntities())
            {
                menuCategoryRepo = new GenericRepo<MENUCATEGORY>(entity);
                return menuCategoryRepo.Select(mc => mc.STATE_ID == 1).ToList();
            }
        }
    }
}