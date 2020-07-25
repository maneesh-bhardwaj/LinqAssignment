using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace LinqExercise
{
    class Program
    {
        static void Main(string[] args)
        {

            //populating data
            CustomerList customers = new CustomerList();
            customers.Add(new Customer() { CustomerNumber = 100, CustomerName = "Customer_100" });
            customers.Add(new Customer() { CustomerNumber = 101, CustomerName = "Customer_101" });
            customers.Add(new Customer() { CustomerNumber = 102, CustomerName = "Customer_102" });
            customers.Add(new Customer() { CustomerNumber = 103, CustomerName = "Customer_103" });

            OrderList orders = new OrderList();
            orders.Add(new Order() { OrderNumber = 1001, CustomerNumber = 103, NumberOfItems = 5 });
            orders.Add(new Order() { OrderNumber = 1002, CustomerNumber = 100, NumberOfItems = 100 });
            orders.Add(new Order() { OrderNumber = 1003, CustomerNumber = 101, NumberOfItems = 1 });
            orders.Add(new Order() { OrderNumber = 1004, CustomerNumber = 100, NumberOfItems = 101 });
            orders.Add(new Order() { OrderNumber = 1005, CustomerNumber = 100, NumberOfItems = 101 });
            orders.Add(new Order() { OrderNumber = 1006, CustomerNumber = 101, NumberOfItems = 101 });

            // 3.print customers in alphabetical order
            Console.WriteLine("3.print customers in alphabetical order");
            foreach (Customer customer in customers.OrderBy(x => x.CustomerName))
            {
                Console.WriteLine(customer.CustomerNumber + " " + customer.CustomerName);
            }

            // 4.Print a list of Orders in Customer # order.
            Console.WriteLine("\n4.Print a list of Orders in Customer # order.");
            foreach (Order order in orders.OrderBy(x => x.CustomerNumber))
            {
                Console.WriteLine(order.CustomerNumber + " " + order.OrderNumber + " " + order.NumberOfItems);
            }

            // 5.Print a list of Orders grouped by Customer # and in ascending Order # sequence
            Console.WriteLine("5.Print a list of Orders grouped by Customer # and in ascending Order # sequence");
            foreach (Order order in orders.OrderBy(x => x.CustomerNumber).OrderBy(x => x.OrderNumber))
            {
                Console.WriteLine(order.CustomerNumber + " " + order.OrderNumber + " " + order.NumberOfItems);
            }

            // 6.Print a list of orders that includes the Customer name.
            Console.WriteLine("6.Print a list of orders that includes the Customer name.");
            /*  var innerJoinQuery =
                  from order in orders
                  join customer in customers on order.CustomerNumber equals customer.CustomerNumber
                  select new { CustomerName = customer.CustomerName, 
                      CustomerNumber = order.CustomerNumber, 
                      OrderNumber = order.OrderNumber,
                      NumberOfItems = order.NumberOfItems };*/


            foreach (var order in orders.Join(customers, o => o.CustomerNumber, c => c.CustomerNumber, (o, c) => new { c.CustomerName, c.CustomerNumber, o.OrderNumber, o.NumberOfItems }))
            {
                Console.WriteLine(order.CustomerName + " " + order.CustomerNumber + " " + order.OrderNumber + " " + order.NumberOfItems);
            }

            // 7.Print a list of orders grouped by Customer with the Customer Name
            Console.WriteLine("7.Print a list of orders grouped by Customer with the Customer Name");

            /*foreach (var order in orders.Join(customers, o => o.CustomerNumber, c => c.CustomerNumber, (o, c) => new { c.CustomerName, o.CustomerNumber, o.OrderNumber, o.NumberOfItems }).OrderBy(x => x.CustomerName))
            {
                Console.WriteLine(order.CustomerName + " " + order.CustomerNumber + " " + order.OrderNumber + " " + order.NumberOfItems);
            }*/
            foreach (var cust in orders.Join(customers,o=>o.CustomerNumber,c=>c.CustomerNumber,(o,c)=> new {o.CustomerNumber,c.CustomerName,o.OrderNumber,o.NumberOfItems }).GroupBy(g=> g.CustomerNumber))
            {
                Console.WriteLine($"CustomerName: {cust.Key}");
                foreach (var item in cust)
                {
                    Console.WriteLine($"---{item.OrderNumber}  -  {item.NumberOfItems}");
                }
            }

            // 8.Print a list of orders in grouped by Customer and in reverse sequence based on Customer Name. 
            Console.WriteLine("8.Print a list of orders in grouped by Customer and in reverse sequence based on Customer Name. ");
            foreach (var order in orders.Join(customers, o => o.CustomerNumber, c => c.CustomerNumber, (o, c) => new { c.CustomerName, o.CustomerNumber, o.OrderNumber, o.NumberOfItems }).OrderByDescending(x => x.CustomerName))
            {
                Console.WriteLine(order.CustomerName + " " + order.CustomerNumber + " " + order.OrderNumber + " " + order.NumberOfItems);
            }

            // 9.Print a list of Customers who have placed an order 
            Console.WriteLine("9.Print a list of Customers who have placed an order ");
            foreach (var order in orders.Join(customers, o => o.CustomerNumber, c => c.CustomerNumber, (o, c) => new { c.CustomerName }).GroupBy(x => x.CustomerName))
            {
                Console.WriteLine(order.Key);
            }

            // 10.Count the # of orders that each Customer has and total number of items they have ordered.
            Console.WriteLine("10.Count the # of orders that each Customer has and total number of items they have ordered.");
            foreach (var order in orders.Join(customers, o => o.CustomerNumber, c => c.CustomerNumber, (o, c) => new { c.CustomerName, o.NumberOfItems }).GroupBy(x => x.CustomerName))
            {
                Console.WriteLine(order.Key + " " + order.Count() + " " + order.Sum(n => n.NumberOfItems));
            }
            // 11.Print a list of orders with more than 10 items ordered
            Console.WriteLine("11.Print a list of orders with more than 10 items ordered");
            foreach (var order in orders.Join(customers, o => o.CustomerNumber, c => c.CustomerNumber, (o, c) => new { c.CustomerName, o.NumberOfItems }).GroupBy(x => x.CustomerName).Where(n => n.Sum(x => x.NumberOfItems) > 10))
            {
                Console.WriteLine(order.Key + " " + order.Count() + " " + order.Sum(n => n.NumberOfItems));
            }
            // 12.Print the first 3 orders.
            Console.WriteLine("12.Print the first 3 orders.");
            foreach (var order in orders.Take(3))
            {
                Console.WriteLine(order.OrderNumber + " " + order.CustomerNumber + " " + order.NumberOfItems);
            }

            // 13.Print a list of orders – stop when one of the orders has only 1 item.
            Console.WriteLine("13.Print a list of orders – stop when one of the orders has only 1 item.");
            foreach (var order in orders.TakeWhile(x => x.NumberOfItems == 1))
            {
                Console.WriteLine(order.OrderNumber + " " + order.CustomerNumber + " " + order.NumberOfItems);
            }

            // 14.Create an array – array1 – of 10 integers containing random #’s from 1 to 10.
            Console.WriteLine("14.Create an array – array1 – of 10 integers containing random #’s from 1 to 10.");
            Random random = new Random();
            int[] array1 = Enumerable.Repeat(0, 10).Select(i => random.Next(1, 10)).ToArray();
            foreach (int item in array1)
                Console.WriteLine($"{item}");
            // 15.Create an array – array2 – of 10 integers containing random #’s from 5 to 15;
            Console.WriteLine("15.Create an array – array2 – of 10 integers containing random #’s from 5 to 15;");
            int[] array2 = Enumerable.Repeat(0, 10).Select(i => random.Next(5, 15)).ToArray();
            foreach (int item in array2)
                Console.WriteLine($"{item}");
            // 16.Print only the values from array1 – ensuring that no duplicates are printed.
            Console.WriteLine("16.Print only the values from array1 – ensuring that no duplicates are printed.");
            foreach (int item in array1.Distinct())
                Console.WriteLine(item);
            // 17.Print the union of array1 and array2.
            Console.WriteLine("17.Print the union of array1 and array2.");
            foreach (int item in array1.Union(array2))
                Console.WriteLine(item);
            // 18.Print the intersection of array1 and array2.
            Console.WriteLine("18.Print the intersection of array1 and array2.");
            foreach (int item in array1.Intersect(array2))
                Console.WriteLine(item);
            // 19.Print the contents of array2 beginning with the first number > 10.
            Console.WriteLine("19.Print the contents of array2 beginning with the first number > 10.");
            foreach (var item in array2.SkipWhile(x => x < 11))
                Console.WriteLine(item);
            // 20.Print the numbers in array2 that do not also appear in array1.
            Console.WriteLine("20.Print the numbers in array2 that do not also appear in array1.");
            //foreach (var item in array2.GroupJoin(array1, a => a, b => b, (a, b) => new { a, b }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, y }).Where(y => y.y == 0).Select(a => a.a))
            foreach (var item in array2.Except(array1))
            {
                Console.WriteLine(item);
            }
        }

    }

    public class CustomerList : IEnumerable<Customer>
    {
        private List<Customer> list = new List<Customer>();

        public void Add(Customer customer)
        {
            list.Add(customer);
        }

        IEnumerator<Customer> IEnumerable<Customer>.GetEnumerator()
        {
            return ((IEnumerable<Customer>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }

    public class Customer
    {
        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }

    }


    public class OrderList : IEnumerable<Order>
    {
        private List<Order> list = new List<Order>();

        public void Add(Order order)
        {
            list.Add(order);
        }

        IEnumerator<Order> IEnumerable<Order>.GetEnumerator()
        {
            return ((IEnumerable<Order>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }

    public class Order
    {

        public int OrderNumber { get; set; }
        public int CustomerNumber { get; set; }
        public int NumberOfItems { get; set; }

    }





}
