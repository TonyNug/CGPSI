//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CGPSI.AbsenceManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewEmployee
    {
        public int ID_Employee { get; set; }
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public System.DateTime JoinDate { get; set; }
        public Nullable<int> ID_CurrentPosition { get; set; }
        public string CurrentPosition { get; set; }
        public Nullable<int> ID_CurrentDepartment { get; set; }
        public string CurrentDepartment { get; set; }
        public Nullable<int> ID_Supervisor { get; set; }
        public string SPVFirstName { get; set; }
        public string SPVLastName { get; set; }
        public string SPVDisplayName { get; set; }
    }
}
