using Microsoft.Data.Entity;
using SQLiteIdentityEF7.Identity;
using SQLiteIdentityEF7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.SQLite
{
    /// <summary>
    /// Class that is responsible for user roles
    /// </summary>
    public class UserRolesRepository
    {
        public IdentityEntities Context { get; private set; }

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        public UserRolesRepository(IdentityEntities context)
        {
            Context = context;
        }

        /// <summary>
        /// Returns user's role name
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public async Task<IList<string>> FindUserRolesByUserIdAsync(string userId)
        {

            var query = from userRole in Context.AspNetUserRoles
                        where userRole.UserId.Equals(userId)
                        join role in Context.AspNetRoles on userRole.RoleId equals role.Id
                        select role.Name;

            /* var query2 = await Context.AspNetUserRoles.
               Where(u => u.UserId.Equals(userId)).
               Select(r => r.RoleId).ToListAsync(); */

            /* var roles = await Context.AspNetRoles.Join(query,
                                                                  r => r.Id,
                                                                  ur => ur.,
                                                                  (r, ur) => new { Name = r.Name });*/


            return await query.ToListAsync();

        }

        /// <summary>
        /// Sets to user given role ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task SetUserRoleAsync(string userId, string roleId)
        {
            if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(roleId))
            {
                var entity = new AspNetUserRoles()
                {
                    UserId = userId,
                    RoleId = roleId
                };

                Context.AspNetUserRoles.Add(entity);
                await Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Sets roleId of a given user to null
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task RemoveUserRoleAsync(string userId, string roleName)
        {
            var roleEntity = await Context.AspNetRoles.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper());
            if (roleEntity != null)
            {
                var roleId = roleEntity.Id;
                var userRole = await Context.AspNetUserRoles.FirstOrDefaultAsync(r => roleId.Equals(r.RoleId) && r.UserId.Equals(userId));
                if (userRole != null)
                {
                    Context.AspNetUserRoles.Remove(userRole);
                    await Context.SaveChangesAsync();
                }
            }
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}