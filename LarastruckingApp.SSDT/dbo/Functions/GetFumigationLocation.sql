CREATE function [dbo].[GetFumigationLocation]    
(    
   @FumigationId int    
)    
returns table    
AS RETURN    
select TOP 1   FUR.FumigationId, CONCAT(PA.CompanyName,',',PA.Address1,' ',PA.City,' ',PST.Name,' ',PA.Zip ) as PickUpLocation,FUR.ReleaseDate,  
CONCAT(FS.CompanyName,',',FS.Address1,' ',FS.City,' ',FST.Name,' ',FS.Zip) AS FumigationSite,  
CONCAT(DA.CompanyName,',',DA.Address1,' ',DA.City,' ',DST.Name,' ',DA.Zip) AS DeliveryLocation, FUR.PickUpArrival,FUR.DeliveryArrival,FUR.FumigationArrival,  
CONCAT(FUR.AirWayBill,' ',FUR.CustomerPO,' ',FUR.ContainerNo) AS AWB_CP_CN ,  
CONCAT(FUR.Temperature,' ',FUR.TemperatureType) as Temperature from tblFumigationRouts FUR    
Left join tblAddress PA ON FUR.PickUpLocation=PA.AddressId    
Left join tblState PST ON PA.State =PST.ID    
Left join tblAddress FS ON FUR.FumigationSite=FS.AddressId    
Left join tblState FST ON FS.State =FST.ID    
Left join tblAddress DA ON FUR.DeliveryLocation=DA.AddressId    
Left join tblState DST ON DA.State =DST.ID    
WHERE FUR.FumigationId=@FumigationId