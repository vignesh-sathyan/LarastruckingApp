using LarastruckingApp.Entities.MailHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IMailHistoryRepository
    {
        MailHistoryDTO Add(MailHistoryDTO entity);

    }
}
