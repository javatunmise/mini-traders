using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using Site.Data;

namespace site.Data.Repositories
{
    public class PaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentDate _serverDate;

        public PaymentRepository(ApplicationDbContext context, ICurrentDate serverDate)
        {
            _dbContext = context;
            _serverDate = serverDate;
        }
        public async Task CreatePayment(PaymentReservationData data)
        {
            _dbContext.Payments.Add(new SitePayment
            {
                Amount = data.Amount,
                Email = data.Email,
                EntityId = data.EntityId,
                EntityType = data.EntityType,
                PaymentDescription = data.Purpose,
                CreatedOn = _serverDate.Now(),
                PaymentRef = data.PaymentRef,
                Status = PaymentStatuses.Initiated
            });

            await _dbContext.SaveChangesAsync();
        }

        internal Task Update(SitePayment payment)
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        internal async Task<SitePayment> GetPayment(string pRef)
        {
            return await _dbContext.Payments.FirstOrDefaultAsync(e => e.PaymentRef == pRef);
        }
    }
}
