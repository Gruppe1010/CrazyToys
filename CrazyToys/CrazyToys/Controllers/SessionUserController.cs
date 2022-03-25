using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpDelete]
        public async Task<ActionResult<SessionUser>> RemoveToyFromSessionUser([FromQuery(Name = "id")] string id)
        {
            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            sessionUser.Cart.Remove(id);
            _sessionService.Update(HttpContext, sessionUser);

            if (!sessionUser.Cart.ContainsKey(id))
            {
                return Ok(JsonConvert.SerializeObject(sessionUser));
            }

            return StatusCode(500);
        }


        [HttpPost]
        public async Task<ActionResult<ShoppingCartToyDTO>> IncOrDecToyFromCart([FromBody] SelectedToy selectedToy)
        {
            // TODO måske få noget shownQuantity ind - ift. hvis den nu ikke skal vises

            // find den pågældende sessionsUser
            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            string toyId = selectedToy.ToyId;
            int chosenQuantity = selectedToy.QuantityToAdd;
            int newQuantity = selectedToy.OldAvailableQuantity + chosenQuantity;

            // hent toy'et som tilsvarer til id
            Toy toy = await _toyDbService.GetById(toyId);

            if (toy != null)
            {
                ShoppingCartToyDTO shoppingCartToyDTO = toy.ConvertToShoppingCartToyDTO(newQuantity);

                // vi sikrer at varen faktisk er i cart
                if (sessionUser.Cart.ContainsKey(toyId))
                {
                    // tjek om den nye mængde er tilladt ift. stock
                    if (toy.Stock >= newQuantity)
                    {
                        // tilføje til eksisterende key-value pair med det selectedToy
                        sessionUser.Cart[toyId] = newQuantity;
                        _sessionService.Update(HttpContext, sessionUser);

                        return Ok(shoppingCartToyDTO);
                    }
                    else if(chosenQuantity < 0)
                    {
                        sessionUser.Cart[toyId] = newQuantity;
                        _sessionService.Update(HttpContext, sessionUser);

                        return BadRequest(shoppingCartToyDTO);
                    }
                }
                // tjek om toy'ets stock er >= den valgte amount
                else if (toy.Stock >= chosenQuantity)
                {
                    // tilføj nyt key-value pair
                    sessionUser.Cart.Add(toyId, chosenQuantity);
                    _sessionService.Update(HttpContext, sessionUser);

                    return Ok(shoppingCartToyDTO);
                }
                // hvis den ikke bliver incrementet, så skal quantity sættes til at være toy.Stock,
                // så vi kan bruge den data til at fortælle brugeren hvor mange der er på lager
                shoppingCartToyDTO.Quantity = toy.Stock;
                return BadRequest(shoppingCartToyDTO);
            }

            // Bad request fordi den beder om noget som vi ikke kan gøre 
            return BadRequest();
        }


        [HttpPost]
        public async Task<ActionResult<SelectedToy>> AddToCart([FromBody] SelectedToy selectedToy)
        {
            string toyId = selectedToy.ToyId;
            int chosenQuantity = selectedToy.QuantityToAdd;

            // find den pågældende sessionsUser
            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            // hent toy'et som tilsvarer til id
            Toy toy = await _toyDbService.GetById(toyId);

            // hvis den allerede har den type toy i sin cart
            if (sessionUser.Cart.ContainsKey(toyId))
            {
                // tjek om den mængde som er i cart'en + den valgte nye mængde stadig er <= toy.Stock
                if (toy.Stock >= sessionUser.Cart[toyId] + chosenQuantity)
                {
                    // hvis chosenQuantity er minustal og det bliver til 0 vi fjerner toyet
                    if (sessionUser.Cart[toyId] + chosenQuantity == 0)
                    {
                        sessionUser.Cart.Remove(toyId);
                    }
                    else
                    {
                        // tilføje til eksisterende key-value pair med det selectedToy
                        sessionUser.Cart[toyId] = sessionUser.Cart[toyId] + chosenQuantity;
                    }

                    _sessionService.Update(HttpContext, sessionUser);

                    selectedToy.QuantityToAdd = selectedToy.QuantityToAdd + chosenQuantity;

                    return Ok(selectedToy);
                }
            }
            // tjek om toy'ets stock er >= den valgte amount
            else if (toy.Stock >= chosenQuantity)
            {
                // tilføj nyt key-value pair
                sessionUser.Cart.Add(toyId, chosenQuantity);
                _sessionService.Update(HttpContext, sessionUser);

                return Ok(selectedToy);
            }

            // Bad request fordi den beder om noget som vi ikke kan gøre 
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<SelectedToy>> AddToyToSessionUsersWishlist([FromBody] SelectedToy selectedToy)
        {
            string toyId = selectedToy.ToyId;

            // find den pågældende sessionsUser
            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            // hent toy'et som tilsvarer til id
            Toy toy = await _toyDbService.GetById(toyId);

            // tilføj hvis den ikke allerede har den type toy på wishlist
            if (!sessionUser.Wishlist.Contains(toyId))
            {
                sessionUser.Wishlist.Add(toyId);
                _sessionService.Update(HttpContext, sessionUser);

                return Ok(selectedToy);
            }

            // Bad request fordi den beder om noget som vi ikke kan gøre 
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult<SessionUser>> RemoveToyFromSessionUsersWishlist([FromQuery(Name = "id")] string toyId)
        {
            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            sessionUser.Wishlist.Remove(toyId);
            _sessionService.Update(HttpContext, sessionUser);

            if (!sessionUser.Wishlist.Contains(toyId))
            {
                return Ok(JsonConvert.SerializeObject(sessionUser));
            }

            return StatusCode(500);
        }
    }
}
