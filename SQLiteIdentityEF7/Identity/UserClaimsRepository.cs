﻿using Microsoft.Data.Entity;
using SQLiteIdentityEF7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.SQLite
{
    /// <summary>
    /// Class that represents the UserClaims table in the Database
    /// </summary>
    public class UserClaimsRepository
    {
        private IdentityEntities _identityEntities;
        
        /// <summary>
        /// Constructor that takes an IdentityEntities instance
        /// </summary>
        public UserClaimsRepository(IdentityEntities identityEntities)
        {
            _identityEntities = identityEntities;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance using user's Id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> FindByUserIdAsync(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                ClaimsIdentity claims = new ClaimsIdentity();
                List<AspNetUserClaims> foundClaims = await _identityEntities.AspNetUserClaims.Where(claim => claim.UserId == userId).ToListAsync();

                if (foundClaims != null && foundClaims.Count > 0)
                {
                    foreach (AspNetUserClaims claim in foundClaims)
                    {
                        claims.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
                    }
                }
                return claims;
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// Deletes all claims from a user using user's Id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public async Task DeleteClaimsAsync(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                List<AspNetUserClaims> foundClaim = await _identityEntities.AspNetUserClaims.Where(claim => claim.UserId == userId).ToListAsync();
                if (foundClaim != null)
                {
                    _identityEntities.AspNetUserClaims.RemoveRange(foundClaim);
                    await _identityEntities.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Adds a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's Id</param>
        /// <returns></returns>
        public async Task AddNewClaimsAsync(Claim userClaim, string userId)
        {
            if (userClaim != null && !String.IsNullOrEmpty(userId))
            {
                _identityEntities.AspNetUserClaims.Add(new AspNetUserClaims
                {
                    ClaimType = userClaim.Type,
                    ClaimValue = userClaim.Value,
                    UserId = userId
                });
                await _identityEntities.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public async Task DeleteClaimAsync(IdentityUser user, Claim claim)
        {
            if (user != null && claim != null)
            {
                AspNetUserClaims foundClaim = await _identityEntities.AspNetUserClaims
                    .SingleOrDefaultAsync(userClaim => userClaim.UserId == user.Id
                        && userClaim.ClaimType == claim.Type
                        && userClaim.ClaimValue == claim.Value);
                
                if (foundClaim != null)
                {
                    _identityEntities.AspNetUserClaims.Remove(foundClaim);
                    await _identityEntities.SaveChangesAsync();
                }
            }
        }
    }
}