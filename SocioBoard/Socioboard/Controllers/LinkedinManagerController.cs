﻿using log4net;
using Newtonsoft.Json.Linq;
using Socioboard.Api.Linkedin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Socioboard.Controllers
{
    public class LinkedinManagerController : Controller
    {
        //
        // GET: /LinkedinManager/
        ILog logger=LogManager.GetLogger(typeof(LinkedinManagerController));
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Linkedin()
        {
            Session["LinkedinCompanyPage"] = null;
            Domain.Socioboard.Domain.User objUser = (Domain.Socioboard.Domain.User)Session["User"];
            string oauth_token = Request.QueryString["oauth_token"];
            string oauth_verifier = Request.QueryString["oauth_verifier"];
            string reuqestTokenSecret=Session["reuqestTokenSecret"].ToString();
            if (Session["linSocial"] == "a")
            {
                Api.Linkedin.Linkedin objApiLinkedin = new Api.Linkedin.Linkedin();
                string AddLinkedinAccount = objApiLinkedin.AddLinkedinAccount(oauth_token, oauth_verifier, reuqestTokenSecret, ConfigurationManager.AppSettings["LiApiKey"], ConfigurationManager.AppSettings["LiSecretKey"], objUser.Id.ToString(), Session["group"].ToString());
                Session["SocialManagerInfo"] = AddLinkedinAccount;
                return RedirectToAction("Index", "Home");
            }
            else

            {
                Api.LinkedinCompanyPage.LinkedinCompanyPage objLiCompanyPage = new Api.LinkedinCompanyPage.LinkedinCompanyPage();

                List<Helper.AddliPage> lstLinkedinCompanyPage = new List<Helper.AddliPage>();

                string page = objLiCompanyPage.GetLinkedinCompanyPage(oauth_token, oauth_verifier, reuqestTokenSecret, ConfigurationManager.AppSettings["LiApiKey"], ConfigurationManager.AppSettings["LiSecretKey"], objUser.Id.ToString(), Session["group"].ToString());
                lstLinkedinCompanyPage = (List<Helper.AddliPage>)(new JavaScriptSerializer().Deserialize(page, typeof(List<Helper.AddliPage>)));
                Session["LinkedinCompanyPage"] = lstLinkedinCompanyPage;
                return RedirectToAction("Index", "Home", new { hint = "linpage" });
            }
        }
        public ActionResult AuthenticateLinkedin()
        {

            try
            {
                try
                {
                    Api.Groups.Groups objApiGroups = new Api.Groups.Groups();
                    JObject group = JObject.Parse(objApiGroups.GetGroupDetailsByGroupId(Session["group"].ToString().ToString()));
                    int profilecount = 0;
                    int totalaccount = 0;
                    try
                    {
                        profilecount = (Int16)(Session["ProfileCount"]);
                        totalaccount = (Int16)Session["TotalAccount"];
                    }
                    catch (Exception ex)
                    {
                        logger.Error("ex.Message : " + ex.Message);
                        logger.Error("ex.StackTrace : " + ex.StackTrace);
                    }
                    if (Convert.ToString(group["GroupName"]) == "Socioboard")
                    {
                        if (profilecount < totalaccount)
                        {
                            Session["linSocial"] = "a";
                            Api.Linkedin.Linkedin objApiLinkedin = new Api.Linkedin.Linkedin();
                            string redircturl = objApiLinkedin.GetLinkedinRedirectUrl(ConfigurationManager.AppSettings["LiApiKey"], ConfigurationManager.AppSettings["LiSecretKey"]);
                            Session["reuqestToken"] = redircturl.Split('~')[1].ToString();
                            Session["reuqestTokenSecret"] = redircturl.Split('~')[2].ToString();
                            redircturl = redircturl.Split('~')[0].ToString();
                            Response.Redirect(redircturl);
                        }
                        else if (profilecount == 0 || totalaccount == 0)
                        {
                            Session["linSocial"] = "a";
                            Api.Linkedin.Linkedin objApiLinkedin = new Api.Linkedin.Linkedin();
                            string redircturl = objApiLinkedin.GetLinkedinRedirectUrl(ConfigurationManager.AppSettings["LiApiKey"], ConfigurationManager.AppSettings["LiSecretKey"]);
                            Session["reuqestToken"] = redircturl.Split('~')[1].ToString();
                            Session["reuqestTokenSecret"] = redircturl.Split('~')[2].ToString();
                            redircturl = redircturl.Split('~')[0].ToString();
                            Response.Redirect(redircturl);
                        }
                        else
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

         public ActionResult linPage_connect()
        {
            string redirectURL = "";
            try
            {
                try
                {
                    Api.Groups.Groups objApiGroups = new Api.Groups.Groups();
                    JObject group = JObject.Parse(objApiGroups.GetGroupDetailsByGroupId(Session["group"].ToString().ToString()));
                    int profilecount = (Int16)(Session["ProfileCount"]);
                    int totalaccount = (Int16)Session["TotalAccount"];
                    if (Convert.ToString(group["GroupName"]) == "Socioboard")
                    {
                        if (profilecount < totalaccount)
                        {
                            Session["linSocial"] = "p";
                            Api.Linkedin.Linkedin objApiLinkedin = new Api.Linkedin.Linkedin();
                            string redircturl = objApiLinkedin.GetLinkedinRedirectUrl(ConfigurationManager.AppSettings["LiApiKey"], ConfigurationManager.AppSettings["LiSecretKey"]);
                            Session["reuqestToken"] = redircturl.Split('~')[1].ToString();
                            Session["reuqestTokenSecret"] = redircturl.Split('~')[2].ToString();
                            redircturl = redircturl.Split('~')[0].ToString();
                            redirectURL = redircturl;
                            //Response.Redirect(redircturl);

                        }
                        else
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
               
            }
            return Content(redirectURL);//View();
        }

         public ActionResult GetLinkedinCompanyPage()
         {
             string ret = string.Empty;
             List<Helper.AddliPage> lstLinkedinCompanyPage = new List<Helper.AddliPage>();
             lstLinkedinCompanyPage = (List<Helper.AddliPage>)Session["LinkedinCompanyPage"];
             if (lstLinkedinCompanyPage.Count > 0)
             {
                 foreach (var item in lstLinkedinCompanyPage)
                 {
                     ret += item.PageId + "`" + item.PageName + "`" + item._Oauth + "~";
                     Session["LinkedinPagedetail"] = item._Oauth;
                 }
                 ret = ret.Substring(0, ret.Length - 1);
                 Session["LinkedinCompanyPage"] = null;

             }
             else
             {
                 ret = "Pages Not Found in this Account!";
             }
             return Content(ret);
         }

         public ActionResult AddLinkedinCompanyPage(string id, string OAuth)
         {

             Domain.Socioboard.Domain.User objUser = new Domain.Socioboard.Domain.User();
             objUser = (Domain.Socioboard.Domain.User)Session["User"];
             oAuthLinkedIn oauthtoken = (oAuthLinkedIn)Session["LinkedinPagedetail"];
             string Oauth = new JavaScriptSerializer().Serialize(oauthtoken);
             //GlobusLinkedinLib.Authentication.oAuthLinkedIn _oauth = (GlobusLinkedinLib.Authentication.oAuthLinkedIn)Session["LinkedinPagedetail"];
             Api.LinkedinCompanyPage.LinkedinCompanyPage objLinkedinCompanypage = new Api.LinkedinCompanyPage.LinkedinCompanyPage();
             string AddLinkedinAccount = objLinkedinCompanypage.AddLinkedinCompanyPage(id, Oauth, objUser.Id.ToString(), Session["group"].ToString());

             return Content("");
         }
    }
}
