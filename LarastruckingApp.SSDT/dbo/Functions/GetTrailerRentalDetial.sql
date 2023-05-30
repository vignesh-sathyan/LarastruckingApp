CREATE function [dbo].[GetTrailerRentalDetial]
(
   @TrailerRentalId int
)
returns table
AS RETURN
select top 1  TRD.TrailerRentalId,CONCAT(PA.CompanyName,',',PA.Address1,' ',PA.City,' ',PST.Name,' ',PA.Zip ) as PickUpLocation,CONCAT(DA.CompanyName,',',DA.Address1,' ',DA.City,' ',DST.Name,' ',DA.Zip) AS DeliveryLocation,TRD.StartDate,TRD.EndDate from tblTrailerRentalDetail TRD
Left join tblAddress PA ON TRD.PickUpLocationId=PA.AddressId
Left join tblState PST ON PA.State =PST.ID
Left join tblAddress DA ON TRD.DeliveryLocationId=DA.AddressId
Left join tblState DST ON DA.State =DST.ID
WHERE TRD.TrailerRentalId=@TrailerRentalId