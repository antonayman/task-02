using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LinqAssignment
{
    // This class plays the role of "ListGenerators.cs" from the assignment:
    // it hands back the in-memory sample data (Products, Customers/Orders,
    // dictionary words) that every LINQ query in Program.cs works on.
    //
    // Customers.xml and dictionary_english.txt are EXTERNAL files that were
    // mentioned in the assignment but not attached. To keep the project
    // runnable as-is, this class:
    //   1) Looks for the real file next to the .exe first.
    //   2) If it's not found, falls back to built-in sample data so every
    //      query below still compiles and runs with meaningful results.
    //
    // Sample copies of both files (dictionary_english.txt, Customers.xml)
    // are included alongside this project - drop your real ones in the
    // same folder (same names) to use your own data instead; no code
    // changes needed.
    public static class DataSource
    {
        public static List<Product> GetProductList()
        {
            return new List<Product>
            {
                new Product { ProductName = "Chai",                          Category = "Beverages",      UnitPrice = 18.00m, UnitsInStock = 39 },
                new Product { ProductName = "Chang",                         Category = "Beverages",      UnitPrice = 19.00m, UnitsInStock = 17 },
                new Product { ProductName = "Cote de Blaye",                 Category = "Beverages",      UnitPrice = 263.50m, UnitsInStock = 0  },
                new Product { ProductName = "Ipoh Coffee",                   Category = "Beverages",      UnitPrice = 46.00m, UnitsInStock = 17 },
                new Product { ProductName = "Aniseed Syrup",                 Category = "Condiments",     UnitPrice = 10.00m, UnitsInStock = 13 },
                new Product { ProductName = "Chef Anton's Cajun Seasoning",  Category = "Condiments",     UnitPrice = 22.00m, UnitsInStock = 53 },
                new Product { ProductName = "Chef Anton's Gumbo Mix",        Category = "Condiments",     UnitPrice = 21.35m, UnitsInStock = 0  },
                new Product { ProductName = "Grandma's Boysenberry Spread",  Category = "Condiments",     UnitPrice = 25.00m, UnitsInStock = 120 },
                new Product { ProductName = "Northwoods Cranberry Sauce",    Category = "Condiments",     UnitPrice = 40.00m, UnitsInStock = 6  },
                new Product { ProductName = "Genen Shouyu",                  Category = "Condiments",     UnitPrice = 15.50m, UnitsInStock = 39 },
                new Product { ProductName = "Uncle Bob's Organic Dried Pears", Category = "Produce",      UnitPrice = 30.00m, UnitsInStock = 15 },
                new Product { ProductName = "Tofu",                         Category = "Produce",         UnitPrice = 23.25m, UnitsInStock = 35 },
                new Product { ProductName = "Longlife Tofu",                 Category = "Produce",         UnitPrice = 10.00m, UnitsInStock = 4  },
                new Product { ProductName = "Mishi Kobe Niku",               Category = "Meat/Poultry",    UnitPrice = 97.00m, UnitsInStock = 29 },
                new Product { ProductName = "Alice Mutton",                  Category = "Meat/Poultry",    UnitPrice = 39.00m, UnitsInStock = 0  },
                new Product { ProductName = "Perth Pasties",                 Category = "Meat/Poultry",    UnitPrice = 32.80m, UnitsInStock = 0  },
                new Product { ProductName = "Ikura",                         Category = "Seafood",         UnitPrice = 31.00m, UnitsInStock = 31 },
                new Product { ProductName = "Konbu",                        Category = "Seafood",         UnitPrice = 6.00m,  UnitsInStock = 24 },
                new Product { ProductName = "Carnarvon Tigers",              Category = "Seafood",         UnitPrice = 62.50m, UnitsInStock = 42 },
                new Product { ProductName = "Nord-Ost Matjeshering",         Category = "Seafood",         UnitPrice = 25.89m, UnitsInStock = 10 },
                new Product { ProductName = "Queso Cabrales",                Category = "Dairy Products",  UnitPrice = 21.00m, UnitsInStock = 22 },
                new Product { ProductName = "Queso Manchego La Pastora",     Category = "Dairy Products",  UnitPrice = 38.00m, UnitsInStock = 86 },
                new Product { ProductName = "Pavlova",                       Category = "Confections",     UnitPrice = 17.45m, UnitsInStock = 29 },
                new Product { ProductName = "Teatime Chocolate Biscuits",    Category = "Confections",     UnitPrice = 9.20m,  UnitsInStock = 25 },
                new Product { ProductName = "Sir Rodney's Marmalade",        Category = "Confections",     UnitPrice = 81.00m, UnitsInStock = 40 },
            };
        }

        public static List<Customer> GetCustomerList()
        {
            const string xmlFile = "Customers.xml";

            if (File.Exists(xmlFile))
            {
                try
                {
                    return LoadCustomersFromXml(xmlFile);
                }
                catch
                {
                    // Malformed file -> fall through to built-in sample data
                }
            }

            Console.WriteLine("[Note] Customers.xml not found next to the program - using built-in sample customer data instead.");
            return GetFallbackCustomers();
        }

        private static List<Customer> LoadCustomersFromXml(string path)
        {
            XDocument doc = XDocument.Load(path);
            List<Customer> customers = new List<Customer>();

            foreach (XElement custElement in doc.Root.Elements("Customer"))
            {
                Customer customer = new Customer
                {
                    CustomerID = (string)custElement.Attribute("CustomerID"),
                    City = (string)custElement.Attribute("City"),
                    Region = (string)custElement.Attribute("Region")
                };

                XElement ordersElement = custElement.Element("Orders");
                if (ordersElement != null)
                {
                    foreach (XElement orderElement in ordersElement.Elements("Order"))
                    {
                        customer.Orders.Add(new Order
                        {
                            OrderID = (int)orderElement.Attribute("OrderID"),
                            OrderDate = (DateTime)orderElement.Attribute("OrderDate"),
                            Total = (decimal)orderElement.Attribute("Total")
                        });
                    }
                }

                customers.Add(customer);
            }

            return customers;
        }

        private static List<Customer> GetFallbackCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    CustomerID = "LAZYK", City = "Seattle", Region = "WA",
                    Orders = new List<Order>
                    {
                        new Order { OrderID = 1001, OrderDate = new DateTime(1997, 3, 21), Total = 450.00m },
                        new Order { OrderID = 1002, OrderDate = new DateTime(1998, 5, 6),  Total = 120.50m },
                        new Order { OrderID = 1003, OrderDate = new DateTime(1998, 9, 12), Total = 980.00m },
                    }
                },
                new Customer
                {
                    CustomerID = "TRAIH", City = "Kirkland", Region = "WA",
                    Orders = new List<Order>
                    {
                        new Order { OrderID = 1004, OrderDate = new DateTime(1997, 11, 2), Total = 610.75m },
                        new Order { OrderID = 1005, OrderDate = new DateTime(1998, 1, 15), Total = 75.00m },
                    }
                },
                new Customer
                {
                    CustomerID = "WHITC", City = "Walla Walla", Region = "WA",
                    Orders = new List<Order>
                    {
                        new Order { OrderID = 1006, OrderDate = new DateTime(1997, 6, 30), Total = 220.00m },
                        new Order { OrderID = 1007, OrderDate = new DateTime(1998, 3, 4),  Total = 530.20m },
                        new Order { OrderID = 1008, OrderDate = new DateTime(1998, 7, 19), Total = 40.00m },
                        new Order { OrderID = 1009, OrderDate = new DateTime(1999, 2, 9),  Total = 300.00m },
                    }
                },
                new Customer
                {
                    CustomerID = "ALFKI", City = "Berlin", Region = null,
                    Orders = new List<Order>
                    {
                        new Order { OrderID = 1010, OrderDate = new DateTime(1997, 8, 25), Total = 95.00m },
                        new Order { OrderID = 1011, OrderDate = new DateTime(1998, 4, 1),  Total = 700.00m },
                    }
                },
                new Customer
                {
                    CustomerID = "ANATR", City = "Mexico D.F.", Region = null,
                    Orders = new List<Order>
                    {
                        new Order { OrderID = 1012, OrderDate = new DateTime(1998, 11, 11), Total = 15.00m },
                    }
                },
            };
        }

        public static string[] GetWordList()
        {
            const string dictFile = "dictionary_english.txt";

            if (File.Exists(dictFile))
            {
                string[] words = File.ReadAllLines(dictFile)
                                      .Select(w => w.Trim())
                                      .Where(w => w.Length > 0)
                                      .ToArray();
                if (words.Length > 0)
                {
                    return words;
                }
            }

            Console.WriteLine("[Note] dictionary_english.txt not found next to the program - using a small built-in sample word list instead.");
            return GetFallbackWordList();
        }

        private static string[] GetFallbackWordList()
        {
            return new string[]
            {
                "apple", "banana", "receive", "believe", "friend", "weird",
                "height", "neighbor", "achieve", "science", "either",
                "cat", "dog", "elephant", "giraffe", "ox", "a",
                "beautiful", "extraordinary", "quick", "jump", "run",
                "ceiling", "protein", "leisure", "seize", "efficient",
                "table", "chair", "window", "programming", "computer"
            };
        }
    }
}
