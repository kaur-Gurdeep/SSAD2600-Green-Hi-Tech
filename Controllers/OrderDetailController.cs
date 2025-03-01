using GreenHiTech.Models;
using GreenHiTech.Repositories;
using GreenHiTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GreenHiTech.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly OrderDetailRepo _orderDetailRepo;
        private readonly OrderRepo _orderRepo;
        private readonly ProductRepo _productRepo;
        private readonly ProductImageRepo _productImageRepo;

        public OrderDetailController(
            OrderDetailRepo orderDetailRepo,
            OrderRepo orderRepo,
            ProductRepo productRepo,
            ProductImageRepo productImageRepo)
        {
            _orderDetailRepo = orderDetailRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
        }

        public IActionResult Index(int orderId)
        {
            List<OrderDetail> orderDetails = _orderDetailRepo.GetByOrderId(orderId);
            List<OrderDetailVM> orderDetailVMs = new List<OrderDetailVM>();
            bool firstPass = true;

            foreach(OrderDetail orderDetail in orderDetails)
            {
                if(firstPass)
                {
                    ViewBag.OrderDate = orderDetail.FkOrder.OrderDate;
                    ViewBag.OrderTotal = orderDetail.FkOrder.TotalAmount;
                    firstPass = false;
                }
                ProductImage? productImage = _productImageRepo.GetFirstForProductId(orderDetail.FkProductId);
                OrderDetailVM orderDetailVM = new OrderDetailVM
                {
                    ProductImage = productImage,
                    ProductName = orderDetail.FkProduct.Name,
                    Price = orderDetail.FkProduct.Price,
                    Quantity = orderDetail.Quantity,
                };
                orderDetailVMs.Add(orderDetailVM);
            }

            ViewBag.OrderId = orderId;

            return View(orderDetailVMs);
        }

        public IActionResult Details(int id)
        {
            var orderDetail = _orderDetailRepo.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderDetailVM = MapToViewModel(orderDetail);
            return View(orderDetailVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderDetailVM orderDetailVM)
        {
            if (ModelState.IsValid)
            {
                var orderDetail = MapToModel(orderDetailVM);
                string result = _orderDetailRepo.Add(orderDetail);
                if (result.StartsWith("success"))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result);
            }
            return View(orderDetailVM);
        }

        public IActionResult Edit(int id)
        {
            var orderDetail = _orderDetailRepo.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderDetailVM = MapToViewModel(orderDetail);
            return View(orderDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OrderDetailVM orderDetailVM)
        {
            if (id != orderDetailVM.PkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var orderDetail = MapToModel(orderDetailVM);
                string result = _orderDetailRepo.Update(orderDetail);
                if (result.StartsWith("success"))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result);
            }
            return View(orderDetailVM);
        }

        public IActionResult Delete(int id)
        {
            var orderDetail = _orderDetailRepo.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderDetailVM = MapToViewModel(orderDetail);
            return View(orderDetailVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            string result = _orderDetailRepo.Delete(id);
            if (result.StartsWith("success"))
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result);
            return View();
        }

        private List<OrderDetailVM> MapToViewModels(List<OrderDetail> orderDetails)
        {
            //Order? order = this._orderRepo.GetById(orderDetails.)
            

            return orderDetails.Select(od => new OrderDetailVM
            {
                PkId = od.PkId,
                FkOrderId = od.FkOrderId,
                FkProductId = od.FkProductId,
                Quantity = od.Quantity,
                ProductName = od.FkProduct?.Name,
                //OrderDate = od.FkOrder?.OrderDate.ToDateTime(TimeOnly.MinValue)
            }).ToList();
        }

        private OrderDetailVM MapToViewModel(OrderDetail orderDetail)
        {
            return new OrderDetailVM
            {
                PkId = orderDetail.PkId,
                FkOrderId = orderDetail.FkOrderId,
                FkProductId = orderDetail.FkProductId,
                Quantity = orderDetail.Quantity,
                ProductName = orderDetail.FkProduct?.Name,
                //OrderDate = orderDetail.FkOrder?.OrderDate.ToDateTime(TimeOnly.MinValue)
            };
        }

        private OrderDetail MapToModel(OrderDetailVM orderDetailVM)
        {
            return new OrderDetail
            {
                PkId = orderDetailVM.PkId,
                FkOrderId = orderDetailVM.FkOrderId,
                FkProductId = orderDetailVM.FkProductId,
                Quantity = orderDetailVM.Quantity
            };
        }
    }
}
