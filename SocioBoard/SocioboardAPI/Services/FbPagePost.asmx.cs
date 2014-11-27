﻿using Api.Socioboard.Helper;
using Api.Socioboard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Api.Socioboard.Services
{
    /// <summary>
    /// Summary description for Group
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class FbPagePost : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string GetPostDetails(string postid)
        {
            try
            {
                Guid id = Guid.Parse(postid);
                FbPagePostRepository objFbPagePostRepository = new FbPagePostRepository();

                Domain.Socioboard.Domain.FbPagePost lstfbmsgs = objFbPagePostRepository.GetPostDetails(id);
                return new JavaScriptSerializer().Serialize(lstfbmsgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new JavaScriptSerializer().Serialize("Please Try Again");
            }

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string GetAllPostOfPage(string profileid,string userid)
        {
            try
            {
                Guid UserId = Guid.Parse(userid);
                FbPagePostRepository objFbPagePostRepository = new FbPagePostRepository();

                List<Domain.Socioboard.Domain.FbPagePost> lstfbmsgs = objFbPagePostRepository.getAllPost(profileid, UserId);
                return new JavaScriptSerializer().Serialize(lstfbmsgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new JavaScriptSerializer().Serialize("Please Try Again");
            }

        }
    }
}
