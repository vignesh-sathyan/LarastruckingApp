using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

public partial class GpsTracker : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //StatusLabel.Visible = false;
        FormsAuthentication.SignOut();
        if (!IsPostBack)
        {
            //  Do the expensive operations only the 
            //  first time the page is loaded.
            RunPostAsync();
            GetVehicleListLive();
            GetVehicleList();
        }
        
        //Response.Write("REsult");
    }

    public  string RunPostAsync()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var request = (HttpWebRequest)WebRequest.Create("https://qws.quartix.com/v2/api/auth");
        var UserName = ConfigurationManager.AppSettings["UserName"];
        var Password = ConfigurationManager.AppSettings["Password"];
        var CustomerID = ConfigurationManager.AppSettings["CustomerID"];
        var Application = ConfigurationManager.AppSettings["Application"];

        var postData = "UserName=" + UserName;
        postData += "&Password=" + Password;
        postData += "&CustomerID=" + CustomerID;
        postData += "&Application=" + Application;
        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
       
        JObject obj = JObject.Parse(responseString);
     
       var accessToken = obj["Data"]["AccessToken"];
       var refreshToken = obj["Data"]["RefreshToken"];
        GpsAccessToken.Text = accessToken.ToString();
        GpsRefreshToken.Text = refreshToken.ToString();
        //Response.Write("Response: {0}", res);
        //HttpContext.Current.Response.Write("accessToken: "+ accessToken + "</br>"+ "RefreshToken: "+ refreshToken);
        return responseString.ToString();
    }

    public string GetVehicleListLive()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var request = (HttpWebRequest)WebRequest.Create("https://qws.quartix.com/v2/api/vehicles/live");
        request.Method = "GET";

        CookieContainer requestCookieContainer = new CookieContainer();
        Cookie requiredCookie = new Cookie("quartixAuth", GpsAccessToken.Text);
        requiredCookie.Domain = "qws.quartix.com";
        requestCookieContainer.Add(requiredCookie);
        request.CookieContainer = requestCookieContainer;
        

        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //HttpContext.Current.Response.Write("VehicleList: " + responseString + "</br>");
        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var jsonObject = serializer.DeserializeObject(responseString);
        VehicleListLive.Text = responseString;
        //HttpContext.Current.Response.Write("VehicleList: " + responseString + "</br>");
        return responseString;
   
        //return webcontent.ToString();
    }

    public string GetVehicleList()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var request = (HttpWebRequest)WebRequest.Create("https://qws.quartix.com/v2/api/vehicles");
        request.Method = "GET";

        CookieContainer requestCookieContainer = new CookieContainer();
        Cookie requiredCookie = new Cookie("quartixAuth", GpsAccessToken.Text);
        requiredCookie.Domain = "qws.quartix.com";
        requestCookieContainer.Add(requiredCookie);
        request.CookieContainer = requestCookieContainer;


        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //HttpContext.Current.Response.Write("VehicleList: " + responseString + "</br>");
        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var jsonObject = serializer.DeserializeObject(responseString);
        VehicleList.Text = responseString;
        //HttpContext.Current.Response.Write("VehicleList: " + responseString + "</br>");
        return responseString;

        //return webcontent.ToString();
    }
}


