using Microsoft.AspNetCore.Mvc;
using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GreenHiTech.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly OrderDetailRepo _orderDetailRepo;
        private readonly ProductRepo _productRepo;
        private readonly OrderRepo _orderRepo;

        public OrderDetailController(OrderDetailRepo orderDetailRepo, ProductRepo productRepo, OrderRepo orderRepo)
        {
            _orderDetailRepo = orderDetailRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        public IActionResult Index()
        {
            var orderDetails = _orderDetailRepo.GetAll();

            var orderDetailVMs = orderDetails.Select(od => new OrderDetailVM
            {
                PkId = od.PkId,
                FkOrderId = od.FkOrderId,
                FkProductId = od.FkProductId,
                Quantity = od.Quantity,
                ProductName = od.FkProduct?.Name,
                //OrderDate = od.FkOrder?.OrderDate
            }).ToList();

            return View(orderDetailVMs);
        }

        public IActionResult Details(int id)
        {
            var orderDetail = _orderDetailRepo.GetById(id);

            if (orderDetail == null)
            {
                return RedirectToAction("Index", new { message = $"error, Order detail not found: (ID {id})" });
            }

            var orderDetailVM = new OrderDetailVM
            {
                PkId = orderDetail.PkId,
                FkOrderId = orderDetail.FkOrderId,
                FkProductId = orderDetail.FkProductId,
                Quantity = orderDetail.Quantity,
                ProductName = orderDetail.FkProduct?.Name,
                //OrderDate = orderDetail.FkOrder?.OrderDate
            };

            return View(orderDetailVM);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.Products = _productRepo.GetAll().Select(p => new SelectListItem
            {
                Value = p.PkId.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Orders = _orderRepo.GetAll().Select(o => new SelectListItem
            {
                Value = o.PkId.ToString(),
                Text = o.OrderDate.ToString()
            }).ToList();

            return View(new OrderDetailVM());
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(OrderDetailVM orderDetailVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var orderDetail = new OrderDetail
                    {
                        FkOrderId = orderDetailVM.FkOrderId,
                        FkProductId = orderDetailVM.FkProductId,
                        Quantity = orderDetailVM.Quantity
                    };

                    _orderDetailRepo.Add(orderDetail);
                    return RedirectToAction("Index", new { message = "success, Order detail created successfully." });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating order detail: {ex.Message}");
                }
            }

            return View(orderDetailVM);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var orderDetail = _orderDetailRepo.GetById(id);

            if (orderDetail == null)
            {
                return RedirectToAction("Index", new { message = $"error, Order detail not found: (ID {id})" });
            }

            var orderDetailVM = new OrderDetailVM
            {
                PkId = orderDetail.PkId,
                FkOrderId = orderDetail.FkOrderId,
                FkProductId = orderDetail.FkProductId,
                Quantity = orderDetail.Quantity
            };

            ViewBag.Products = _productRepo.GetAll().Select(p => new SelectListItem
            {
                Value = p.PkId.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Orders = _orderRepo.GetAll().Select(o => new SelectListItem
            {
                Value = o.PkId.ToString(),
                Text = o.OrderDate.ToString()
            }).ToList();

            return View(orderDetailVM);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(OrderDetailVM orderDetailVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingOrderDetail = _orderDetailRepo.GetById(orderDetailVM.PkId);

                    if (existingOrderDetail == null)
                    {
                        return RedirectToAction("Index", new { message = $"error, Order detail not found: (ID {orderDetailVM.PkId})" });
                    }

                    existingOrderDetail.FkOrderId = orderDetailVM.FkOrderId;
                    existingOrderDetail.FkProductId = orderDetailVM.FkProductId;
                    existingOrderDetail.Quantity = orderDetailVM.Quantity;

                    _orderDetailRepo.Update(existingOrderDetail);
                    return RedirectToAction("Index", new { message = "success, Order detail updated successfully." });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating order detail: {ex.Message}");
                }
            }

            return View(orderDetailVM);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var orderDetail = _orderDetailRepo.GetById(id);

            if (orderDetail == null)
            {
                return RedirectToAction("Index", new { message = $"error, Order detail not found: (ID {id})" });
            }

            var orderDetailVM = new OrderDetailVM
            {
                PkId = id,
                FkOrderId = orderDetail.FkOrderId,
                FkProductId = orderDetail.FkProductId,
                Quantity = orderDetail.Quantity,
                ProductName = orderDetail.FkProduct?.Name,
                //OrderDate = orderDetail.FkOrder?.OrderDate
            };

            return View(orderDetailVM);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _orderDetailRepo.Delete(id);
                return RedirectToAction("Index", new { message = "success, Order detail deleted successfully." });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { message = $"error, Error deleting order detail: {ex.Message}" });
            }
        }
    }
}
