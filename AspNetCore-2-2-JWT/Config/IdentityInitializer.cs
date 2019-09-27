using AspNetCore_2_2_JWT.Data;
using AspNetCore_2_2_JWT.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore_2_2_JWT.Config
{
    public class IdentityInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public IdentityInitializer(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                CreateRole(Roles.ROLE_API_ADM);
                CreateRole(Roles.ROLE_API_BASIC);
            }
        }

        private void CreateRole(string role)
        {
            if (!_roleManager.RoleExistsAsync(role).Result)
            {
                IdentityRole identityRole = new IdentityRole(role);
                var resultado = _roleManager.CreateAsync(
                    identityRole).Result;
                if (!resultado.Succeeded)
                {
                    throw new Exception(
                        $"Erro durante a criação da role {role}.");
                }

            }
        }
    }
}
