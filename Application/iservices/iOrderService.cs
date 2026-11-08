using System;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;

namespace shopforge_backend.Application.iservices;

public interface iOrderService
{
     Task<response_model<orderresponsedto>> PlaceOrder(Guid userId, placeorderdto dto);
     Task<response_model<List<orderresponsedto>>> GetMyOrders(Guid userId);
     Task<response_model<orderresponsedto>> GetOrder(Guid userId, Guid orderId, bool isAdmin);
     Task<response_model<orderresponsedto>> UpdateStatus(Guid orderId, updateorderstatusdto dto);
     Task<string> GenerateOrderNumber();
}