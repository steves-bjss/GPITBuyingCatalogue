﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Ordering;
using NHSD.GPIT.BuyingCatalogue.Framework.Logging;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders;

namespace NHSD.GPIT.BuyingCatalogue.Services.Orders
{
    public class OrderService : IOrderService
    {
        // private readonly ApplicationDbContext context;
        private readonly ILogWrapper<OrderService> logger;
        private readonly OrderingDbContext dbContext;
        private readonly IDbRepository<Order, OrderingDbContext> orderRepository;

        public OrderService(
            ILogWrapper<OrderService> logger,
            OrderingDbContext dbContext,
            IDbRepository<Order, OrderingDbContext> orderRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Order> GetOrder(CallOffId callOffId)
        {
            return await dbContext.Orders
                .Where(o => o.Id == callOffId.Id)
                .Include(o => o.OrderingParty).ThenInclude(p => p.Address)
                .Include(o => o.OrderingPartyContact)
                .Include(o => o.Supplier).ThenInclude(s => s.Address)
                .Include(o => o.SupplierContact)
                .Include(o => o.ServiceInstanceItems).Include(o => o.OrderItems).ThenInclude(i => i.CatalogueItem)
                .Include(o => o.OrderItems).ThenInclude(i => i.OrderItemRecipients).ThenInclude(r => r.Recipient)
                .Include(o => o.OrderItems).ThenInclude(i => i.PricingUnit)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<IList<Order>> GetOrders(Guid organisationId)
        {
            return await dbContext.OrderingParty
                .Where(o => o.Id == organisationId)
                .SelectMany(o => o.Orders)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order> GetOrderSummary(CallOffId callOffId)
        {
            return await dbContext.Orders
                .Where(o => o.Id == callOffId.Id)
                .Include(o => o.OrderingParty)
                .Include(o => o.OrderingPartyContact)
                .Include(o => o.SupplierContact)
                .Include(o => o.OrderItems).ThenInclude(i => i.CatalogueItem)
                .Include(o => o.OrderItems).ThenInclude(i => i.OrderItemRecipients)
                .Include(o => o.Progress)
                .AsQueryable()
                .SingleOrDefaultAsync();
        }

        public async Task<Order> GetOrderForStatusUpdate(CallOffId callOffId)
        {
            return await dbContext.Orders
                .Where(o => o.Id == callOffId.Id)
                .Include(o => o.OrderingParty)
                .Include(o => o.Supplier)
                .Include(o => o.OrderItems).ThenInclude(i => i.CatalogueItem)
                .Include(o => o.OrderItems).ThenInclude(i => i.OrderItemRecipients).ThenInclude(r => r.Recipient)
                .Include(o => o.OrderItems).ThenInclude(i => i.PricingUnit)
                .Include(o => o.Progress)
                .SingleOrDefaultAsync();
        }

        public async Task<Order> CreateOrder(string description, Guid organisationId)
        {
            OrderingParty orderingParty = (await GetOrderingParty(organisationId)) ?? new OrderingParty { Id = organisationId };

            var order = new Order
            {
                Description = description,
                OrderingParty = orderingParty,
            };

            dbContext.Add(order);
            await dbContext.SaveChangesAsync();

            return order;
        }

        public async Task DeleteOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            order.IsDeleted = true;

            await dbContext.SaveChangesAsync();
        }

        private async Task<OrderingParty> GetOrderingParty(Guid organisationId)
        {
            return await dbContext.OrderingParty.FindAsync(organisationId);
        }
    }
}
