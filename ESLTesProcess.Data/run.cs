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
    
    public partial class run
    {
        public run()
        {
            this.results = new HashSet<result>();
        }
    
        public int run_id { get; internal set; }
        public string run_pcb_unit_id { get; set; }
        public System.DateTime run_start_timestamp { get; set; }
        public System.DateTime run_complete_timestamp { get; set; }
        public int run_session_id { get; set; }
    
        public virtual session session { get; set; }
        public virtual ICollection<result> results { get; set; }
        public virtual pcb_unit pcb_unit { get; set; }
    }
}
