//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShopping.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order_Modification
    {
        public int Modification_ID { get; set; }
        public string Username_Modify { get; set; }
        public int Order_ID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Employee_Account Employee_Account { get; set; }
        public virtual Order_Details Order_Details { get; set; }
    }
}
