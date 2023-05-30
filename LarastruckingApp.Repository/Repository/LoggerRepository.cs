using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class LoggerRepository : ILoggerRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities loggerContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Defining constructor
        /// </summary>
        public LoggerRepository()
        {
            loggerContext = new LarastruckingDBEntities();
        }
        #endregion

        #region Private Member
        /// <summary>
        /// Add exception
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="trace"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public int AddException(string errorMessage, string trace, string innerException = "")
        {
            try
            {
                tblLog objtblLog = new tblLog()
                {
                    ErrorMessage = errorMessage,
                    InnerException = innerException,
                    Trace = trace,
                    CreatedDate = DateTime.Now
                };
                objtblLog = loggerContext.tblLogs.Add(objtblLog);
                loggerContext.SaveChanges();
                return objtblLog.ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
