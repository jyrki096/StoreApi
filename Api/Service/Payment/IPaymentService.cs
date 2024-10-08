using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Service.Payment
{
    public interface IPaymentService
    {
        Task<ActionResult<ServerResponse>> HandlePaymentAsync(string userId, int orderHeaderId, string cardNumber);
    }
}
