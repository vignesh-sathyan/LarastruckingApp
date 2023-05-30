  
--drop FUNCTION  [GetShipmentDeliveryLocation]  
CREATE FUNCTION [dbo].[GetShipmentDeliveryLocation]  
(@ShippingId INT  
)  
RETURNS TABLE  
AS  
     RETURN  
(  
    SELECT TOP 1 SPRT1.ShippingId,  
           SPRT1.ShippingRoutesId,  
           DeliveryAddress = STUFF(  
(  
    SELECT   
           '|'+CONCAT(PA.CompanyName, ', ', PA.Address1, ' ', PA.City, ' ', PST.Name, ' ', PA.Zip)  
    FROM tblShipmentRoutesStop SPRT  
         LEFT JOIN tblAddress PA ON SPRT.DeliveryLocationId = PA.AddressId  
         LEFT JOIN tblState PST ON PA.State = PST.ID  
   -- WHERE SPRT.ShippingId = 223    
      WHERE SPRT.ShippingId = SPRT1.ShippingId     
    FOR XML PATH('')  
), 1, 1, '')  
    FROM tblShipmentRoutesStop SPRT1  
         LEFT JOIN tblAddress PA ON SPRT1.DeliveryLocationId = PA.AddressId  
         LEFT JOIN tblState PST ON PA.State = PST.ID  
    WHERE SPRT1.ShippingId = @ShippingId  
    --GROUP BY SPRT.ShippingId  
);  
  
  
  
--SELECT ADRESS AS PickupLocation FROM [dbo].[GetShipmentPickupLocation](223)