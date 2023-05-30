using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.UploadShipmentDTO;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LarastruckingApp.DTO;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace LarastruckingApp.Repository
{
    public class UploadShipmentRepository : IUploadShipmentRepository
    {
        #region private member
        private readonly LarastruckingDBEntities UploadShipmentContext;

        #endregion

        #region constroctor
        public UploadShipmentRepository()
        {
            UploadShipmentContext = new LarastruckingDBEntities();
        }
        #endregion

        #region GetCompanyName
        /// <summary>
        /// Get Company name
        /// </summary>
        /// <returns></returns>
        public List<CompanyNameDTO> GetCompanyName(UserRoleDTO dto)
        {
            try
            {


                var companyNameList = new List<CompanyNameDTO>();
                if ("Customer" == dto.RoleName && dto.UserID > 0)
                {
                    companyNameList = (from customer in UploadShipmentContext.tblCustomerRegistrations
                                       where customer.CustomerName != null && customer.IsUploadShipment && customer.UserId == dto.UserID && customer.IsDeleted == false && customer.IsActive == true

                                       select new CompanyNameDTO
                                       {
                                           CustomerId = customer.CustomerID,
                                           CustomerName = customer.CustomerName ?? string.Empty,

                                       }
                                           ).ToList();

                }
                else
                {
                    companyNameList = (from customer in UploadShipmentContext.tblCustomerRegistrations
                                       where customer.CustomerName != null && customer.IsUploadShipment && customer.IsDeleted == false && customer.IsActive == true
                                       select new CompanyNameDTO
                                       {
                                           CustomerId = customer.CustomerID,
                                           CustomerName = customer.CustomerName ?? string.Empty,

                                       }
                                          ).ToList();

                }
                return companyNameList.OrderBy(x => x.CustomerName).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetSample
        /// <summary>
        /// Get shipment sample by address id
        /// </summary>
        /// <returns></returns>
        public string GetSample()
        {
            try
            {

                string samplePath = (from sample in UploadShipmentContext.tblUploadShipmentSamples select sample.ShipmentSamplePath).FirstOrDefault();
                return samplePath;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Valided UploadShipent
        /// <summary>
        /// Valided Upload Shipment 
        /// </summary>
        /// <param name="uploadShipmentList"></param>
        /// <returns></returns>

        public List<UploadShipmentDTO> ValidedUploadShipment(List<UploadShipmentDTO> uploadShipmentList)
        {
            try
            {

                foreach (var shipment in uploadShipmentList)
                {
                    shipment.Date = Configurations.ConvertLocalToUTC(Convert.ToDateTime(shipment.Date));
                    shipment.DeliveryDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(shipment.DeliveryDate));
                    shipment.PricingMethodId = IsPricingMethod(shipment.PricingMethod);
                    shipment.FreightTypeId = IsFreightType(shipment.FreightType);
                    shipment.PickUpLocationId = IsLocationValid(shipment.PickUpLocation) != null ? IsLocationValid(shipment.PickUpLocation).PickUpLocationId : 0;
                    shipment.PickUpLocation = IsLocationValid(shipment.PickUpLocation) != null ? IsLocationValid(shipment.PickUpLocation).PickUpLocation : shipment.PickUpLocation.ToUpper();
                    shipment.DeliveryLocationId = IsLocationValid(shipment.DeliveryLocation) != null ? IsLocationValid(shipment.DeliveryLocation).PickUpLocationId : 0;
                    shipment.DeliveryLocation = IsLocationValid(shipment.DeliveryLocation) != null ? IsLocationValid(shipment.DeliveryLocation).PickUpLocation : shipment.DeliveryLocation.ToUpper();
                    shipment.IsAirWayBillNoExist = IsAirWayBill(shipment.AWB, shipment.CustomerPO, shipment.OrderNo);

                    shipment.Unit = shipment.Unit.ToUpper();
                    if (shipment.Unit.ToUpper() == Unit.KG.ToString().ToUpper() || shipment.Unit.ToUpper() == Unit.LBS.ToString().ToUpper() || string.IsNullOrEmpty(shipment.Unit.Trim()))
                    {
                        shipment.IsUnit = true;
                    }

                }
                return uploadShipmentList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsAirWayBill(string airWayBillNo, string customerPo, string orderNo)
        {

            var shipmetnList = UploadShipmentContext.tblShipments.Where(x => x.IsDeleted == false && (airWayBillNo != "" ? x.AirWayBill.Trim() == airWayBillNo.Trim() : 1 == 1) && (customerPo != "" ? x.CustomerPO.Trim() == customerPo.Trim() : 1 == 1) && (orderNo != "" ? x.OrderNo.Trim() == orderNo.Trim() : 1 == 1)).ToList();
            return shipmetnList.Count() > 0;
        }


        public UploadShipmentDTO IsPcsType(string pcsType)
        {
            var pricingMethods = (from pricingMethod in UploadShipmentContext.tblPricingMethods
                                  where pricingMethod.PricingMethodExt.ToLower().Contains(pcsType.Trim().ToLower())
                                  select new UploadShipmentDTO
                                  {
                                      PcsTypeId = pricingMethod.PricingMethodId,
                                      PcsType = pricingMethod.PricingMethodExt.ToUpper(),

                                  }).FirstOrDefault();
            return pricingMethods;
        }
        public int IsPricingMethod(string pricingMethod)
        {
            var pricingMethodId = UploadShipmentContext.tblPricingMethods.Where(x => x.PricingMethodName.Trim().ToLower() == pricingMethod.Trim().ToLower()).Select(x => x.PricingMethodId).FirstOrDefault();

            return pricingMethodId > 0 ? pricingMethodId : 0;

        }

        public int IsFreightType(string freightType)
        {
            var freightTypeId = UploadShipmentContext.tblFreightTypes.Where(x => x.FreightTypeName.Trim().ToLower() == freightType.Trim().ToLower()).Select(x => x.FreightTypeId).FirstOrDefault();
            return freightTypeId > 0 ? freightTypeId : 0;

        }

        public UploadShipmentDTO IsLocationValid(string location)
        {
            var pickUpLocation = (from address in UploadShipmentContext.tblAddresses
                                  join state in UploadShipmentContext.tblStates on address.State equals state.ID
                                  where address.CompanyName.Trim().Contains(location.ToLower().Trim())
                                  select new UploadShipmentDTO
                                  {
                                      PickUpLocation = (address.CompanyName.Trim() + "," + address.Address1.Trim() + " " + address.City.Trim() + " " + state.Name.Trim() + " " + address.Zip.Trim()).ToUpper().Trim(),
                                      PickUpLocationId = address.AddressId,


                                  }).FirstOrDefault();
            return pickUpLocation;
        }



        #endregion

        #region bind pricing method and freight type dropdown
        /// <summary>
        /// bind pricing method and freight type and pcs type dropdown
        /// </summary>
        /// <returns></returns>

        public FreightTypeNPricingMethodDTO BindFreightTypeNPricingMethod()
        {
            try
            {


                FreightTypeNPricingMethodDTO objFreightTypeNPricingMethodDTO = new FreightTypeNPricingMethodDTO();
                objFreightTypeNPricingMethodDTO.PricingMethod = (from pricingMethod in UploadShipmentContext.tblPricingMethods
                                                                 where pricingMethod.IsActive
                                                                 select new GetPricingMethodDTO
                                                                 {
                                                                     PricingMethodId = pricingMethod.PricingMethodId,
                                                                     PricingMethodName = pricingMethod.PricingMethodName,
                                                                 }).ToList();
                objFreightTypeNPricingMethodDTO.FreightType = (from freighType in UploadShipmentContext.tblFreightTypes
                                                               where freighType.IsActive
                                                               select new GetFreightTypeDTO
                                                               {
                                                                   FreightTypeId = freighType.FreightTypeId,
                                                                   FreightTypeName = freighType.FreightTypeName,
                                                               }).ToList();
                objFreightTypeNPricingMethodDTO.PcsType = (from pricingMethod in UploadShipmentContext.tblPricingMethods
                                                           where pricingMethod.IsActive && pricingMethod.PricingMethodExt != null
                                                           select new GetPricingMethodDTO
                                                           {
                                                               PricingMethodId = pricingMethod.PricingMethodId,
                                                               PricingMethodName = pricingMethod.PricingMethodExt,
                                                           }).ToList();

                return objFreightTypeNPricingMethodDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region save excel sheet
        /// <summary>
        /// save excel sheet data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveExcelData(List<UploadShipmentDTO> model)
        {
            try
            {
                bool result = false;
                if (model != null && model.Count > 0)
                {
                    foreach (var modeldata in model)
                    {

                        bool IsWaitingTime = UploadShipmentContext.tblCustomerRegistrations.Where(x => x.CustomerID == modeldata.CustomerId).Select(x => x.IsWaitingTimeRequired).FirstOrDefault();
                        var chkShipment = UploadShipmentContext.tblShipments.Where(x => x.IsDeleted == false && x.ShipmentId==0 && modeldata.AWB != null && modeldata.AWB != "" && x.AirWayBill.Trim() == modeldata.AWB.Trim() && DbFunctions.TruncateTime(Configurations.TodayDateTime) == DbFunctions.TruncateTime(x.CreatedDate)).FirstOrDefault();   
                        if (chkShipment != null)
                        {
                            var chkRoute = UploadShipmentContext.tblShipmentRoutesStops.Where(x => x.ShippingId == chkShipment.ShipmentId && x.IsDeleted == false).FirstOrDefault();
                            if (chkRoute != null)
                            {
                                if (!string.IsNullOrEmpty(modeldata.CustomerPO))
                                {
                                    chkShipment.CustomerPO = chkShipment.CustomerPO + " / " + modeldata.CustomerPO;
                                }
                                if (!string.IsNullOrEmpty(modeldata.OrderNo))
                                {
                                    chkShipment.OrderNo = chkShipment.OrderNo + " / " + modeldata.OrderNo;
                                }


                                UploadShipmentContext.Entry(chkShipment).State = EntityState.Modified;

                                tblShipmentFreightDetail objShipmentFreight = new tblShipmentFreightDetail();
                                objShipmentFreight.ShipmentId = chkShipment.ShipmentId;
                                objShipmentFreight.ShipmentRouteStopeId = chkRoute.ShippingRoutesId;
                                objShipmentFreight.Commodity = modeldata.Commodity;
                                objShipmentFreight.FreightTypeId = modeldata.FreightTypeId;
                                objShipmentFreight.PricingMethodId = (modeldata.PricingMethodId == 0 ? null : modeldata.PricingMethodId);

                                objShipmentFreight.Hazardous = false;
                                objShipmentFreight.Temperature = Convert.ToDecimal(modeldata.ReqTemp);
                                objShipmentFreight.TemperatureType = "F";
                                objShipmentFreight.QuantityNweight = Convert.ToDecimal(modeldata.NoOfPallets);
                                objShipmentFreight.PcsType = modeldata.PcsTypeId;
                                objShipmentFreight.NoOfBox = Convert.ToInt32(modeldata.NoOfBox);
                                objShipmentFreight.Weight = modeldata.Weight;
                                objShipmentFreight.Unit = modeldata.Unit;
                                objShipmentFreight.Comments = modeldata.Comments;

                                if (modeldata.PartialBox > 0 || modeldata.PartialPallet > 0)
                                {
                                    objShipmentFreight.IsPartialShipment = true;
                                    objShipmentFreight.PartialBox = modeldata.PartialBox;
                                    objShipmentFreight.PartialPallete = modeldata.PartialPallet;


                                }

                                UploadShipmentContext.tblShipmentFreightDetails.Add(objShipmentFreight);
                                result = UploadShipmentContext.SaveChanges() > 0;

                            }
                            else
                            {
                                tblShipmentRoutesStop objShipmentRouteStops = new tblShipmentRoutesStop();
                                objShipmentRouteStops.ShippingId = chkShipment.ShipmentId;
                                objShipmentRouteStops.RouteNo = 1;
                                objShipmentRouteStops.PickupLocationId = modeldata.PickUpLocationId;
                                objShipmentRouteStops.PickDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date));
                                objShipmentRouteStops.PickUpDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date));
                                objShipmentRouteStops.DeliveryLocationId = modeldata.DeliveryLocationId;
                                objShipmentRouteStops.DeliveryDateTime = modeldata.DeliveryDate == null ? Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date)) : Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.DeliveryDate));
                                objShipmentRouteStops.DeliveryDateTimeTo = modeldata.DeliveryDate == null ? Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date)) : Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.DeliveryDate));
                                objShipmentRouteStops.IsPickUpWaitingTimeRequired = IsWaitingTime;
                                objShipmentRouteStops.IsDeliveryWaitingTimeRequired = IsWaitingTime;
                                UploadShipmentContext.tblShipmentRoutesStops.Add(objShipmentRouteStops);



                                tblShipmentFreightDetail objShipmentFreight = new tblShipmentFreightDetail();
                                objShipmentFreight.ShipmentId = chkShipment.ShipmentId;
                                objShipmentFreight.ShipmentRouteStopeId = objShipmentRouteStops.ShippingRoutesId;
                                objShipmentFreight.Commodity = modeldata.Commodity;
                                objShipmentFreight.FreightTypeId = modeldata.FreightTypeId;
                                objShipmentFreight.PricingMethodId = (modeldata.PricingMethodId == 0 ? null : modeldata.PricingMethodId);

                                objShipmentFreight.Hazardous = false;
                                objShipmentFreight.Temperature = Convert.ToDecimal(modeldata.ReqTemp);
                                objShipmentFreight.TemperatureType = "F";
                                objShipmentFreight.QuantityNweight = Convert.ToDecimal(modeldata.NoOfPallets);
                                objShipmentFreight.PcsType = modeldata.PcsTypeId;
                                objShipmentFreight.NoOfBox = Convert.ToInt32(modeldata.NoOfBox);
                                objShipmentFreight.Weight = modeldata.Weight;
                                objShipmentFreight.Unit = modeldata.Unit;
                                objShipmentFreight.Comments = modeldata.Comments;
                                UploadShipmentContext.tblShipmentFreightDetails.Add(objShipmentFreight);
                                result = UploadShipmentContext.SaveChanges() > 0;

                            }
                        }
                        else
                        {
                            tblShipment shipmentdata = new tblShipment();
                            shipmentdata.StatusId = 1;
                            shipmentdata.CustomerId = modeldata.CustomerId;
                            shipmentdata.VendorNconsignee = modeldata.ConsigneeNVendorName;
                            shipmentdata.SubStatusId = null;
                            shipmentdata.RequestedBy = null;
                            shipmentdata.Reason = null;
                            shipmentdata.ShipmentRefNo = GenrateShipmentRefNo();
                            shipmentdata.AirWayBill = modeldata.AWB;
                            shipmentdata.CustomerPO = modeldata.CustomerPO;
                            shipmentdata.OrderNo = modeldata.OrderNo;
                            shipmentdata.CreatedBy = Convert.ToInt32(modeldata.CreatedBy);
                            shipmentdata.CreatedDate = Configurations.TodayDateTime;
                            shipmentdata.UploadedFileName = modeldata.UploadedFileName;
                            shipmentdata.RequestedBy = modeldata.RequestedBy;
                            UploadShipmentContext.tblShipments.Add(shipmentdata);

                            tblShipmentRoutesStop objShipmentRouteStops = new tblShipmentRoutesStop();
                            objShipmentRouteStops.ShippingId = shipmentdata.ShipmentId;
                            objShipmentRouteStops.RouteNo = 1;
                            objShipmentRouteStops.PickupLocationId = modeldata.PickUpLocationId;
                            objShipmentRouteStops.PickDateTime = Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date));
                            objShipmentRouteStops.PickUpDateTimeTo = Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date));
                            objShipmentRouteStops.DeliveryLocationId = modeldata.DeliveryLocationId;
                            objShipmentRouteStops.DeliveryDateTime = modeldata.DeliveryDate == null ? Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date)) : Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.DeliveryDate));
                            objShipmentRouteStops.DeliveryDateTimeTo = modeldata.DeliveryDate == null ? Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.Date)) : Configurations.ConvertLocalToUTC(Convert.ToDateTime(modeldata.DeliveryDate));
                            objShipmentRouteStops.IsPickUpWaitingTimeRequired = IsWaitingTime;
                            objShipmentRouteStops.IsDeliveryWaitingTimeRequired = IsWaitingTime;
                            UploadShipmentContext.tblShipmentRoutesStops.Add(objShipmentRouteStops);

                            tblShipmentFreightDetail objShipmentFreight = new tblShipmentFreightDetail();
                            objShipmentFreight.ShipmentId = shipmentdata.ShipmentId;
                            objShipmentFreight.ShipmentRouteStopeId = objShipmentRouteStops.ShippingRoutesId;
                            objShipmentFreight.Commodity = modeldata.Commodity;
                            objShipmentFreight.FreightTypeId = modeldata.FreightTypeId;
                            objShipmentFreight.PricingMethodId = (modeldata.PricingMethodId == 0 ? null : modeldata.PricingMethodId);

                            objShipmentFreight.Hazardous = false;
                            objShipmentFreight.Temperature = Convert.ToDecimal(modeldata.ReqTemp);
                            objShipmentFreight.TemperatureType = "F";
                            objShipmentFreight.QuantityNweight = Convert.ToDecimal(modeldata.NoOfPallets);
                            objShipmentFreight.PcsType = modeldata.PcsTypeId;
                            objShipmentFreight.NoOfBox = Convert.ToInt32(modeldata.NoOfBox);
                            objShipmentFreight.Weight = modeldata.Weight;
                            objShipmentFreight.Unit = modeldata.Unit;
                            objShipmentFreight.Comments = modeldata.Comments;

                            if (modeldata.PartialBox > 0 || modeldata.PartialPallet > 0)
                            {
                                objShipmentFreight.IsPartialShipment = true;
                                objShipmentFreight.PartialBox = modeldata.PartialBox;
                                objShipmentFreight.PartialPallete = modeldata.PartialPallet;


                            }
                            else
                            {
                                objShipmentFreight.PartialBox = Convert.ToInt32(modeldata.NoOfBox);
                                objShipmentFreight.PartialPallete = Convert.ToInt32(modeldata.NoOfPallets);
                            }
                            UploadShipmentContext.tblShipmentFreightDetails.Add(objShipmentFreight);


                            tblShipmentStatusHistory shipmentStatusHistory = new tblShipmentStatusHistory();
                            shipmentStatusHistory.ShipmentId = shipmentdata.ShipmentId;
                            shipmentStatusHistory.StatusId = 1;

                            shipmentStatusHistory.CreatedBy = Convert.ToInt32(modeldata.CreatedBy);
                            shipmentStatusHistory.CreatedOn = Configurations.TodayDateTime;
                            UploadShipmentContext.tblShipmentStatusHistories.Add(shipmentStatusHistory);
                            result = UploadShipmentContext.SaveChanges() > 0;
                        }
                    }

                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Genrate shipment reference no 
        /// <summary>
        /// Genrate shipment reference no
        /// </summary>
        /// <returns></returns>

        public string GenrateShipmentRefNo()
        {
            try
            {
                string preLable = "SRN_";
                var lastShipmentId = UploadShipmentContext.tblShipments.OrderByDescending(x => x.ShipmentId).Select(x => x.ShipmentId).FirstOrDefault();
                return lastShipmentId > 0 ? (preLable + (1000 + lastShipmentId + 1).ToString()) : (preLable + (1000 + 1).ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region validate file name
        public bool CheckFileName(string fileName)
        {
            try
            {
                var shipmentlist = UploadShipmentContext.tblShipments.Where(x => x.UploadedFileName.ToLower().Trim().Contains(fileName.ToLower().Trim())).ToList();
                return shipmentlist.Count > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region validate customer count
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool ValidateContactInfo(int customerId)
        {

            int contactInfoCount = UploadShipmentContext.tblCustomerContacts.Where(x => x.CustomerId == customerId).Count();
            return contactInfoCount > 0;
        }
        #endregion


    }
}
