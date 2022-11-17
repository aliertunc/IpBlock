using IpBlock.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpBlock.Middleware
{
    public class TraceIP : ActionFilterAttribute
    {
        IPDetail model = new IPDetail();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {


                var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                //var remoteIp =  context.HttpContext.Connection.RemoteIpAddress.ToString();
                #region FirstRequest


                //first request per each ipAddress
                if (context.HttpContext.Session.GetString(ip) == null)
                {
                    model.Count = 1;
                    model.IPAddress = ip;
                    model.Time = DateTime.Now;

                    context.HttpContext.Session.SetString(ip, JsonConvert.SerializeObject(model));
                }
                #endregion
                else
                {
                    #region CheckDbNotAllowList
                    //Todo
                    #endregion

                    #region MultipleRequest 
                    var record = JsonConvert.DeserializeObject<IPDetail>(context.HttpContext.Session.GetString(ip));
                    //many request for same ipAddress
                    if (DateTime.Now.Subtract(record.Time).TotalMinutes < 1 && record.Count > 1)
                    {
                        context.Result = new JsonResult("Permission denined Reason is Many Request!");
                        //Add this ip to BlackList
                    }
                    else
                    {
                        record.Count = record.Count + 1;
                        context.HttpContext.Session.Remove(ip);
                        context.HttpContext.Session.SetString(ip, JsonConvert.SerializeObject(record));
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }
    }
}
