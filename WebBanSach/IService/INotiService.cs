using BanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.DataAccess.Repository.IRepository
{
    public interface INotiService
    {
        List<Noti> GetNotification(string nToUserId, bool bIsGetOnlyUnread);
    }
}
