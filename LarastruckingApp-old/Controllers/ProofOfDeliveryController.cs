using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    public class ProofOfDeliveryController : BaseController
    {
        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly IShipmentBAL shipmentBAL = null;
        private readonly IFumigationBAL fumigationBAL = null;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iShipmentBAL"></param>
        public ProofOfDeliveryController(IShipmentBAL iShipmentBAL, IFumigationBAL ifumigationBAL)
        {
            shipmentBAL = iShipmentBAL;
            fumigationBAL = ifumigationBAL;
        }
        #endregion
        // GET: ProofOfDelivery
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShipmentProofOfDelivery(string shipmentId)
        {
            List<GetShipmentRouteStopDTO> objShipmentRouteStop = new List<GetShipmentRouteStopDTO>();
            if (!string.IsNullOrEmpty(shipmentId))
            {
                
                objShipmentRouteStop = shipmentBAL.ShipmentProofOfDelivery(shipmentId);
            }
            return View(objShipmentRouteStop);
        }

        public ActionResult FumigationProofOfDelivery(string fumigationId)
        {
            List<GetFumigationRouteDTO> objFumigationRouteStop = new List<GetFumigationRouteDTO>();
            if (!string.IsNullOrEmpty(fumigationId))
            {

                objFumigationRouteStop = fumigationBAL.FumigationProofOfDelivery(fumigationId);
            }
            return View(objFumigationRouteStop);
        }

        #endregion
    }
}