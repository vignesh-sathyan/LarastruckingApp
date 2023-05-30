CREATE function [dbo].[shipmentStatus]
(
   @shipmetnID int
)
returns table
AS RETURN
WITH  GetStatus(StatusId,StatusName,ShipmentId) AS (
select top 1 SH.StatusId,SS.StatusName,SH.ShipmentId from tblShipmentStatusHistory SH
inner join tblShipmentStatus SS on SH.StatusId = SS.StatusId
WHERE SH.ShipmentId=@shipmetnID
order by SH.CreatedOn desc
) select StatusId,StatusName,ShipmentId FROM GetStatus