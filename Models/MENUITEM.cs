//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace klassycafe.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MENUITEM
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<int> MENUCATEGORY_ID { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public string IMAGE { get; set; }
        public Nullable<int> STATE_ID { get; set; }
        public Nullable<bool> MENUSLIDER_SHOW { get; set; }
    
        public virtual MENUCATEGORY MENUCATEGORY { get; set; }
        public virtual STATE STATE { get; set; }
    }
}
