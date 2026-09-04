using Castle.Core.Internal;
using ETG.Data.Tour.Services;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.Booking.Requests;
using ETG.WebAPI.Models.Booking.Responses;
using ETG.WebAPI.Routing;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using ETG.Booking.Pricing.Services;
using ETG.Data.Promotion;
using ETG.Data.Tour;
using ETG.Module.Booking.Shopping;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("promotion")]
    public class PromotionController : ApiController
    {
        private readonly IBookingCart _bookingCart;
        private readonly ICurrencyService _currencyService;
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ITourService _tourService;
        
        public PromotionController(
            IBookingCart bookingCart,  
            ICurrencyService currencyService,
            IPromotionRepository promotionRepository, ITourService tourService)
        {
            _bookingCart = bookingCart;
            _currencyService = currencyService;
            _currentCurrencyPricing = new CurrentCurrencyPricing(_currencyService);
            _promotionRepository = promotionRepository;
            _tourService = tourService;
        }

        [Route("apply-code")]
        [HttpGet]
        public async Task<IHttpActionResult> SubmitRoomOptionsStep(string code)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (code.IsNullOrEmpty())
            {
                baseResponse.Message = "No promo code specified.";
                return Ok(baseResponse);
            }

            if (_bookingCart.CartIsEmpty)
            {
                baseResponse.Message = "Cart is empty.";
                return Ok(baseResponse);
            }
            
            var tour = _tourService.GetTourByTourCode(_bookingCart.CurrentTourCode);
            
            if (tour == null)
            {
                baseResponse.Message = "Bad Request.";
                return Ok(baseResponse);
            }
            
            
            
            var promotion =  _promotionRepository.GetPromotionInfoByPromoCode(code, tour, DateTime.Now, _bookingCart.CurrentDepartureDate);

            if (promotion == null)
            {
                baseResponse.Message = "Invalid promo code.";
                return Ok(baseResponse);
            }

            _bookingCart.SetPromotion(promotion);
            _bookingCart.SetPromoCode(code);
            
            var response = new ApplyPromoCodeResponse
            {
                Success = true
            };
            return Ok(response);
        }

 
    }
}