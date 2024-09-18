using FirstApiTry.Models;
using FirstApiTry.TouristModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirstApiTry.TouristModels;
namespace FirstApiTry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Tourist>> Get()
        {
            try
            {
                var tourists = TouristDBServices.GetAllTourists();
                return Ok(tourists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
            try
            {
                var tourist = TouristDBServices.GetTouristById(id);
                if (tourist == null)
                {
                    return NotFound($"Tourist with id = {id} was not found.");
                }
                return Ok(tourist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginData loginData)
        {
            try
            {
                var tourist = TouristDBServices.LoginTourist(loginData.Email, loginData.Pass);
                if (tourist == null)
                {
                    return NotFound("Invalid email or password.");
                }
                return Ok(tourist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteTourist(int id)
        {
            try
            {
                var tourist = TouristDBServices.GetTouristById(id);
                if (tourist == null)
                {
                    return NotFound($"Tourist with id = {id} was not found.");
                }

                
                TouristDBServices.DeleteTourist(id);
                return NoContent();  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateTourist(int id, [FromBody] Tourist tourist)
        {
            try
            {
                if (tourist == null || id != tourist.TouristId)
                {
                    return BadRequest("Invalid tourist data.");
                }

                var existingTourist = TouristDBServices.GetTouristById(id);
                if (existingTourist == null)
                {
                    return NotFound($"Tourist with id = {id} was not found.");
                }

                
                TouristDBServices.UpdateTourist(id, tourist);
                return NoContent();  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RegisterTourist([FromBody] Tourist tourist)
        {
            try
            {
                if (tourist == null)
                {
                    return BadRequest("Invalid tourist data.");
                }

                
                var existingTourist = TouristDBServices.GetAllTourists().FirstOrDefault(t => t.Email == tourist.Email);
                if (existingTourist != null)
                {
                    return BadRequest("Email is already registered.");
                }

                
                TouristDBServices.RegisterTourist(tourist);
                return CreatedAtAction(nameof(Get), new { id = tourist.TouristId }, tourist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{touristId}/reviews/{guideId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Review))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetReview(int touristId, int guideId)
        {
            try
            {
                var review = TouristDBServices.GetReview(touristId, guideId);
                if (review == null)
                {
                    return NotFound($"Review for guide with id = {guideId} and tourist with id = {touristId} was not found.");
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{touristId}/reviews/{guideId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddReview(int touristId, int guideId, [FromBody] Review review)
        {
            try
            {
                if (review == null)
                {
                    return BadRequest("Review details are missing.");
                }

                
                TouristDBServices.AddReview(touristId, guideId, review.Rating, review.Comment);

                return CreatedAtAction(nameof(GetReview), new { touristId = touristId, guideId = guideId }, review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("bookings")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Booking>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllBookings()
        {
            try
            {
                var bookings = TouristDBServices.GetAllBookings();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{bookingId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Booking))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetBooking(int bookingId)
        {
            try
            {
                var booking = TouristDBServices.GetBooking(bookingId);
                if (booking == null)
                {
                    return NotFound($"Booking with id = {bookingId} was not found.");
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addBooking")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddBooking([FromBody] Booking booking)
        {
            try
            {
                if (booking == null)
                {
                    return BadRequest("Booking details are missing.");
                }

                
                TouristDBServices.AddBooking(booking);

                return CreatedAtAction(nameof(GetBooking), new { bookingId = booking.BookingId }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("booking/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                var booking = TouristDBServices.GetBooking(id);
                if (booking == null)
                {
                    return NotFound($"Booking with id = {id} was not found.");
                }

                
                TouristDBServices.DeleteBooking(id);
                return NoContent();  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
