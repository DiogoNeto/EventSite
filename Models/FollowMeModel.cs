using EventDAL.Model;
using EventDAL.ModelAux;
using EventDAL.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventSite.Models
{
    public class FollowMeModel
    {
        public int User_id { get; set; }
        public int Event_id { get; set; }
        public IQueryable<sp_USER_SELECT_EVENT_Result> Users { get; set; }
        public int RefreshInterval { get; set; }

        public int? GetDefaultRefreshInterval(out string result)
        {
            ParamProcedure dbParam = new ParamProcedure();               
            var param = dbParam.GetParamByParamId(Constants.mapRefreshParamId, Event_id,out result);
            if (param == null)
            {
                return null;
            }
            else
            {
                return param.EVENT_ID;
            }
            
        } 

    }
}