using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using site.Data;
using Shared.Entities;
using Shared;
using site.Repositories;
using Site.RequestDtos;
using Microsoft.Extensions.Logging;

namespace site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class OperationsController : ControllerBase
    {
        private readonly ISiteContentProvider _provider;
        private readonly AccountRepository _accountRepo;
        private readonly ProductsRepository _productRepo;
        private readonly ChatRepository _chatRepository;
        private readonly ICurrentDate _currentDate;
        private readonly StoreRepository _storeRepo;
        private readonly ILogger<OperationsController> _logger;

        public OperationsController(ISiteContentProvider provider, 
                                    AccountRepository accountRepository,
                                    ProductsRepository productRepository,
                                    ChatRepository chatRepository,
                                    StoreRepository storeRepository,
                                    ICurrentDate currentDate,
                                    ILogger<OperationsController> logger)
        {
            _provider = provider;
            _accountRepo = accountRepository;
            _productRepo = productRepository;
            _chatRepository = chatRepository;
            _currentDate = currentDate;
            _storeRepo = storeRepository;
            _logger = logger;
        }

        [HttpPost("vendormessage")]
        public async Task<IActionResult> SubmitVendorMessage(VendorMessageRequest request)
        {
            var currentUser = await _accountRepo.FindSiteUser(User.Identity.Name);
            if (currentUser == null)
                return Unauthorized();

            var message = new MessageContext();

            if(request.ChatId > 0)
            {
                var chat = await _chatRepository.GetChat(request.ChatId);
                if (chat.InitiatorUserId != currentUser.Id && chat.RecipientUserId != currentUser.Id)
                    return Forbid();

                message = new MessageContext
                {
                    UserId = chat.InitiatorUserId,
                    RecipientUserId = chat.RecipientUserId,
                    Message = request.Message,
                    MessageUserId = currentUser.Id
                };

            }
            else
            {
                var store = await _storeRepo.GetStoreById(request.StoreId);
                if (store == null) return BadRequest();

                message = new MessageContext
                {
                    UserId = currentUser.Id,
                    RecipientUserId = store.SiteUserId,
                    Message = request.Message,
                    MessageUserId = currentUser.Id
                };
            }

            try
            {
                await _chatRepository.CreateChat(message);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, ex.Message);
                return BadRequest();
            }

        }

        [HttpPost("productreviews")]
        public async Task<IActionResult> SubmitProductReviews(ProductReviewRequest request)
        {
            var currentUser = await _accountRepo.FindSiteUser(User.Identity.Name);

            if (currentUser == null)
                return Unauthorized();

            var review = new ProductReview {
                Message = request.Message,
                Rating = Math.Min(request.Rating, 5),
                CreatedOn = _currentDate.Now(),
                ProductId = request.ProductId,
                ReviewerId = currentUser.Id,
                ReviewerName = $"{currentUser.FirstName} {currentUser.LastName}",
                StoreId = request.StoreId
            };

            await _productRepo.AddReview(review);

            return Ok();
        }
    }
}

namespace Site.RequestDtos
{
    public class ProductReviewRequest
    {
        public double Rating { get; set; }
        public string Message { get; set; }
        public bool HideMyIdentity { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
    }

    public class VendorMessageRequest
    {
        public string Message { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int ChatId { get; set; }
    }
}
