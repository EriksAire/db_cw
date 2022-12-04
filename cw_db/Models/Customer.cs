using System;
using Microsoft.AspNetCore.Identity;

namespace cw_db.Models
{
    public class Customer : IdentityUser
    {
        public Customer()
        {
            feedBacks = new List<Feedback>();
            Orders = new List<Order>();
        }

        public List<Feedback>? feedBacks { get; set; }

        public List<Order>? Orders { get; set; }
    }
}