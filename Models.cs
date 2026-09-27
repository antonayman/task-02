using System;
using System.Collections.Generic;

namespace LinqAssignment
{
    public class Product
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }

    public class Customer
    {
        public string CustomerID { get; set; }
        public string City { get; set; }
        public string Region { get; set; } // "WA" for Washington customers, null/other otherwise
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
