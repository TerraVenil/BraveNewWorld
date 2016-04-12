using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BraveNewWorld.Dal;

namespace BraveNewWorld.Web.Controllers
{
    public class OrdersController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var connectionString = db.Database.Connection.ConnectionString;

            var queryString = @"SELECT 
    [Extent1].[OrderID] AS [OrderID], 
    [Extent1].[CustomerID] AS [CustomerID], 
    [Extent1].[EmployeeID] AS [EmployeeID], 
    [Extent1].[OrderDate] AS [OrderDate], 
    [Extent1].[RequiredDate] AS [RequiredDate], 
    [Extent1].[ShippedDate] AS [ShippedDate], 
    [Extent1].[ShipVia] AS [ShipVia], 
    [Extent1].[Freight] AS [Freight], 
    [Extent1].[ShipName] AS [ShipName], 
    [Extent1].[ShipAddress] AS [ShipAddress], 
    [Extent1].[ShipCity] AS [ShipCity], 
    [Extent1].[ShipRegion] AS [ShipRegion], 
    [Extent1].[ShipPostalCode] AS [ShipPostalCode], 
    [Extent1].[ShipCountry] AS [ShipCountry], 
    [Extent2].[CustomerID] AS [CustomerID1], 
    [Extent2].[CompanyName] AS [CompanyName], 
    [Extent2].[ContactName] AS [ContactName], 
    [Extent2].[ContactTitle] AS [ContactTitle], 
    [Extent2].[Address] AS [Address], 
    [Extent2].[City] AS [City], 
    [Extent2].[Region] AS [Region], 
    [Extent2].[PostalCode] AS [PostalCode], 
    [Extent2].[Country] AS [Country], 
    [Extent2].[Phone] AS [Phone], 
    [Extent2].[Fax] AS [Fax], 
    [Extent3].[EmployeeID] AS [EmployeeID1], 
    [Extent3].[LastName] AS [LastName], 
    [Extent3].[FirstName] AS [FirstName], 
    [Extent3].[Title] AS [Title], 
    [Extent3].[TitleOfCourtesy] AS [TitleOfCourtesy], 
    [Extent3].[BirthDate] AS [BirthDate], 
    [Extent3].[HireDate] AS [HireDate], 
    [Extent3].[Address] AS [Address1], 
    [Extent3].[City] AS [City1], 
    [Extent3].[Region] AS [Region1], 
    [Extent3].[PostalCode] AS [PostalCode1], 
    [Extent3].[Country] AS [Country1], 
    [Extent3].[HomePhone] AS [HomePhone], 
    [Extent3].[Extension] AS [Extension], 
    [Extent3].[Photo] AS [Photo], 
    [Extent3].[Notes] AS [Notes], 
    [Extent3].[ReportsTo] AS [ReportsTo], 
    [Extent3].[PhotoPath] AS [PhotoPath]
    FROM  [dbo].[Orders] AS [Extent1]
    LEFT OUTER JOIN [dbo].[Customers] AS [Extent2] ON [Extent1].[CustomerID] = [Extent2].[CustomerID]
    LEFT OUTER JOIN [dbo].[Employees] AS [Extent3] ON [Extent1].[EmployeeID] = [Extent3].[EmployeeID]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var orders = reader.Select(
                        x => new Order
                             {
                                 OrderID = int.Parse(x["OrderID"].ToString()),
                                 OrderDate = DateTime.Parse(x["OrderDate"].ToString()),
                                 ShipName = x["ShipName"].ToString(),
                                 Customer = new Customer { CompanyName = x["CompanyName"].ToString() }
                             }).ToList();

                    reader.Close();

                    return View(orders);
                }
            }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", order.EmployeeID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", order.EmployeeID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", order.EmployeeID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
