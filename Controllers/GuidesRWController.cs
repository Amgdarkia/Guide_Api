using FirstApiTry.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FirstApiTry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidesRWController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Guide>> Get()
        {
            try
            {
                var guides = DBServices.GetGuides();
                return Ok(guides);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guide))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
            try
            {
                var guide = DBServices.GetGuideById(id);
                if (guide == null)
                {
                    return NotFound($"Guide with id = {id} was not found!");
                }
                return Ok(guide);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guide))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] Guide guide)
        {
            try
            {
                if (guide == null)
                {
                    return BadRequest("Guide object is null");
                }
                DBServices.RegisterGuide(guide);
                return CreatedAtAction(nameof(Get), new { id = guide.ID }, guide);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Guide guide)
        {
            try
            {
                if (guide == null || guide.ID != id)
                {
                    return BadRequest();
                }
                DBServices.UpdateGuide(id, guide);
                return NoContent();
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
        public IActionResult Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid ID");
                }
                DBServices.DeleteGuide(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guide))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginData ld)
        {
            try
            {
                Guide gui = DBServices.Login(ld.Email, ld.Pass);
                if (gui != null)
                {
                    return Ok(gui);
                }
                else
                {
                    return NotFound($"Guide with email = {ld.Email} and password not found!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("routes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Models.Route>> GetAllRoutes()
        {
            try
            {
                var routes = DBServices.GetAllRoutes(); 
                return Ok(routes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("routes/{routeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Route))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetRouteById(int routeId)
        {
            try
            {
                var route = DBServices.GetRouteById(routeId);
                if (route == null)
                {
                    return NotFound($"Route with id = {routeId} not found.");
                }
                return Ok(route);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}/routes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.Route>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetRoutesByGuideId(int id)
        {
            try
            {
                var routes = DBServices.GetRoutesByGuideId(id);
                if (routes == null || routes.Count == 0)
                {
                    return NotFound($"No routes found for guide with id = {id}");
                }
                return Ok(routes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Review>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetReviewsByGuideId(int id)
        {
            try
            {
                var reviews = DBServices.GetReviewsByGuideId(id);
                if (reviews == null || reviews.Count == 0)
                {
                    return NotFound($"No reviews found for guide with id = {id}");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/routes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddRoute(int id, [FromBody] Models.Route route)
        {
            try
            {
                if (route == null)
                {
                    return BadRequest("Route details are missing");
                }

                
                DBServices.AddRoute(id, route.Description, route.Duration, route.DifficultyLevel, route.StartPoint, route.EndPoint, route.RouteType);

                return CreatedAtAction(nameof(GetRoutesByGuideId), new { id = id }, route);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{guideId}/routes/{routeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteRoute(int guideId, int routeId)
        {
            try
            {
                
                DBServices.DeleteRoute(guideId, routeId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{guideId}/routes/{routeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateRoute(int guideId, int routeId, [FromBody] Models.Route route)
        {
            try
            {
                if (route == null)
                {
                    return BadRequest("Route details are missing");
                }

                // Call the DB service to update the route
                DBServices.UpdateRoute(guideId, routeId, route.Description, route.Duration, route.DifficultyLevel, route.StartPoint, route.EndPoint, route.RouteType);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
