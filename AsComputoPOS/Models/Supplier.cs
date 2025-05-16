using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsComputoPOS.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool isActive { get; set; }

        public Supplier(string name, string contactName, string address, string email, string phone, bool isActive)
        {
            Name = name;
            ContactName = contactName;
            Address = address;
            Email = email;
            Phone = phone;
            this.isActive = isActive;
        }


    }
}
