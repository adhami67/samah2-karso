using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRIT.Framework.Utility.View
{
    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual Security.EdmsPrincipal User
        {
            get
            {
                var serializer = new JavaScriptSerializer();
                var data = ((System.Web.Security.FormsIdentity) base.User.Identity).Ticket.UserData;
                return serializer.Deserialize<Security.EdmsPrincipal>(data);
            }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual Security.EdmsPrincipal User
        {
            get
            {
                var serializer = new JavaScriptSerializer();
                var data = ((System.Web.Security.FormsIdentity)base.User.Identity).Ticket.UserData;
                return serializer.Deserialize<Security.EdmsPrincipal>(data);
            }
        }
    }
}