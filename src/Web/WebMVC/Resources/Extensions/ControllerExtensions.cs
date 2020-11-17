using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Resources.Extensions
{
    public static class ControllerExtensions
    {
        public static ToastMessage AddToastMessage(this Controller controller, string title, string message,
            ToastType toastType = ToastType.Info)
        {
            var toastr = new Toastr();

            toastr = toastr ?? new Toastr();

            var toastMessage = toastr.AddToastMessage(title, message, toastType);

            controller.TempData.SerializeAlerts("Toastr", toastr);

            return toastMessage;
        }
    }
}