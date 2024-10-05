using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public enum OrderStatus
    {
        [Display (Name = "Создан")]
        Created,
        [Display(Name = "Обработан")]
        Processed,
        [Display(Name = "В пути")]
        Delivering,
        [Display(Name = "Доставлен")]
        Delivered,
        [Display(Name = "Отменен")]
        Cancelled
    }
}