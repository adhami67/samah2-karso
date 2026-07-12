using System;
using System.Collections.Generic;
using System.Linq;

namespace IRIT.Framework.Utility.Security.HiddenField
{
    public class ActionKeyService : IActionKeyService
    {

        private static readonly IList<ActionKey> ActionKeys;

        static ActionKeyService()
        {
            ActionKeys = new List<ActionKey>
           {
               new ActionKey
               {
                   Area = "",
                   Controller = "Product",
                   Action = "dit",
                   ActionKeyValue = "E702E4C2-A3B9-446A-912F-8DAC6B0444BC",
               }
           };
        }

        /// <summary>
        /// پیدا کردن کلید متناظر هر ویو.ایجاد کلید جدید در صورت عدم وجود کلید در سیستم
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public string GetActionKey(string action, string controller, string area = "")
        {
            area = area ?? "";
            var actionKey = ActionKeys.FirstOrDefault(a =>
                a.Action.ToLower() == action.ToLower() &&
                a.Controller.ToLower() == controller.ToLower() &&
                a.Area.ToLower() == area.ToLower());
            return actionKey != null ? actionKey.ActionKeyValue : AddActionKey(action, controller, area);
        }

        /// <summary>
        /// اضافه کردن کلید جدید به سیستم
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private static string AddActionKey(string action, string controller, string area = "")
        {
            var actionKey = new ActionKey
            {
                Action = action,
                Controller = controller,
                Area = area,
                ActionKeyValue = Guid.NewGuid().ToString()
            };
            ActionKeys.Add(actionKey);
            return actionKey.ActionKeyValue;
        }

    }
}
