//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Restier.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hotel
    {
        public int Id { get; set; }
        public int HotelGroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AddressId { get; set; }
        public bool Enabled { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual HotelGroup HotelGroup { get; set; }
    }
}