using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESLTesProcess.Data
{
    public sealed class ProcessControl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // -- Singleton pattern -- //
        private static volatile ProcessControl _instance;
        private static readonly object SyncRoot = new object();

        public static ProcessControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ProcessControl();
                    }
                }
                return _instance;
            }
        }

        public bool AddTechnician(string technicianName)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    entities.technicians.Add(new technician { technician_name = technicianName, technician_create_timestamp = DateTime.Now });
                    return entities.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return false;
        }

        public string[] GetTechnicianNames()
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    return entities.technicians.Select(t => t.technician_name).ToArray();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return new string[0];
        }
    }
}
