﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL;
using WEB_APP.Models;
namespace WEB_APP.Controllers
{
    public class PanierController : Controller
    {
        private ICustomersManager customersManager { get; set; }
        private IOrdersManager ordersManager { get; set; }
        private IOrder_DishManager order_DishManager { get; set; }
        private IDishesManager dishesManager { get; set; }
        private IStaffsManager staffsManager { get; set; }
        public PanierController(ICustomersManager customersManager, 
            IOrdersManager ordersManager,
            IDishesManager dishesManager,
            IOrder_DishManager order_DishManager, IStaffsManager staffsManager)
        {
            this.staffsManager = staffsManager;
            this.customersManager = customersManager;
            this.ordersManager = ordersManager;
            this.order_DishManager = order_DishManager;
            this.dishesManager = dishesManager;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Panier> paniers = getPaniers(); 
            return View(paniers);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var od = order_DishManager.GetByID(id);
            var dish = dishesManager.GetByID(od.IdDish);
            var order = ordersManager.GetByID(od.IdOrder);


            order.NbrDish -= od.Quantity;
            order.TotalPrice -= (od.Quantity* dish.Price);
            HttpContext.Session.SetInt32("NbrDish", order.NbrDish);

            ordersManager.Update(order);

            order_DishManager.Delete(od.IdOrder_Dish);
            return RedirectToAction("Index");
        }

        private List<Panier> getPaniers()
        {
            var user = getLoggedUser();
            var orders = ordersManager.GetAllByUser(user.IdCustomer);
            List<Panier> paniers = new List<Panier>();

            foreach (var order in orders)
            {
                List<OrderDish> l_orderDish = new List<OrderDish>();
                foreach (var od in order_DishManager.GetByOrder(order.IdOrder))
                {
                    l_orderDish.Add(new OrderDish { Dish = dishesManager.GetByID(od.IdDish), Order_dish = od });
                }
                paniers.Add(new Panier { Order = order, OrderDishes = l_orderDish, Staff=staffsManager.GetByID(order.IdStaff) });
                // ordersDishes.Add(new OrderDish { Order = order, Dishes = dishes, OrderDishes = ods });
            }
            return paniers;
        }
        public IActionResult DeleteCommande(int id)
        {
            ViewBag.IdOrderToDelete = id;
            return View();
        }

        public IActionResult ConfirmDelete(string lastname, string firstname, int idCommande)
        {
            var idCust = ordersManager.GetByID(idCommande).IdCustomer;
            var customer = customersManager.GetByID(idCust);

            if (customer.Firstname == firstname && customer.Lastname == lastname)
            {
                ordersManager.Delete(idCommande);
                return RedirectToAction("Index");
            } else
            {
                TempData["ErrorDeleteCommande"] = "Le nom ou le prénom est faux! Veuillez retenter votre chance.";
                return RedirectToAction("DeleteCommande", new { id = idCommande });
            }

        }
        public IActionResult Confirm(int idOrder, int time)
        {

            if (HttpContext.Session.GetInt32("IdCustomer") == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetInt32("NbrDish", 0);

            var user = getLoggedUser();
            var order = ordersManager.GetByID(idOrder);
            var timetoadd = time;
            if (order.TimeToPrepare > time)
                timetoadd = order.TimeToPrepare;


            order.DatetimeConfirmed = DateTime.Now.AddMinutes((double)timetoadd);
            ordersManager.Update(order);
            
            var staffs = staffsManager.GetByCity(user.IdCity);
            foreach (var s in staffs)
            {
                if (!ordersManager.StaffHasMoreThenFive(s.IdStaff, (DateTime)order.DatetimeConfirmed))
                {
                    order.Status = DTO.Order.TO_DELIVERY;
                    order.IdStaff = s.IdStaff;
                    ordersManager.Update(order);
                    return RedirectToAction("Index");
                }
            }
            order.Status = DTO.Order.UNABLE_TO_DELIVER;
            ordersManager.Update(order);
            // ordersManager.Update(order);

            return RedirectToAction("Index");
        }
        private DTO.Customer getLoggedUser()
        {
            var IdCustomer = (int)HttpContext.Session.GetInt32("IdCustomer");
            return customersManager.GetByID(IdCustomer);
        }
    }
}