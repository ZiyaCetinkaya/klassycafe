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
    
    public partial class RESERVATION
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public Nullable<int> GUEST_COUNT { get; set; }
        public Nullable<System.DateTime> RESERVATION_DATE { get; set; }
        public string MESSAGE { get; set; }
        public Nullable<int> RESERVATIONSTATE_ID { get; set; }
        public string COMMENT { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
    
        public virtual RESERVATIONSTATE RESERVATIONSTATE { get; set; }
    }
}