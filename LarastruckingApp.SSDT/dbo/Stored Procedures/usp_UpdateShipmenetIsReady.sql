CREATE PROCEDURE usp_UpdateShipmenetIsReady 
(
@ShipmentId INT,
@IsReady BIT
)
AS
BEGIN
UPDATE tblShipment SET IsReady=@IsReady WHERE ShipmentId=@ShipmentId
END