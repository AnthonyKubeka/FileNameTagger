﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Category
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public Category(string name)
        {
            Name = name;
            IsChecked = false;
        }
    }
}
