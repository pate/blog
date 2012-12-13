using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class Controllers
    {
        public static void FlashInfo(this ControllerBase controller, string infoMessage)
        {
            List<string> messages = controller.TempData["InfoMessages"] as List<string> ?? new List<string>();
            messages.Add(infoMessage);
            controller.TempData["InfoMessages"] = messages;
        }

        public static void FlashWarning(this ControllerBase controller, string warningMessage)
        {
            List<string> messages = controller.TempData["WarningMessages"] as List<string> ?? new List<string>();
            messages.Add(warningMessage);
            controller.TempData["WarningMessages"] = messages;
        }

        public static void FlashError(this ControllerBase controller, string errorMessage)
        {
            List<string> messages = controller.TempData["ErrorMessages"] as List<string> ?? new List<string>();
            messages.Add(errorMessage);
            controller.TempData["ErrorMessages"] = messages;
        }
    }
}