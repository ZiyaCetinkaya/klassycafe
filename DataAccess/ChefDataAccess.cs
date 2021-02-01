using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class ChefDataAccess
    {
        GenericRepo<STATE> stateRepo;
        GenericRepo<CHEF> chefRepo;

        klassycafeEntities entity;
        internal List<CHEF> GetAll()
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                return chefRepo.Select().ToList();
            }
        }

        internal void Add(CHEF chef)
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                chefRepo.Insert(chef);
                chefRepo.Save();
            }
        }

        internal List<CHEF> GetActives()
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                return chefRepo.Select(c => c.STATE_ID == 1).ToList();
            }
        }

        internal CHEF GetById(int id)
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                return chefRepo.FindByID(id);
            }
        }

        internal void Edit(CHEF chef)
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                chefRepo.Update(chef);
                chefRepo.Save();
            }
        }

        internal void Remove(int id)
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                chefRepo.Delete(id);
                chefRepo.Save();
            }
        }

        internal List<CHEFLIST> GetAllForList()
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                stateRepo = new GenericRepo<STATE>(entity);

                List<CHEF> cs = chefRepo.Select().ToList();
                List<CHEFLIST> cList = new List<CHEFLIST>();

                foreach (CHEF c in cs)
                {
                    cList.Add(new CHEFLIST()
                    {
                        chef = c,
                        stateName = stateRepo.FindByID(c.STATE_ID).NAME
                    });
                }

                return cList;
            }
        }

        internal CHEFLIST GetByIdForList(int id)
        {
            using (entity = new klassycafeEntities())
            {
                chefRepo = new GenericRepo<CHEF>(entity);
                stateRepo = new GenericRepo<STATE>(entity);

                CHEF chef = chefRepo.FindByID(id);

                return new CHEFLIST()
                {
                    chef = chef,
                    stateName = stateRepo.FindByID(chef.STATE_ID).NAME
                };
            }
        }
    }
}