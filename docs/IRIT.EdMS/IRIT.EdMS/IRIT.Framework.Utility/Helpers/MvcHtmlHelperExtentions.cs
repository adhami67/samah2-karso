using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using IRIT.Framework.Utility.Security;
using IRIT.Framework.Utility.Security.HiddenField;

namespace IRIT.Framework.Utility.Helpers
{
    public static class MvcHtmlHelperExtentions
    {
        #region Input FormControl
        public static MvcHtmlString NoAutoCompleteTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.TextBoxFor(expression, new { @class = "form-control", autocomplete = "off" });
        }
        public static MvcHtmlString NoAutoCompleteTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return html.TextBoxFor(expression, htmlAttributes);
        }
        public static MvcHtmlString NoAutoCompleteTextBoxForLtr<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.TextBoxFor(expression, new { @class = "form-control", autocomplete = "off", dir = "ltr" });
        }
        public static MvcHtmlString FormControlTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.TextBoxFor(expression, new { @class = "form-control" });
        }
        public static MvcHtmlString FormControlPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.PasswordFor(expression, new { @class = "form-control" });
        }
        #endregion

        #region Encrypted Inputs
        public static MvcHtmlString EncryptedHidden(this HtmlHelper helper, string name, object value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            var strValue = value.ToString();
            IEncryptSettingsProvider settings = new EncryptSettingsProvider();
            var encrypter = new RijndaelStringEncrypter(settings, helper.GetActionKey());
            var encryptedValue = encrypter.Encrypt(strValue);
            encrypter.Dispose();

            var encodedValue = helper.Encode(encryptedValue);
            var newName = string.Concat(settings.EncryptionPrefix, name);

            return helper.Hidden(newName, encodedValue);
        }

        public static MvcHtmlString EncryptedHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return EncryptedHidden(htmlHelper, name, metadata.Model);
        }
        #endregion

        #region Security (GetActionKey)
        public static string GetActionKey(this System.Web.Routing.RequestContext requestContext)
        {
            IActionKeyService actionKeyService = new ActionKeyService();
            var action = requestContext.RouteData.Values["Action"].ToString();
            var controller = requestContext.RouteData.Values["Controller"].ToString();
            var area = requestContext.RouteData.Values["Area"];
            var actionKeyValue = actionKeyService.GetActionKey(
                            action, controller, area != null ? area.ToString() : null);

            return actionKeyValue;
        }

        public static string GetActionKey(this HtmlHelper helper)
        {
            IActionKeyService actionKeyService = new ActionKeyService();
            var action = helper.ViewContext.RouteData.Values["Action"].ToString();
            var controller = helper.ViewContext.RouteData.Values["Controller"].ToString();
            var area = helper.ViewContext.RouteData.Values["Area"];
            var actionKeyValue = actionKeyService.GetActionKey(
                            action, controller, area != null ? area.ToString() : null);

            return actionKeyValue;
        }
        #endregion

    }
}
