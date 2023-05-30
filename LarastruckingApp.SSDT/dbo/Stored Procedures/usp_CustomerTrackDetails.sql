        
-- [usp_CustomerTrackDetails] 24        
 CREATE PROCEDURE [dbo].[usp_CustomerTrackDetails]                            
(                                
 @ShipmentId INT                                            
)                            
AS                            
BEGIN          
SET NOCOUNT ON;                          
                           
                                      
   SELECT  distinct                                       
   TS.ShipmentId,                                             
   TSRS.ShippingRoutesId,                                              
   TS.ShipmentRefNo,                                              
   TS.AirWayBill,                                              
   TS.CustomerPO,                                              
   TS.OrderNo,                                              
   TS.CustomerRef,                                              
   TS.ContainerNo,                                              
   TS.PurchaseDoc,      
   CONCAT(A1.CompanyName,', ', A1.Address1,' ', A1.City,' ', S1.Name,' ',A1.Zip )as PickUpLocation ,                                              
   CONCAT(A2.CompanyName,', ',A2.Address1,' ', A2.City,' ', S1.Name,' ',A2.Zip ) DeliveryAddress ,                                     
   TSRS.PickDateTime as PickUpArrivalDate,                                              
   TSRS.DeliveryDateTime as DeliveryArrive                 
                                    
  FROM [dbo].[tblShipment] TS                                             
  INNER JOIN [dbo].[tblShipmentRoutesStop] TSRS ON TSRS.ShippingId = TS.ShipmentId                  
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TSRS.PickupLocationId                                              
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                              
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                            
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TSRS.DeliveryLocationId                                     
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                              
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country                 
                                         
  WHERE                                               
  TS.ShipmentId = @ShipmentId                                       
                          
                          
END