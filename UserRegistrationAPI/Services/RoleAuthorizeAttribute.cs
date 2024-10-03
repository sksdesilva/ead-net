using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace UserRegistrationAPI.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RoleAuthorizeAttribute : TypeFilterAttribute
    {
        public RoleAuthorizeAttribute(string role) : base(typeof(RoleAuthorizeFilter))
        {
            Arguments = new object[] { role };
        }
    }
}
