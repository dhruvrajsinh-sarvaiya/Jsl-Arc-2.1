﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Culture;
using CleanArchitecture.Core.Entities.Resource;
using CleanArchitecture.Core.Entities.User;
using CleanArchitecture.Core.Interfaces.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenIddict.Core;
using OpenIddict.Models;

namespace CleanArchitecture.Infrastructure
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync(IConfiguration configuration);
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly CleanArchitectureContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _openIddictApplicationManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(
            CleanArchitectureContext context,
            ILogger<DatabaseInitializer> logger,
            OpenIddictApplicationManager<OpenIddictApplication> openIddictApplicationManager,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IRegisterTypeService registerTypeService
            )
        {
            _context = context;
            _logger = logger;
            _openIddictApplicationManager = openIddictApplicationManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _registerTypeService = registerTypeService;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            CreateRoles();
            CreateUsers();
            AddLocalisedData();
            await AddOpenIdConnectOptions(configuration);
            AddWalletType(); // ntrivedi 01-10-2018 added default wallettype master entry
            CreateRegisterType(); // Birju 02-10-2018  added by default Register type add;
        }

        private void CreateRegisterType()
        {
            var TypeToAdd = new List<RegisterType>(){
                new RegisterType { CreatedDate = DateTime.Now, CreatedBy= 1,Type="Mobile",ActiveStatus=true,IsDeleted = false},
                new RegisterType { CreatedDate = DateTime.Now, CreatedBy= 1,Type="Email",ActiveStatus=true,IsDeleted = false},
                new RegisterType { CreatedDate = DateTime.Now, CreatedBy= 1,Type="Standerd",ActiveStatus=true,IsDeleted = false},
            };
            foreach (var Type in TypeToAdd)
            {
                if (!_registerTypeService.GetRegisterType(Type.Type).Result)
                {
                    _registerTypeService.AddRegisterType(Type);
                }
            }
        }

        private void CreateRoles()
        {
            var rolesToAdd = new List<ApplicationRole>(){
                new ApplicationRole { Name= "Admin", Description = "Full rights role"},
                new ApplicationRole { Name= "User", Description = "Limited rights role"}
            };
            foreach (var role in rolesToAdd)
            {
                if (!_roleManager.RoleExistsAsync(role.Name).Result)
                {
                    _roleManager.CreateAsync(role).Result.ToString();
                }
            }
        }

        private void CreateUsers()
        {
            if (!_context.Users.Any())
            {
                var adminUser = new ApplicationUser { UserName = "admin@admin.com", FirstName = "Admin first", LastName = "Admin last", Email = "admin@admin.com", Mobile = "0123456789", EmailConfirmed = true, CreatedDate = DateTime.Now, IsEnabled = true };
                _userManager.CreateAsync(adminUser, "P@ssw0rd!").Result.ToString();
                _userManager.AddClaimAsync(adminUser, new Claim(OpenIdConnectConstants.Claims.PhoneNumber, adminUser.Mobile.ToString(), ClaimValueTypes.Integer)).Result.ToString();
                _userManager.AddToRoleAsync(_userManager.FindByNameAsync("admin@admin.com").GetAwaiter().GetResult(), "Admin").Result.ToString();

                var normalUser = new ApplicationUser { UserName = "user@user.com", FirstName = "First", LastName = "Last", Email = "user@user.com", Mobile = "0123456789", EmailConfirmed = true, CreatedDate = DateTime.Now, IsEnabled = true };
                _userManager.CreateAsync(normalUser, "P@ssw0rd!").Result.ToString();
                _userManager.AddClaimAsync(adminUser, new Claim(OpenIdConnectConstants.Claims.PhoneNumber, adminUser.Mobile.ToString(), ClaimValueTypes.Integer)).Result.ToString();
                _userManager.AddToRoleAsync(_userManager.FindByNameAsync("user@user.com").GetAwaiter().GetResult(), "User").Result.ToString();
            }
        }

        private void AddLocalisedData()
        {
            if (!_context.Cultures.Any())
            {
                _context.Cultures.AddRange(
                    new Cultures
                    {
                        Name = "en-US",
                        Resources = new List<Resources>() {
                            new Resources { Key = "app_title", Value = "AspNetCoreSpa" },
                            new Resources { Key = "app_description", Value = "Single page application using aspnet core and angular" },
                            new Resources { Key = "app_repo_url", Value = "https://github.com/asadsahi/aspnetcorespa" },
                            new Resources { Key = "app_nav_home", Value = "Home" },
                            new Resources { Key = "app_nav_signalr", Value = "SignalR" },
                            new Resources { Key = "app_nav_examples", Value = "Examples" },
                            new Resources { Key = "app_nav_register", Value = "Register" },
                            new Resources { Key = "app_nav_login", Value = "Login" },
                            new Resources { Key = "app_nav_logout", Value = "Logout" },
                        }
                    },
                    new Cultures
                    {
                        Name = "fr-FR",
                        Resources = new List<Resources>() {
                            new Resources { Key = "app_title", Value = "AspNetCoreSpa" },
                            new Resources { Key = "app_description", Value = "Application d'une seule page utilisant aspnet core et angular" },
                            new Resources { Key = "app_repo_url", Value = "https://github.com/asadsahi/aspnetcorespa" },
                            new Resources { Key = "app_nav_home", Value = "Accueil" },
                            new Resources { Key = "app_nav_signalr", Value = "SignalR" },
                            new Resources { Key = "app_nav_examples", Value = "Exemples" },
                            new Resources { Key = "app_nav_register", Value = "registre" },
                            new Resources { Key = "app_nav_login", Value = "S'identifier" },
                            new Resources { Key = "app_nav_logout", Value = "Connectez - Out" },
                        }
                    }
                    );

                _context.SaveChanges();
            }

        }
        
        private async Task AddOpenIdConnectOptions(IConfiguration configuration)
        {
            if (await _openIddictApplicationManager.FindByClientIdAsync("cleanarchitecture") == null)
            {
                var host = configuration["HostUrl"].ToString();

                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "cleanarchitecture",
                    DisplayName = "CleanArchitecture",
                    PostLogoutRedirectUris = { new Uri($"{host}signout-oidc") },
                    RedirectUris = { new Uri(host) },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Implicit,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken
                    }
                };

                await _openIddictApplicationManager.CreateAsync(descriptor);
            }

        }


        private void AddWalletType()
        {
            if (!_context.WalletTypeMasters.Any())
            {
                _context.WalletTypeMasters.AddRange(
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "BTC", Status = 1, WalletTypeName = "BTC" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "INR", Status = 1, WalletTypeName = "INR" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "LTC", Status = 1, WalletTypeName = "LTC" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "ETH", Status = 1, WalletTypeName = "ETH" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "XRP", Status = 1, WalletTypeName = "XRP" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "ATCC", Status = 1, WalletTypeName = "ATCC" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "ARISTO", Status = 1, WalletTypeName = "ARISTO" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "BCI", Status = 1, WalletTypeName = "BCI" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "BCH", Status = 1, WalletTypeName = "BCH" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "BTG", Status = 1, WalletTypeName = "BTG" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "CVC", Status = 1, WalletTypeName = "CVC" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "REP", Status = 1, WalletTypeName = "REP" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "OMG", Status = 1, WalletTypeName = "OMG" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "PAY", Status = 1, WalletTypeName = "PAY" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "GNT", Status = 1, WalletTypeName = "GNT" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "BAT", Status = 1, WalletTypeName = "BAT" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "KNC", Status = 1, WalletTypeName = "KNC" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "ZRX", Status = 1, WalletTypeName = "ZRX" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "FUN", Status = 1, WalletTypeName = "FUN" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "EOS", Status = 1, WalletTypeName = "EOS" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "QRL", Status = 1, WalletTypeName = "QRL" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "NMR", Status = 1, WalletTypeName = "NMR" },
                  new WalletTypeMaster { CreatedBy = 900, CreatedDate = DateTime.UtcNow, Discription = "LMX", Status = 1, WalletTypeName = "LMX" }
               );
                _context.SaveChanges();
            }
        }


    }
}