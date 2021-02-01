using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class ReservationStateDataAccess
    {
        GenericRepo<RESERVATIONSTATE> stateRepo;
        klassycafeEntities entity;
        internal List<RESERVATIONSTATE> GetAll()
        {
            using (entity = new klassycafeEntities())
            {
                stateRepo = new GenericRepo<RESERVATIONSTATE>(entity);
                return stateRepo.Select().ToList();
            }
        }
    }
}