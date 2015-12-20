namespace FinanceTracker.WebApi.AspApi.Controllers
{
    using System;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    public abstract class Common : ApiController
    {
        protected string UserId
        {
            get { return base.User.Identity.GetUserId(); }
        }

        protected string FormatCategoryName(string name)
        {
            var result = string.Format("{0}{1}", Char.ToUpper(name[0]), name.Substring(1).ToLower());

            return result;
        }
    }
}