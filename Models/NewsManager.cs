using EventDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventSite.Models
{
    public static class NewsManager
    {
        private static EVENT_DBEntities db = new EVENT_DBEntities();

        //public static List<AgendaModel> GetAgendas(int BlockNumber, int BlockSize)
        public static List<TBL_NEWS> GetNews(int? id, int BlockNumber, int BlockSize)
        {
            //tendo em conta que começa com 9 e vai acrescentado 3
            
            int startIndex = (BlockNumber - 1) * BlockSize;
            if (BlockNumber >= 2) { startIndex += 6; }
            var news = db.TBL_NEWS.Where(e => e.EVENT_ID == id).OrderByDescending(e=>e.ROW_ID).ToList().Skip(startIndex).Take(BlockSize).ToList();
            return news;
        }
    }


    public class JsonNewsModel
    {
        public string HTMLString { get; set; }
        public bool NoMoreData { get; set; }
    }

}
