using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public sealed class DataManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // -- Singleton pattern -- //
        private static volatile DataManager _instance;
        private static readonly object SyncRoot = new object();

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DataManager();
                    }
                }
                return _instance;
            }
        }

        private DataManager()
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    entities.Database.ExecuteSqlCommand("ALTER TABLE technicians DROP CONSTRAINT UC_technician_name");
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }

            try
            {
                using (Entities entities = new Entities())
                {
                    entities.Database.ExecuteSqlCommand("ALTER TABLE technicians ADD CONSTRAINT UC_technician_name UNIQUE (technician_name)");
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
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

        public technician GetTechnician(string technicianName)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    return entities.technicians.FirstOrDefault(t => t.technician_name == technicianName);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return null;
        }

        public bool AddSession(session currentSession)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    // Attach first so that the child objects (technician in this case) is bound to the new data context
                    entities.sessions.Attach(currentSession);
                    entities.sessions.Add(currentSession);
                    return entities.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }

        public bool SaveSession(session currentSession)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    entities.sessions.Attach(currentSession);
                    return entities.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }
    }
}
