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
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public partial class SendSMS : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //StatusLabel.Visible = false;
        FormsAuthentication.SignOut();
        if (!IsPostBack)
        {
            //  Do the expensive operations only the 
            //  first time the page is loaded.

            //SendMessageTest();
            //VehicleListLive.Text = SendMessage();
        }
        
        //Response.Write("REsult");
    }

    [System.Web.Services.WebMethod()]
    public static string SendMessage(string picLoc,string phone)
    {
        try
        {
            string accountSid = ConfigurationManager.AppSettings["accountSid"];
            string authToken = ConfigurationManager.AppSettings["authToken"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            TwilioClient.Init(accountSid, authToken);

           var messagebody  = "LARAS DISPATCH &lt;br/&gt;&lt; SHIPMENT ORDER PU: " + picLoc+"";

           //var messagebody2  = "<![CDATA[<span >DEL: " + delLoc + "</span></br><span> # " + AWB + "</span></br><span>TR # " + Equipment + "</span>]]>";
          // var messagebody3  = "<![CDATA[<span> Plts: " + pallets + " </span></br><span>Bxs: " + boxes + "</span></br><span>Cuenta: " + customer + "]]>";
            var message = MessageResource.Create(
                body: picLoc,
                from: new Twilio.Types.PhoneNumber("(802) 347-6625"),
                to: new Twilio.Types.PhoneNumber(phone)
            );
       
            Console.WriteLine(message.Sid);
            HttpContext.Current.Response.Write(message.Sid);

            return message.Sid;
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
            return ex.Message.ToString();
        }

    }

    [System.Web.Services.WebMethod()]
    public static string PickupMessage(string pickupDetails,string phone)
    {
        try
        {
            string accountSid = ConfigurationManager.AppSettings["accountSid"];
            string authToken = ConfigurationManager.AppSettings["authToken"];
            //var message;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            TwilioClient.Init(accountSid, authToken);
            //var pickupMessage[] = pickupDetails;

            var message = MessageResource.Create(
            body: pickupDetails,
          //  body: "laras dispatch\n" + "fumigation loading\n" + "tr # " + pickupdetails[0].equipment,
            from: new Twilio.Types.PhoneNumber("(802) 347-6625"),
            to: new Twilio.Types.PhoneNumber(phone)
        );


            return message.Sid;
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
            return ex.Message.ToString();
        }

    }

}


