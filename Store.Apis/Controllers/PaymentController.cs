using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Core.Services.Contract;
using Stripe;

namespace Store.Apis.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var basket = await paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(basket);
        }


        //whsec_fa1db5fe3ae18a9fbc0a2b02f1298006b780d01c5a535b4d007d0695461d283b

        const string endpointSecret = "whsec_fa1db5fe3ae18a9fbc0a2b02f1298006b780d01c5a535b4d007d0695461d283b";
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                //pi_3QC4mFD8KLBVgb8k1fKyziSm
                //pi_3QC4uhD8KLBVgb8k0IQKvz50
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    //UpdateDB
                    await paymentService.UpdatePaymentStatus(paymentIntent.Id, false);

                }
                else if (stripeEvent.Type == "payment_intent.payment_succeeded")
                {
                    //UpdateDB
                    await paymentService.UpdatePaymentStatus(paymentIntent.Id, true);
                }

                return Ok();


            }
            catch (Exception ex)
            {
                {
                    return BadRequest(ex);
                }
            }
        }

    }
}
