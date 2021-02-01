using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class ReservationDataAccess
    {
        GenericRepo<RESERVATIONSTATE> stateRepo;
        GenericRepo<RESERVATION> reservationRepo;

        klassycafeEntities entity;
        internal List<RESERVATIONLIST> GetAllForList()
        {
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                stateRepo = new GenericRepo<RESERVATIONSTATE>(entity);

                List<RESERVATION> rs = reservationRepo.Select().ToList();
                List<RESERVATIONLIST> rList = new List<RESERVATIONLIST>();

                foreach (RESERVATION r in rs)
                {
                    rList.Add(new RESERVATIONLIST()
                    {
                        reservation = r,
                        reservationStateName = stateRepo.FindByID(r.RESERVATIONSTATE_ID).NAME
                    });
                }

                return rList;
            }
        }

        internal List<RESERVATIONLIST> GetTodayList()
        {
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1);
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                stateRepo = new GenericRepo<RESERVATIONSTATE>(entity);

                List<RESERVATION> rs = reservationRepo.Select(r => r.RESERVATION_DATE >= startDateTime && r.RESERVATION_DATE <= endDateTime).ToList();
                List<RESERVATIONLIST> rList = new List<RESERVATIONLIST>();

                foreach (RESERVATION r in rs)
                {
                    rList.Add(new RESERVATIONLIST()
                    {
                        reservation = r,
                        reservationStateName = stateRepo.FindByID(r.RESERVATIONSTATE_ID).NAME
                    });
                }

                return rList;
            }
        }
        
        internal List<RESERVATIONLIST> GetLast(int count)
        {
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                stateRepo = new GenericRepo<RESERVATIONSTATE>(entity);

                List<RESERVATION> rs = reservationRepo.Select().OrderByDescending(r => r.CREATED_DATE).Take(count).ToList();
                List<RESERVATIONLIST> rList = new List<RESERVATIONLIST>();

                foreach (RESERVATION r in rs)
                {
                    rList.Add(new RESERVATIONLIST()
                    {
                        reservation = r,
                        reservationStateName = stateRepo.FindByID(r.RESERVATIONSTATE_ID).NAME
                    });
                }

                return rList;
            }
        }

        internal void Add(RESERVATION reservation)
        {
            using (entity = new klassycafeEntities())
            {
                reservation.CREATED_DATE = DateTime.Now;
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                reservationRepo.Insert(reservation);
                reservationRepo.Save();
            }
        }

        internal RESERVATIONLIST GetByIdForList(int id)
        {
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                stateRepo = new GenericRepo<RESERVATIONSTATE>(entity);

                RESERVATION reservation = reservationRepo.FindByID(id);

                return new RESERVATIONLIST()
                {
                    reservation = reservation,
                    reservationStateName = stateRepo.FindByID(reservation.RESERVATIONSTATE_ID).NAME
                };
            }
        }

        internal RESERVATION GetById(int id)
        {
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                return reservationRepo.FindByID(id);
            }
        }

        internal void Edit(RESERVATION reservation)
        {
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                reservationRepo.Update(reservation);
                reservationRepo.Save();
            }
        }

        internal void Remove(int id)
        {
            using (entity = new klassycafeEntities())
            {
                reservationRepo = new GenericRepo<RESERVATION>(entity);
                reservationRepo.Delete(id);
                reservationRepo.Save();
            }
        }
    }
}