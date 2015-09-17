using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlServerCe;
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
                throw;
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
                throw;
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
                throw;
            }
            return null;
        }

        public session AddSession(session currentSession)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    // Attach first so that the child objects (technician in this case) is bound to the new data context
                    entities.sessions.Attach(currentSession);
                    entities.sessions.Add(currentSession);
                    entities.SaveChanges();
                    return currentSession;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }
        
        public session AddRun(session currentSession, run currentRun, pcb_unit pcbUnit, bool isNewPCB)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    entities.sessions.Attach(currentSession);

                    currentRun.run_complete_timestamp = currentRun.run_start_timestamp = DateTime.Now;
                    currentRun.session = currentSession;
                    if (isNewPCB)
                    {
                        entities.pcb_unit.Add(currentRun.pcb_unit);
                    }
                    else
                    {
                        //entities.Entry(currentRun.pcb_unit).State = EntityState.Unchanged;
                        //entities.pcb_unit.Attach(currentRun.pcb_unit);
                    }
                    currentSession.runs.Add(currentRun);
                    entities.runs.Add(currentRun);

                    foreach (var responseItem in currentRun.responses)
                    {
                        entities.responses.Add(responseItem);
                    }

                    
                    entities.SaveChanges();
                    return entities.sessions.First(s => s.session_id == currentSession.session_id);

                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }

        public pcb_unit GetTestUnit(string manufacturesSerial)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    return entities.pcb_unit.FirstOrDefault(p => p.pcb_unit_serial_sticker_manufacture == manufacturesSerial);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }

        public session SaveResponses(session currentSession)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    entities.sessions.Attach(currentSession);
                    entities.runs.Attach(currentSession.runs.Last());

                    currentSession.runs.Last().run_complete_timestamp = DateTime.Now; // Update on every save
                    foreach (var responseItem in currentSession.runs.Last().responses)
                    {
                        entities.responses.Attach(responseItem);
                        entities.Entry(responseItem).State = EntityState.Modified;
                    }
                    
                    entities.SaveChanges();
                    return currentSession;
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

        public void ExportTestData(string fileName)
        {
            try
            {
                using (var exportFile = new System.IO.StreamWriter(fileName, false))
                {
                    using (Entities entities = new Entities())
                    {
                        using (var connection = new SqlCeConnection(entities.Database.Connection.ConnectionString))
                        {
                            SqlCeCommand command = connection.CreateCommand();
                            command.CommandText = ASCIIEncoding.ASCII.GetString(ESLTestProcess.Data.Properties.Resources.results);
                            connection.Open();
                            using (SqlCeDataReader reader = command.ExecuteReader())
                            {
                                // Write out the column headings
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (i > 0)
                                        sb.Append(",");
                                    sb.Append(reader.GetName(i));
                                }
                                exportFile.WriteLine(sb.ToString());
                                
                                // Write the data for each row
                                while (reader.Read())
                                {
                                    sb.Clear();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        if (i > 0)
                                            sb.Append(",");
                                        sb.Append(reader.GetValue(i).ToString());
                                    }
                                    exportFile.WriteLine(sb.ToString());
                                }
                            }
                        }
                    }
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
