using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.Infrastructure.config;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.services;

public class OrderService(appdbcontext _context, iCouponService couponService) : iOrderService
{
     
}