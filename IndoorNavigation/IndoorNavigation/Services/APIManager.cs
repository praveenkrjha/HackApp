using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IndoorNavigation.Services
{
    class APIManager
    {
        public static RestService restService = new RestService();
        //public async static Task<string> ValidateUser(ValidateUserInput ValidateUserInput)
        //{
        //    try
        //    {
        //        var validateUserJson = JsonConvert.SerializeObject(ValidateUserInput);
        //        return await restService.PostRequestRaw(AppConstants.BaseUrl + "ValidateUser", validateUserJson);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public async static Task<string> RegisterUser(string PassKey)
        {
            try
            {
                return await restService.GetRequestRawAsync(AppConstants.BaseUrl + "ValidateUser");
            }
            catch
            {
                return null;
            }
        }
    }
}
