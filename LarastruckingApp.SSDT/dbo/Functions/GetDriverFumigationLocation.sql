CREATE FUNCTION [dbo].[GetDriverFumigationLocation]  
(@FumigationId INT  
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
           '|'+CONCAT(PA.CompanyName, ', ', PA.Address1, ' ', PA.City, ' ', PST.Name, ' ', PA.Zip)  
    FROM tblFumigationRouts FUMR  
         LEFT JOIN tblAddress PA ON FUMR.FumigationSite = PA.AddressId  
         LEFT JOIN tblState PST ON PA.State = PST.ID  
         WHERE FUMR.FumigationId = FUM.FumigationId     
    FOR XML PATH('')  
), 1, 1, '')  
    FROM tblFumigationRouts FUM
         LEFT JOIN tblAddress PA ON FUM.FumigationSite = PA.AddressId  
         LEFT JOIN tblState PST ON PA.State = PST.ID  
    WHERE FUM.FumigationId =  @FumigationId 
    --GROUP BY SPRT.ShippingId  
);