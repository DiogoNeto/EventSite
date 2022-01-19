using EventDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventSite.Models
{
    public static class AgendaManager
    {

        private static EVENT_DBEntities db = new EVENT_DBEntities();

        public static List<TBL_AGENDA> GetAgendas(int? id, int BlockNumber, int BlockSize)
        {
            int startIndex = (BlockNumber - 1) * BlockSize;
            var agenda = db.TBL_AGENDA.Where(e => e.EVENT_ID == id).OrderByDescending(e => e.ROW_ID).ToList().Skip(startIndex).Take(BlockSize).ToList();
            return agenda;
        }
    }

    public class JsonModel
    {
        public string HTMLString { get; set; }
        public bool NoMoreData { get; set; }
    }
    
}