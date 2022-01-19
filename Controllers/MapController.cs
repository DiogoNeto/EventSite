using EventSite.Models;
//using EventSite.Utils;
using EventDAL.Model;
using EventDAL.ModelAux;
using EventDAL.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventSite.Controllers
{

    public class MapController : Controller
    {
        private UserProcedure dbUser = new UserProcedure();
        private ParamProcedure dbParam = new ParamProcedure();
        private string result;
        //[MyAuthorize]
        public ActionResult Global(int id)
        {

            ViewBag.eventId = id;

            //    SessionModel sessModel = (SessionModel)Session["sessionmodel"];
                FollowMeModel fmodel = new FollowMeModel();
                fmodel.Event_id = id;
                fmodel.User_id = 0;
                fmodel.Users = dbUser.GetUsersByEvent(id,false,out result);
                var paramsList = dbParam.GetParamsByEvent(1007,out result);
                var param = paramsList.FirstOrDefault(p => p.PARAM_ID == Constants.mapRefreshParamId);
                if (param != null)
                {
                    fmodel.RefreshInterval = Convert.ToInt32(param.VALOR_PARAM);
                }
                else
                {
                    fmodel.RefreshInterval = 15;
                }

            //    return View("Map",fmodel);
            return View("Map", fmodel);
        }

        public ActionResult FollowMe(long id, int event_id)
        {

            try
            {
                string value = id.ToString();
                int firstHalf = Convert.ToInt32(value.Substring(0, 8));
                int secondHalf = Convert.ToInt32(value.Substring(8, value.Length - 8));
                int userId = secondHalf - firstHalf;
                //int userId = (int)id;
                var user = dbUser.GetUserById(userId, false, out result);
                if (user == null)
                {
                    return RedirectToAction("Index", "Error", new { message = "The person you are trying to follow does not exist, make sure you have the correct link." });
                }
                FollowMeModel fmodel = new FollowMeModel();
                fmodel.Event_id = event_id;
                fmodel.User_id = (int)userId;
                return View("MapFollowme", fmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { message = "The person you are trying to follow does not exist, make sure you have the correct link." });
            }
        }

        public ActionResult Coordinates(int id, int event_id)
        {

            //Random rnd = new Random();                 
            //List<sp_USER_FRIEND_SELECT_Result> data = new List<sp_USER_FRIEND_SELECT_Result>();
            //for (int i = 0; i < 5; i++)
            //{
            //    double lat = 38.743462 + rnd.NextDouble() * 0.001;
            //    double lon = -9.146843 - rnd.NextDouble() * 0.001;
            //    data.Add(new sp_USER_FRIEND_SELECT_Result { OWNER_ID = i, LATITUDE = lat.ToString(), LONGITUDE = lon.ToString(), USERNAME = "Vera", LAST_UPDT = DateTime.UtcNow });
            //}
            //data.Add(new sp_USER_FRIEND_SELECT_Result { LATITUDE = lat.ToString(), LONGITUDE = Math.Abs(lon).ToString(), USERNAME = "Vera", LAST_UPDT = DateTime.UtcNow });
            //data.Add(new sp_USER_FRIEND_SELECT_Result { LATITUDE = lat.ToString(), LONGITUDE = lon.ToString(), USERNAME = "Nuno Silva", LAST_UPDT = DateTime.UtcNow });
            //var data = ApiCalls.GetAll<sp_USER_FRIEND_SELECT_Result>("http://setupevent.worldit.pt/service/","api/Friend/GetFriendsWithCoord/20?eventId=1007");
            string result;
            dynamic data;
            try
            {
                if (id > 0)
                {
                    //sp_USER_COORD_SELECT_Result user = dbUser.GetUserByIdWithCoord(id, event_id, out result);
                    //data = new List<sp_USER_COORD_SELECT_Result>();
                    //if (user != null)
                    //{

                    //    data.Add(user);
                    //}
                    data = dbUser.GetUserCoordTopX(id, 1, out result);
                }
                else
                {
                    data = dbUser.GetUsersWithCoord(event_id, out result);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
            return Json(data, JsonRequestBehavior.AllowGet); ;
        }
        [HttpPost]
        public ActionResult Coordinates(string users)
        {
            int[] usersArray = JsonConvert.DeserializeObject<int[]>(users);
            string query = "";
            IQueryable<sp_USER_COORD_SELECT_ROW_Result> coords = null;
            if (users != null)
            {
                try
                {
                    foreach (var userId in usersArray)
                    {
                        query += "INSERT INTO TMP_USER(USER_ID) VALUES(" + userId + ") ";
                    }
                    coords = dbUser.GetMultUsersByIdWithCoord(query, out result);
                }
                catch (Exception e)
                {
                    return Json(new List<sp_USER_COORD_SELECT_ROW_Result>(), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(coords, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get User coordinates by dates
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="eid">Event id</param>
        /// <param name="sd">Start date</param>
        /// <param name="ed">End date</param>
        /// <returns></returns>
        public ActionResult GetUserCourse(int id, int eid, DateTime sd, DateTime ed)
        {
            IEnumerable<sp_TIME_SELECT_ROUTE_Result> coordinates = dbUser.GetUserRouteByDate(id, eid, sd, ed, out result);
            if (coordinates == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result);
            }
            return Json(coordinates, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetInterestPoints(int event_id)
        {
            PointProcedure db = new PointProcedure();
            IEnumerable<sp_POINT_SELECT_EVENT_Result> points = db.GetPointsByEvent(event_id, out result);

            db.Dispose();
            if (points == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result);
            }

            foreach (var item in points)
            {
                item.IMAGE_B64 = Convert.ToBase64String(item.IMAGE);
            }
            return Json(points, JsonRequestBehavior.AllowGet);
        }
    }
}