//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DA.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_RoleScreenMapping : IEntity
    {
        public int RoleScreenMappingID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> ScreenID { get; set; }
        public string ActionType { get; set; }
    
        public virtual tbl_Roles tbl_Roles { get; set; }
        public virtual tbl_Screens tbl_Screens { get; set; }
        public EntityState EntityState { get; set; }
    }
}
