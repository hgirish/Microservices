using Microsoft.AspNetCore.Mvc;

namespace LoyaltyProgramServiceTests.Mocks
{
    public class NotificationsMock : ControllerBase
    {
        public static bool ReceiveNotification = false;

        [HttpPost("/notify")]
        public OkResult Notify()
        {
            ReceiveNotification = true;
            return Ok();
        }
    }
}
