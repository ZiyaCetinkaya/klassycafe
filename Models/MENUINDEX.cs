using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace klassycafe.Models
{
    public class MENUINDEX
    {
        public List<MENUCATEGORY> CATEGORIES { get; set; }
        public List<MENUITEM> ITEMS { get; set; }
    }
}