﻿using Checkout.Models;
using System.ComponentModel;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<string> _scannedItems = new List<string>();
        private readonly List<SKU> _productList;

        public Checkout(IEnumerable<SKU> productList)
        {
            _productList = productList.ToList();
        }

        public int GetTotalPrice()
        {
            if (_scannedItems.Count == 0)
                return 0;

            return CalculatePrice();
        }

        public void Scan(string item)
        {
            if (!_productList.Any(x => x.ProductName == item))
            {
                throw new ArgumentException($"Product {item} is not a valid product.", nameof(item));
            }

            _scannedItems.Add(item);
        }

        private int CalculatePrice()
        {
            var price = 0;

            foreach(var item in _scannedItems)
            {
                var sku = _productList
                    .FirstOrDefault(x => x.ProductName == item);

                price += sku.UnitPrice;
            }

            return price;
        }
    }
}
