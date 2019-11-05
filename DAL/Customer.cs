﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Customer
    {
        public int IdCustomer { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public City City { get; set; }
    }
}