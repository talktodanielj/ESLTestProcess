using ESLTestProcess.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.UnitTest
{
    [TestFixture]
    public class TestSaveData
    {
        [Test]
        public void TestSaevSession()
        {
            using (Entities entities = new Entities())
            {
                try
                {
                    entities.Database.ExecuteSqlCommand("DELETE FROM sessions");
                }
                catch (Exception ex)
                { }

                try
                {
                    entities.Database.ExecuteSqlCommand("DELETE FROM technicians");
                }
                catch (Exception ex)
                { }
                entities.SaveChanges();
            }

            DataManager.Instance.AddTechnician("dan");

            using (Entities entities = new Entities())
            {
                Assert.AreEqual(1, entities.technicians.Count());
            }

            technician technicianDan;
            using (Entities entities = new Entities())
            {
                technicianDan = entities.technicians.FirstOrDefault(t => t.technician_name == "dan");
                Assert.IsNotNull(technicianDan);
            }

            session newSession = new session { technician = technicianDan, session_time_stamp = DateTime.Now };

            using (Entities entities = new Entities())
            {
                entities.sessions.Attach(newSession);
                newSession = entities.sessions.Add(newSession);
                Assert.IsTrue(entities.SaveChanges() > 0);
                Assert.IsNotNull(newSession);
            }
            
        }
    }
}
