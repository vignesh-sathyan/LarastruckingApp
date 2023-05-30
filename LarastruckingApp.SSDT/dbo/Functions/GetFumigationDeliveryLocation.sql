
--drop FUNCTION  [GetFumigationDeliveryLocation]
CREATE FUNCTION [dbo].[GetFumigationDeliveryLocation]
(
@FumigationRoutsId INT
)
RETURNS TABLE
AS
     RETURN

	
	select top 1  FUR.FumigationId, CONCAT(PA.CompanyName,', ',PA.Address1,' ',PA.City,' ',PST.Name,' ',PA.Zip ) as FumigationPickUpLocation,FUR.ReleaseDate,CONCAT(FS.CompanyName,',',FS.Address1,' ',FS.City,' ',FST.Name,' ',FS.Zip) AS FumigationSite,CONCAT(DA.CompanyName,',',DA.Address1,' ',DA.City,' ',DST.Name,' ',DA.Zip) AS FumigationDeliveryLocation, FUR.PickUpArrival,FUR.DeliveryArrival,FUR.FumigationArrival,CONCAT(FUR.AirWayBill,' ',FUR.CustomerPO,' ',FUR.ContainerNo) AS AWB_CP_CN ,CONCAT(FUR.Temperature,' ',FUR.TemperatureType) as Temperature from tblFumigationRouts FUR
Left join tblAddress PA ON FUR.PickUpLocation=PA.AddressId
Left join tblState PST ON PA.State =PST.ID
Left join tblAddress FS ON FUR.FumigationSite=FS.AddressId
Left join tblState FST ON FS.State =FST.ID
Left join tblAddress DA ON FUR.DeliveryLocation=DA.AddressId
Left join tblState DST ON DA.State =DST.ID
WHERE FUR.FumigationRoutsId=@FumigationRoutsId


--(
--    SELECT TOP 1 FURT1.FumigationId,
--           FURT1.FumigationRoutsId,
--          CONCAT(PA.CompanyName,', ',PA.Address1,' ',PA.City,' ',PST.Name,' ',PA.Zip ) as FumigationDeliveryLocation
----		  STUFF(
----(
----    SELECT DISTINCT PA.CompanyName, ', ', PA.Address1, ' ', PA.City, ' ', PST.Name, ' ', PA.Zip
----    FROM tblFumigationRouts FURT
----         LEFT JOIN tblAddress PA ON FURT.DeliveryLocation = PA.AddressId
----         LEFT JOIN tblState PST ON PA.State = PST.ID
----   -- WHERE SPRT.ShippingId = 223  
----      WHERE FURT.FumigationId = FURT1.FumigationId   
----    FOR XML PATH('')
----), 1, 1, '')
--    FROM tblFumigationRouts FURT1
--         LEFT JOIN tblAddress PA ON FURT1.DeliveryLocation = PA.AddressId
--         LEFT JOIN tblState PST ON PA.State = PST.ID
--    WHERE FURT1.FumigationRoutsId =  @FumigationRoutsId
--    --GROUP BY SPRT.ShippingId
--);



--SELECT ADRESS AS PickupLocation FROM [dbo].[GetShipmentPickupLocation](223)