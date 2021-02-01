using klassycafe.Models;
using klassycafe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.DataAccess
{
    public class UserDataAccess
    {
        GenericRepo<USER> userRepo;
        klassycafeEntities entity;

        internal bool IsLoginOK(USER user)
        {
            bool isLoginOk = false;
            using (entity = new klassycafeEntities())
            {
                userRepo = new GenericRepo<USER>(entity);
                USER _user = userRepo.FindByLambda(u => u.EMAIL == user.EMAIL && u.PASSWORD == user.PASSWORD);
                if (_user != null)
                    isLoginOk = true;
            }
            return isLoginOk;
        }

        internal USER GetByEmail(string email)
        {
            using (entity = new klassycafeEntities())
            {
                userRepo = new GenericRepo<USER>(entity);
                return userRepo.FindByLambda(u => u.EMAIL == email);
            }
        }
    }
}