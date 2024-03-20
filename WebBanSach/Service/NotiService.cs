using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebBanSach.Common;


public class NotiService : INotiService
{
    List<Noti> _noTifications = new List<Noti>();


    public List<Noti> GetNotification(string nToUserId, bool bIsGetOnlyUnread)
    {
        _noTifications = new List<Noti>();
        using (IDbConnection con = new SqlConnection(Global.ConnectionString))
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var query = "SELECT * FROM View_Notification WHERE ApplicationUserId = @UserId";
            var _oNotis = con.Query<Noti>(query, new { UserId = nToUserId }).ToList();

            if (_oNotis.Count() > 0 && _oNotis != null)
            {
                _noTifications = _oNotis;
            }
            return _noTifications;
        }
    }

}
