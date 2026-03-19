// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4.Contrib.Membership.IdsvrDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential(persistKey: false)
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(IdentityResources.Get())
                .AddMembershipService(new MembershipOptions
                {
                    ConnectionString = "Data Source=localhost;Initial Catalog=Membership;Integrated Security=True",
                    ApplicationName = "Test",
                    UseRoleProviderSource = true,
                    MaxInvalidPasswordAttempts = 5
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
                {
                    new Client
                    {
                        ClientName = "ServiceStack.SelfHost",
                        ClientId = "ServiceStack.SelfHost",
                        Enabled = true,

                        AllowedGrantTypes = GrantTypes.Hybrid,

                        AccessTokenType = AccessTokenType.Jwt,

                        ClientSecrets = new List<Secret>
                        {
                            new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                        },

                        AllowOfflineAccess = true,

                        RedirectUris = new List<string>
                        {
                            "https://localhost:44321/signin-oidc"
                        },

                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,

                            "ServiceStack.SelfHost",
                        },

                        RequireConsent = false,
                        RequirePkce = false
                    }
                };
        }
    }

    static class IdentityResources
    {
        public static List<IdentityResource> Get()
        {
            return new List<IdentityResource>
            {
                new Duende.IdentityServer.Models.IdentityResources.OpenId(),
                new Duende.IdentityServer.Models.IdentityResources.Profile(),
                new Duende.IdentityServer.Models.IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "ServiceStack.SelfHost",
                    Enabled = true,
                    UserClaims = new List<string>
                    {
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Name
                    }
                }
            };
        }
    }
}
