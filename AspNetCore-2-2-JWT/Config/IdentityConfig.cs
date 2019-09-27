using AspNetCore_2_2_JWT.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_2_2_JWT.Config
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection AddIdentityContextInMemoryConfiguration(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt => 
            opt.UseInMemoryDatabase("InMemoryDatabase"));
            return services;
        }
    }
}
