using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqAssignment
{
    // Custom comparer used in Ordering - Problem 2
    public class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }

    class Program
    {
        static List<Product> products = DataSource.GetProductList();
        static List<Customer> customers = DataSource.GetCustomerList();
        static string[] words = DataSource.GetWordList();

        static void Main(string[] args)
        {
            Restriction_Operators();
            Element_Operators();
            Aggregate_Operators();
            Ordering_Operators();
            Transformation_Operators();
            Partitioning_Operators();
            Quantifiers();

            Console.WriteLine();
            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadKey();
        }

        // ===================================================================
        // LINQ - Restriction Operators
        // ===================================================================
        static void Restriction_Operators()
        {
            Console.WriteLine("############ Restriction Operators ############");

            // 1. All products that are out of stock.
            Console.WriteLine("-- 1. Out of stock products --");
            var outOfStock = from p in products
                              where p.UnitsInStock == 0
                              select p;
            foreach (var p in outOfStock)
                Console.WriteLine(p.ProductName);

            // 2. In stock and cost more than 3.00 per unit.
            Console.WriteLine("-- 2. In stock & price > 3.00 --");
            var inStockExpensive = from p in products
                                    where p.UnitsInStock > 0 && p.UnitPrice > 3.00m
                                    select p;
            foreach (var p in inStockExpensive)
                Console.WriteLine(p.ProductName + " - " + p.UnitPrice);

            // 3. Digits whose name is shorter than their value.
            Console.WriteLine("-- 3. Digits shorter than their value --");
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var shortDigits = digits.Where((digit, index) => digit.Length < index);
            foreach (var d in shortDigits)
                Console.WriteLine(d);

            Console.WriteLine();
        }

        // ===================================================================
        // LINQ - Element Operators
        // ===================================================================
        static void Element_Operators()
        {
            Console.WriteLine("############ Element Operators ############");

            // 1. First product out of stock.
            Console.WriteLine("-- 1. First product out of stock --");
            var firstOut = products.First(p => p.UnitsInStock == 0);
            Console.WriteLine(firstOut.ProductName);

            // 2. First product whose Price > 1000, else null.
            Console.WriteLine("-- 2. First product with price > 1000 (or null) --");
            var first1000 = products.FirstOrDefault(p => p.UnitPrice > 1000);
            Console.WriteLine(first1000 == null ? "null (no product matches)" : first1000.ProductName);

            // 3. Retrieve the second number greater than 5.
            Console.WriteLine("-- 3. Second number greater than 5 --");
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var secondGreaterThan5 = arr.Where(n => n > 5).Skip(1).First();
            Console.WriteLine(secondGreaterThan5);

            Console.WriteLine();
        }

        // ===================================================================
        // LINQ - Aggregate Operators
        // ===================================================================
        static void Aggregate_Operators()
        {
            Console.WriteLine("############ Aggregate Operators ############");
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            // 1. Count of odd numbers.
            Console.WriteLine("-- 1. Count of odd numbers --");
            int oddCount = arr.Count(n => n % 2 != 0);
            Console.WriteLine(oddCount);

            // 2. Customers and how many orders each has.
            Console.WriteLine("-- 2. Orders count per customer --");
            var customerOrderCounts = from c in customers
                                       select new { c.CustomerID, OrderCount = c.Orders.Count };
            foreach (var c in customerOrderCounts)
                Console.WriteLine(c.CustomerID + " -> " + c.OrderCount + " orders");

            // 3. Categories and how many products each.
            Console.WriteLine("-- 3. Product count per category --");
            var productCountsByCategory = from p in products
                                           group p by p.Category into g
                                           select new { Category = g.Key, ProductCount = g.Count() };
            foreach (var c in productCountsByCategory)
                Console.WriteLine(c.Category + " -> " + c.ProductCount);

            // 4. Total of the numbers in the array.
            Console.WriteLine("-- 4. Total of numbers --");
            Console.WriteLine(arr.Sum());

            // 5. Total number of characters of all words in the dictionary.
            Console.WriteLine("-- 5. Total characters in dictionary --");
            Console.WriteLine(words.Sum(w => w.Length));

            // 6. Total units in stock per category.
            Console.WriteLine("-- 6. Total units in stock per category --");
            var unitsPerCategory = from p in products
                                    group p by p.Category into g
                                    select new { Category = g.Key, TotalUnits = g.Sum(p => p.UnitsInStock) };
            foreach (var c in unitsPerCategory)
                Console.WriteLine(c.Category + " -> " + c.TotalUnits);

            // 7. Length of the shortest word in the dictionary.
            Console.WriteLine("-- 7. Shortest word length --");
            Console.WriteLine(words.Min(w => w.Length));

            // 8. Cheapest price among each category's products.
            Console.WriteLine("-- 8. Cheapest price per category --");
            var cheapestPerCategory = from p in products
                                       group p by p.Category into g
                                       select new { Category = g.Key, Cheapest = g.Min(p => p.UnitPrice) };
            foreach (var c in cheapestPerCategory)
                Console.WriteLine(c.Category + " -> " + c.Cheapest);

            // 9. Products with the cheapest price in each category (use let).
            Console.WriteLine("-- 9. Cheapest product(s) per category --");
            var cheapestProductsPerCategory = from p in products
                                               group p by p.Category into g
                                               let minPrice = g.Min(p => p.UnitPrice)
                                               select new { Category = g.Key, CheapestProducts = g.Where(p => p.UnitPrice == minPrice) };
            foreach (var c in cheapestProductsPerCategory)
            {
                foreach (var p in c.CheapestProducts)
                    Console.WriteLine(c.Category + " -> " + p.ProductName + " (" + p.UnitPrice + ")");
            }

            // 10. Length of the longest word.
            Console.WriteLine("-- 10. Longest word length --");
            Console.WriteLine(words.Max(w => w.Length));

            // 11. Most expensive price among each category's products.
            Console.WriteLine("-- 11. Most expensive price per category --");
            var mostExpensivePerCategory = from p in products
                                            group p by p.Category into g
                                            select new { Category = g.Key, MostExpensive = g.Max(p => p.UnitPrice) };
            foreach (var c in mostExpensivePerCategory)
                Console.WriteLine(c.Category + " -> " + c.MostExpensive);

            // 12. Products with the most expensive price in each category.
            Console.WriteLine("-- 12. Most expensive product(s) per category --");
            var mostExpensiveProductsPerCategory = from p in products
                                                    group p by p.Category into g
                                                    let maxPrice = g.Max(p => p.UnitPrice)
                                                    select new { Category = g.Key, TopProducts = g.Where(p => p.UnitPrice == maxPrice) };
            foreach (var c in mostExpensiveProductsPerCategory)
            {
                foreach (var p in c.TopProducts)
                    Console.WriteLine(c.Category + " -> " + p.ProductName + " (" + p.UnitPrice + ")");
            }

            // 13. Average length of the words in the dictionary.
            Console.WriteLine("-- 13. Average word length --");
            Console.WriteLine(words.Average(w => w.Length));

            // 14. Average price of each category's products.
            Console.WriteLine("-- 14. Average price per category --");
            var avgPricePerCategory = from p in products
                                       group p by p.Category into g
                                       select new { Category = g.Key, AveragePrice = g.Average(p => p.UnitPrice) };
            foreach (var c in avgPricePerCategory)
                Console.WriteLine(c.Category + " -> " + c.AveragePrice);

            Console.WriteLine();
        }

        // ===================================================================
        // LINQ - Ordering Operators
        // ===================================================================
        static void Ordering_Operators()
        {
            Console.WriteLine("############ Ordering Operators ############");
            string[] wordsMixedCase = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            // 1. Sort products by name.
            Console.WriteLine("-- 1. Products sorted by name --");
            var byName = products.OrderBy(p => p.ProductName);
            foreach (var p in byName) Console.WriteLine(p.ProductName);

            // 2. Custom comparer, case-insensitive sort of words.
            Console.WriteLine("-- 2. Case-insensitive sort (custom comparer) --");
            var sortedCustom = wordsMixedCase.OrderBy(w => w, new CaseInsensitiveComparer());
            foreach (var w in sortedCustom) Console.WriteLine(w);

            // 3. Sort products by units in stock, highest to lowest.
            Console.WriteLine("-- 3. Products by units in stock (desc) --");
            var byStockDesc = products.OrderByDescending(p => p.UnitsInStock);
            foreach (var p in byStockDesc) Console.WriteLine(p.ProductName + " - " + p.UnitsInStock);

            // 4. Sort digits by name length, then alphabetically.
            Console.WriteLine("-- 4. Digits by length then alphabetically --");
            var digitsSorted = digits.OrderBy(d => d.Length).ThenBy(d => d);
            foreach (var d in digitsSorted) Console.WriteLine(d);

            // 5. Sort words first by length, then case-insensitive alphabetically.
            Console.WriteLine("-- 5. Words by length then case-insensitive --");
            var byLengthThenAlpha = wordsMixedCase.OrderBy(w => w.Length).ThenBy(w => w, StringComparer.OrdinalIgnoreCase);
            foreach (var w in byLengthThenAlpha) Console.WriteLine(w);

            // 6. Sort products by category, then unit price descending.
            Console.WriteLine("-- 6. Products by category, then price desc --");
            var byCategoryThenPrice = products.OrderBy(p => p.Category).ThenByDescending(p => p.UnitPrice);
            foreach (var p in byCategoryThenPrice) Console.WriteLine(p.Category + " | " + p.ProductName + " - " + p.UnitPrice);

            // 7. Sort by length, then case-insensitive DESCENDING.
            Console.WriteLine("-- 7. Words by length then case-insensitive desc --");
            var byLengthThenAlphaDesc = wordsMixedCase.OrderBy(w => w.Length).ThenByDescending(w => w, StringComparer.OrdinalIgnoreCase);
            foreach (var w in byLengthThenAlphaDesc) Console.WriteLine(w);

            // 8. Digits whose second letter is 'i', reversed from original order.
            Console.WriteLine("-- 8. Digits with 2nd letter 'i', reversed --");
            var secondLetterI = digits.Where(d => d.Length > 1 && d[1] == 'i').Reverse();
            foreach (var d in secondLetterI) Console.WriteLine(d);

            Console.WriteLine();
        }

        // ===================================================================
        // LINQ - Transformation Operators
        // ===================================================================
        static void Transformation_Operators()
        {
            Console.WriteLine("############ Transformation Operators ############");
            string[] wordsMixedCase = { "aPPLE", "BlUeBeRrY", "cHeRry" };
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            // 1. Sequence of just the names of a list of products.
            Console.WriteLine("-- 1. Product names only --");
            var justNames = products.Select(p => p.ProductName);
            foreach (var n in justNames) Console.WriteLine(n);

            // 2. Uppercase/lowercase version of each word (anonymous types).
            Console.WriteLine("-- 2. Upper/lower versions of words --");
            var upperLower = wordsMixedCase.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() });
            foreach (var w in upperLower) Console.WriteLine(w.Upper + " / " + w.Lower);

            // 3. Some properties of Products, UnitPrice renamed to Price.
            Console.WriteLine("-- 3. Product name/category/price (renamed) --");
            var renamed = products.Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice });
            foreach (var p in renamed) Console.WriteLine(p.ProductName + " | " + p.Category + " | " + p.Price);

            // 4. Determine if the value of ints matches their position in the array.
            Console.WriteLine("-- 4. Number in-place check --");
            var inPlaceCheck = numbers.Select((n, index) => new { Number = n, InPlace = (n == index) });
            foreach (var item in inPlaceCheck)
                Console.WriteLine(item.Number + ": " + item.InPlace);

            // 5. All pairs where number from numbersA < number from numbersB.
            Console.WriteLine("-- 5. Pairs where a < b --");
            var pairs = from a in numbersA
                        from b in numbersB
                        where a < b
                        select new { a, b };
            foreach (var pair in pairs)
                Console.WriteLine(pair.a + " is less than " + pair.b);

            // 6. Orders where the order total is less than 500.00.
            Console.WriteLine("-- 6. Orders with total < 500 --");
            var cheapOrders = from c in customers
                              from o in c.Orders
                              where o.Total < 500.00m
                              select new { c.CustomerID, o.OrderID, o.Total };
            foreach (var o in cheapOrders)
                Console.WriteLine(o.CustomerID + " - Order " + o.OrderID + " - " + o.Total);

            // 7. Orders made in 1998 or later.
            Console.WriteLine("-- 7. Orders from 1998 or later --");
            var recentOrders = from c in customers
                               from o in c.Orders
                               where o.OrderDate.Year >= 1998
                               select new { c.CustomerID, o.OrderID, o.OrderDate };
            foreach (var o in recentOrders)
                Console.WriteLine(o.CustomerID + " - Order " + o.OrderID + " - " + o.OrderDate.ToShortDateString());

            Console.WriteLine();
        }

        // ===================================================================
        // LINQ - Partitioning Operators
        // ===================================================================
        static void Partitioning_Operators()
        {
            Console.WriteLine("############ Partitioning Operators ############");
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var waOrders = customers.Where(c => c.Region == "WA").SelectMany(c => c.Orders);

            // 1. First 3 orders from customers in Washington.
            Console.WriteLine("-- 1. First 3 orders from WA customers --");
            var first3 = waOrders.Take(3);
            foreach (var o in first3) Console.WriteLine("Order " + o.OrderID + " - " + o.Total);

            // 2. All but the first 2 orders from customers in Washington.
            Console.WriteLine("-- 2. WA orders, skipping first 2 --");
            var skip2 = waOrders.Skip(2);
            foreach (var o in skip2) Console.WriteLine("Order " + o.OrderID + " - " + o.Total);

            // 3. Elements from the start until a number less than its position is hit.
            Console.WriteLine("-- 3. TakeWhile value >= position --");
            var takeWhileResult = numbers.TakeWhile((n, index) => n >= index);
            foreach (var n in takeWhileResult) Console.WriteLine(n);

            // 4. Elements starting from the first element divisible by 3.
            Console.WriteLine("-- 4. SkipWhile not divisible by 3 --");
            var skipWhileDivisible = numbers.SkipWhile(n => n % 3 != 0);
            foreach (var n in skipWhileDivisible) Console.WriteLine(n);

            // 5. Elements starting from the first element less than its position.
            Console.WriteLine("-- 5. SkipWhile value >= position --");
            var skipWhilePosition = numbers.SkipWhile((n, index) => n >= index);
            foreach (var n in skipWhilePosition) Console.WriteLine(n);

            Console.WriteLine();
        }

        // ===================================================================
        // LINQ - Quantifiers
        // ===================================================================
        static void Quantifiers()
        {
            Console.WriteLine("############ Quantifiers ############");

            // 1. Any word containing "ei".
            Console.WriteLine("-- 1. Any word contains 'ei'? --");
            bool anyContainsEi = words.Any(w => w.Contains("ei"));
            Console.WriteLine(anyContainsEi);
            if (anyContainsEi)
            {
                foreach (var w in words.Where(w => w.Contains("ei")))
                    Console.WriteLine("  -> " + w);
            }

            // 2. Grouped products only for categories with at least one out-of-stock product.
            Console.WriteLine("-- 2. Categories with >=1 out-of-stock product --");
            var categoriesWithOutOfStock = products.GroupBy(p => p.Category)
                                                    .Where(g => g.Any(p => p.UnitsInStock == 0));
            foreach (var g in categoriesWithOutOfStock)
            {
                Console.WriteLine("Category: " + g.Key);
                foreach (var p in g)
                    Console.WriteLine("   " + p.ProductName + " (Stock: " + p.UnitsInStock + ")");
            }

            // 3. Grouped products only for categories where ALL products are in stock.
            Console.WriteLine("-- 3. Categories where ALL products are in stock --");
            var categoriesAllInStock = products.GroupBy(p => p.Category)
                                                .Where(g => g.All(p => p.UnitsInStock > 0));
            foreach (var g in categoriesAllInStock)
            {
                Console.WriteLine("Category: " + g.Key);
                foreach (var p in g)
                    Console.WriteLine("   " + p.ProductName + " (Stock: " + p.UnitsInStock + ")");
            }

            Console.WriteLine();
        }
    }
}
