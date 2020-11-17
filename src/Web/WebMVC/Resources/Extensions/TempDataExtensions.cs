using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using WebMVC.Resources;

public static class TempDataExtensions
{
    public static void SerializeAlerts(this ITempDataDictionary tempData, string alertKeyName, Toastr alert)
    {
        tempData[alertKeyName] = JsonConvert.SerializeObject(alert);
    }

    public static Toastr DeserializeAlerts(this ITempDataDictionary tempData, string alertKeyName)
    {
        var alert = new Toastr();
        if (tempData.ContainsKey(alertKeyName))
        {
            alert = JsonConvert.DeserializeObject<Toastr>(tempData[alertKeyName].ToString());
        }
        return alert;
    }
}