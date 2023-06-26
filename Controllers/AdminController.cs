using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationPlugin;
using HotelBooking.Data;
using HotelBooking.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    //[Authorize(Roles="Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly HotelBookingDbContext context;
        public AdminController(HotelBookingDbContext hotelBookingDbContext)
        {
            context = hotelBookingDbContext;
        }
        //[Route("GetUserDetails")]
        [HttpGet]
        public IActionResult GetUserDetails()
        {
            var info = context.UserRegistrations.ToList();
            if (info.Count == 0)
            {
                return NotFound("User does not exist");
            }
            return Ok(info);
        }
        // [Route("GetUserDetailsById")]
        [HttpGet("{UserId}")]
        public IActionResult GetUserDetailsById(int UserId)
        {
            var info = context.UserRegistrations.Find(UserId);
            if (info == null)
            {
                return NotFound("Id does not exist");
            }
            return Ok(info);
        }
        //[Route(" DeleteUser")]
        [HttpDelete]
        public IActionResult DeleteUser(int UserId)
        {
            var info = context.UserRegistrations.Find(UserId);
            if (info == null)
            {
                return NotFound("Id does not exist");
            }
            context.UserRegistrations.Remove(info);
            context.SaveChanges();

            return Ok("User deleted Successfully");
        }


        //[Route("GetHotels")]
        [HttpGet]
        //[Route("api/user/[controller]")]
        public IActionResult GetHotels()
        {
            var info = context.ListOfHotels.ToList();
            if (info.Count == 0)
            {
                return NotFound("Hotels are  not available");
            }
            return Ok(info);
        }
        //  [Route("GetHotelsById")]
        [HttpGet("{id}")]

        public IActionResult GetHotelById(int id)
        {
            var info = context.ListOfHotels.Find(id);
            if (info == null)
            {
                return NotFound("Hotel Does not Exist");
            }
            return Ok(info);
        }
        // [Route("AddHotels")]
        [HttpPost]
        public IActionResult AddHotels(Hotels hotel)
        {
            context.ListOfHotels.Add(hotel);
            context.SaveChanges();
            return Ok("Hotels added successfully");
        }
        // [Route("UpadateHotels")]
        [HttpPut]
        public IActionResult UpdateHotels(Hotels hotel)
        {
            if (hotel == null || hotel.HotelsId == 0)
            {
                return BadRequest("Data is invaild");
            }
            else if (hotel.HotelsId == 0)
            {
                return BadRequest("Hotelname is invaild");
            }
            var info = context.ListOfHotels.Find(hotel.HotelsId);
            if (info == null)
            {
                return NotFound(" Does not Exist ");
            }

            info.Imagelink = hotel.Imagelink;
            info.Hotelname = hotel.Hotelname;
            info.Place = hotel.Place;
            info.Fromdate = hotel.Fromdate;
            info.Todate = hotel.Todate;
            info.Noofperson = hotel.Noofperson;
            context.SaveChanges();
            return Ok("Hotel added successfully");
        }
        // [Route("DeleteHotels")]
        [HttpDelete]
        public IActionResult DeleteHotels(int HotelsId)
        {
            var info = context.ListOfHotels.Find(HotelsId);
            if (info == null)
            {
                return NotFound(" Does not Exist ");
            }
            context.ListOfHotels.Remove(info);
            context.SaveChanges();
            return Ok("hotel deleted successfully");
        }


        //[Route("GetBookings")]
        [HttpGet]
        public IActionResult GetBookings()
        {
            var info = context.BookedRoomDetails.ToList();
            if (info.Count == 0)
            {
                return NotFound("Bookings are not available");
            }
            return Ok(info);
        }
    }
}

