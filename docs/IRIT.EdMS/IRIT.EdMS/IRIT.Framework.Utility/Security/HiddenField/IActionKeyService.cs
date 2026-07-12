namespace IRIT.Framework.Utility.Security.HiddenField
{
    interface IActionKeyService
    {
        /// <summary>
        /// پیدا کردن کلید متناظر هر ویو.ایجاد کلید جدید در صورت عدم وجود کلید در سیستم
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        string GetActionKey(string action, string controller, string area = "");
    }
}
