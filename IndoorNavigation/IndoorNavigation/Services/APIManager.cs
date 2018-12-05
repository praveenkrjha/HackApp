using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IndoorNavigation.Services
{
    class APIManager
    {
        public static RestService restService = new RestService();
        public async static Task<string> ValidateUser(string MailId)
        {
            try
            {
                return await restService.GetRequestRawAsync(AppConstants.BaseUrl + "");
            }
            catch
            {
                return null;
            }
        }
    }
}
