//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESLTesProcess.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class session
    {
        public session()
        {
            this.runs = new HashSet<run>();
        }
    
        public int session_id { get; internal set; }
        public System.DateTime session_time_stamp { get; set; }
        public int session_technician_id { get; set; }
    
        public virtual ICollection<run> runs { get; set; }
        public virtual technician technician { get; set; }
    }
}
