using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class ContactDataAccess
    {
        GenericRepo<CONTACT> ContactRepo;

        klassycafeEntities entity;
        internal void Update(CONTACT CONTACT)
        {
            using (entity = new klassycafeEntities())
            {
                ContactRepo = new GenericRepo<CONTACT>(entity);
                ContactRepo.Update(CONTACT);
                ContactRepo.Save();
            }
        }

        internal CONTACT Get()
        {
            using (entity = new klassycafeEntities())
            {
                ContactRepo = new GenericRepo<CONTACT>(entity);
                int CONTACTCount = ContactRepo.Select().Count();
                if (CONTACTCount <= 0)
                {
                    ContactRepo.Insert(new CONTACT()
                    {
                        EMAIL = "",
                        PHONE = "",
                        FACEBOOK = "",
                        INSTAGRAM = "",
                        LINKEDIN = "",
                        TWITTER = ""
                    });
                    ContactRepo.Save();

                    return ContactRepo.Select().ToList()[0];
                }
                else
                    return ContactRepo.Select().ToList()[0];
            }
        }
    }
}