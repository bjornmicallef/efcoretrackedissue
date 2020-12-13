using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly IUserSettingsService _userSettingsService;

        public SettingsController(ILogger<SettingsController> logger, IUserSettingsService userSettingsService)
        {
            _logger = logger;
            _userSettingsService = userSettingsService;
        }

        // this will get a list of sellers the user would like to receive notifications from
        [HttpGet]
        public ObjectResult GetUserSellers()
        {
            return Ok(_userSettingsService.GetUserExcludedSellerPreferences("bjorn").Result);
        }

        // this will get a list of sellers
        [HttpGet("sellers")]
        public ObjectResult GetSellers()
        {
            return Ok(_userSettingsService.GetSellers().Result);
        }

        // this will update the user's notifications
        [HttpPut]
        public ObjectResult UpdateUserSellers(IEnumerable<int> selectedSellerIds)
        {
            _userSettingsService.UpdateUserSellerPreferences("bjorn", selectedSellerIds);
            return Ok("Updated");
        }

        [HttpGet("test")]
        public ObjectResult UpdateUserSellersTest()
        {
            var sellerIds = new List<int>() { 3, 4 };

            _userSettingsService.UpdateUserSellerPreferences("bjorn", sellerIds);
            return Ok("Updated");
        }
    }
}
