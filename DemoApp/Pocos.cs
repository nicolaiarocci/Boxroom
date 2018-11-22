using System;

namespace DemoApp
{
    public class Customer
    {
        public string Zip { get; set; }
        public string Name { get; set; }
    }
    public class Invoice
    {
        public DateTime Date { get; set; }
        public string Number { get; set; }
    }
}