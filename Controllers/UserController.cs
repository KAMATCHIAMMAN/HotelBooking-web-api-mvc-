using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationPlugin;
using HotelBooking.Data;
using HotelBooking.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HotelBookingDbContext context;
        public UserController(HotelBookingDbContext hotelBookingDbContext)
        {
            context = hotelBookingDbContext;
        }
        [HttpPost]
        public IActionResult AddBookings(BookingDetails bookedRoomDetails)
        {
            context.BookedRoomDetails.Add(bookedRoomDetails);
            context.SaveChanges();
            return Ok("Bookings added successfully");
        }
        // [Route("AddCardDetails")]
        [HttpPost]
        public IActionResult AddCardDetails(CardDetails creditCardDetails)
        {
            var details = new CardDetails
            {

                Date = creditCardDetails.Date,
                NameOnCard = creditCardDetails.NameOnCard,
                Cardnumber = SecurePasswordHasherHelper.Hash(creditCardDetails.Cardnumber),
                Month = creditCardDetails.Month,
                year = creditCardDetails.year,
                Cvv = SecurePasswordHasherHelper.Hash(creditCardDetails.Cvv),
                UsersId = creditCardDetails.UsersId
            };
            context.CardDetails.Add(details);
            context.SaveChangesAsync();
            return Ok("Card details added successfully");
        }

        [HttpGet]
        public IActionResult GetHotels()
        {
            var info = context.ListOfHotels.ToList();
            if (info.Count == 0)
            {
                return NotFound("Hotel is not available");
            }
            return Ok(info);
        }
        [HttpPost]
        public  IActionResult AddRegisteredUser(UserRegistration userRegistration)
        {
            context.UserRegistrations.Add(userRegistration);
            context.SaveChanges();
            return Ok("User Added successfully");
        }
    }
}


