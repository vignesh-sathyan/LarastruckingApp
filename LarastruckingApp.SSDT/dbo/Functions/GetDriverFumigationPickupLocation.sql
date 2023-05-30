    
--drop FUNCTION  GetShipmentPickupLocation    
CREATE FUNCTION [dbo].[GetDriverFumigationPickupLocation]    
(@FumigationId INT,
@DriverId INT   
)    
RETURNS TABLE    
AS    
     RETURN    
(    
    SELECT TOP 1 FUM.FumigationId,    
           --FUM.FumigationRoutsId,    
           PickUpAddress = STUFF(    
(    
    SELECT     
           '|'+( case when FED.IsPickUp=1 THEN (CONCAT(PA.CompanyName, ', ', PA.Address1, ' ', PA.City, ' ', PST.Name, ' ', PA.Zip)) 
		   ELSE (CONCAT(FA.CompanyName, ', ', FA.Address1, ' ', FA.City, ' ', FS.Name, ' ', FA.Zip)) END) 
    FROM tblFumigationRouts FUMR    
         LEFT JOIN tblAddress PA ON FUMR.PickUpLocation = PA.AddressId    
         LEFT JOIN tblState PST ON PA.State = PST.ID   
		 LEFT JOIN tblAddress FA ON FUMR.FumigationSite=FA.AddressId
		 LEFT JOIN tblState FS ON FA.State=FS.ID
		 LEFT JOIN tblFumigationEquipmentNDriver  FED ON FUMR.FumigationRoutsId=FED.FumigationRoutsId 
         WHERE FUMR.FumigationId =  FUM.FumigationId  AND  FED.DriverId=@DriverId
    FOR XML PATH('')    
), 1, 1, '')    
    FROM tblFumigationRouts FUM  
         --LEFT JOIN tblAddress PA ON FUM.PickUpLocation = PA.AddressId    
         --LEFT JOIN tblState PST ON PA.State = PST.ID    
    WHERE FUM.FumigationId = @FumigationId   
    --GROUP BY SPRT.ShippingId    
);