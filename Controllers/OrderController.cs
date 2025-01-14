using Microsoft.AspNetCore.Mvc;

namespace GreenHiTech.Controllers
{
    public class OrderController: Controller
    {
        private readonly OrderRepo _orderRepo;
        private readonly OrderController _logger;

        public OrderController(ILogger<OrderController> logger, OrderRepo orderRepo)
        {
            _logger = logger;
            _orderRepo = orderRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(payPalConfirmationModel payPalConfirmation) 
        {

        }
    }
}
