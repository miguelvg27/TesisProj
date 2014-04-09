using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using TesisProj.Models;
using System.Web.Security;

namespace TesisProj.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection(TesisProj.MvcApplication.ConnectionString, "UserProfile", "UserId", "UserName", autoCreateTables: true);

                    if (!Roles.RoleExists("sadmin")) Roles.CreateRole("sadmin");
                    if (!Roles.RoleExists("admin")) Roles.CreateRole("admin");
                    if (!Roles.RoleExists("nav")) Roles.CreateRole("nav");

                    if (!WebSecurity.UserExists("miguelavg"))
                    {
                        WebSecurity.CreateUserAndAccount("miguelavg", "miguelavg");
                        Roles.AddUserToRole("miguelavg", "sadmin");
                        Roles.AddUserToRole("miguelavg", "admin");
                        Roles.AddUserToRole("miguelavg", "nav");
                    }

                    if (!WebSecurity.UserExists("pedrocg"))
                    {
                        WebSecurity.CreateUserAndAccount("pedrocg", "pedrocg");
                        Roles.AddUserToRole("pedrocg", "nav");
                    }

                    if (!WebSecurity.UserExists("marianadc"))
                    {
                        WebSecurity.CreateUserAndAccount("marianadc", "marianadc");
                        Roles.AddUserToRole("marianadc", "admin");
                        Roles.AddUserToRole("marianadc", "nav");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
