using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Services;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CrazyToys.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionUserController : ControllerBase
    {

        private readonly ISessionService _sessionService;
        private readonly ToyDbService _toyDbService;

        public SessionUserController(ISessionService sessionService, ToyDbService toyDbService)
        {
            _sessionService = sessionService;
            _toyDbService = toyDbService;
        }


        [HttpGet]
        public async Task<ActionResult<SessionUser>> GetSessionUser()
        {
            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            return Ok(JsonConvert.SerializeObject(sessionUser));
        }


        [HttpPost]
        public async Task<ActionResult<SelectedToy>> AddToCart([FromBody] SelectedToy selectedToy)
        {
            string toyId = selectedToy.ToyId;
            int chosenQuantity = selectedToy.Quantity;

            // find den pågældende sessionsUser
            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            // hent toy'et som tilsvarer til id
            Toy toy = await _toyDbService.GetById(toyId);

            // tjek om toy'ets stock er >= den valgte amount
            if (toy.Stock >= chosenQuantity)
            {
                // hvis den allerede har den type toy i sin cart
                if (sessionUser.Cart.ContainsKey(toyId))
                {
                    // tjek om den mængde som er i cart'en + den valgte nye mængde stadig er <= toy.Stock
                    if (toy.Stock >= sessionUser.Cart[toyId] + chosenQuantity)
                    {
                        // tilføje til eksisterende key-value pair med det selectedToy
                        sessionUser.Cart[toyId] = sessionUser.Cart[toyId] + chosenQuantity;
                        _sessionService.Update(HttpContext, sessionUser);

                        selectedToy.Quantity = selectedToy.Quantity + chosenQuantity;

                        return Ok(selectedToy);
                    }
                }
                else // tilføj nyt key-value pair
                {
                    sessionUser.Cart.Add(toyId, chosenQuantity);
                    _sessionService.Update(HttpContext, sessionUser);

                    return Ok(selectedToy);
                }
            }
            // Bad request fordi den beder om noget som vi ikke kan gøre 
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<SelectedToy>> addOrRemoveFromWishlist([FromBody] SelectedToy selectedToy)
        {
            string toyId = selectedToy.ToyId;

            // find den pågældende sessionsUser
            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            // hent toy'et som tilsvarer til id
            Toy toy = await _toyDbService.GetById(toyId);

            // hvis den allerede har den type toy på wishlist
            if (sessionUser.Wishlist.Contains(toyId))
            {
                sessionUser.Wishlist.Remove(toyId);
            }
            else // tilføj nyt element på HashSet
            {
                sessionUser.Wishlist.Add(toyId);
                _sessionService.Update(HttpContext, sessionUser);

                return Ok(selectedToy);
            }
            // Bad request fordi den beder om noget som vi ikke kan gøre 
            return BadRequest();
        }
    }
}
