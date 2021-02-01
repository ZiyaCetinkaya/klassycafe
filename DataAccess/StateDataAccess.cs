using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class StateDataAccess
    {
        GenericRepo<STATE> stateRepo;
        klassycafeEntities entity;
        internal List<STATE> GetAll()
        {
            using (entity = new klassycafeEntities())
            {
                stateRepo = new GenericRepo<STATE>(entity);
                return stateRepo.Select().ToList();
            }
        }
    }
}