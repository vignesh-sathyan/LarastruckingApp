   -- Select * from View_DriverShipmentDetail
	
	 CREATE View View_DriverShipmentDetail as
	 
	  WITH CTE_EqipmentCommaSeperated(ShipmentId, 
                                        DriverEquipment)
             AS (SELECT DISTINCT
                        (ShipmentId), 
                        STUFF(
                 (
                     SELECT ', ' + TE.EquipmentNo
                     FROM tblShipmentEquipmentNdriver tblSED
                          LEFT JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = tblSED.EquipmentId
                     WHERE tblSED.ShipmentId = TS.ShipmentId FOR XML PATH('')
                 ), 1, 1, '') AS DriverEquipment
                 FROM tblShipment TS),
             cte_getDriver(DriverId, 
                           UserId, 
                           ShipmentId)
             AS (SELECT DISTINCT
                        (TSEND.DriverId), 
                        TD.UserId, 
                        ShipmentId
                 FROM tblShipmentEquipmentNdriver TSEND
                      LEFT JOIN tblDriver TD ON TSEND.DriverId = TD.DriverID)
             SELECT TS.ShipmentId AS Id, 
             (
                 SELECT CONCAT(FSQ.PALLET, FSQ.BOX, FSQ.LB, FSQ.KG)
                 FROM fun_GetShipmentQuantity(TS.ShipmentId) FSQ
             ) AS QuantityNMethod, 
                    TS.AirWayBill, 
                    TS.CustomerPO, 
                    TS.OrderNo, 
                    TS.DriverInstruction, 
             (
                 SELECT PickUpAddress
                 FROM [dbo].[GetShipmentPickupLocation](TS.ShipmentId)
             ) AS PickupLocation, 
             (
                 SELECT DeliveryAddress
                 FROM [dbo].[GetShipmentDeliveryLocation](TS.ShipmentId)
             ) AS DeliveryLocation, 
                    TSS.StatusAbbreviation AS StatusName, 
                    TSS.FontColor, 
                    TSS.Colour,    
             --CTEP.PickDateTime,    
                    CTEE.DriverEquipment,
                    CASE
                        WHEN dbo.ufnGetPreTripStatus(TS.ShipmentId, TC.UserId) IS NULL
                        THEN 'PENDING'
                        ELSE dbo.ufnGetPreTripStatus(TS.ShipmentId, TC.UserId)
                    END PreTripStatus, 
                    TS.CreatedDate CreatedOn, 
                    'Shipment' AS Types,    
        cted.UserId      
        --ISNULL(TC.PreTripCheckupId, 0) PreTripCheckupId,                                      
        --TS.StatusId                                                                                   

     
             FROM [dbo].[tblShipment] TS
                  LEFT JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId
                  LEFT JOIN cte_getDriver cted ON TS.ShipmentId = cted.ShipmentId  
                  -- LEFT JOIN cte_getPickDate CTEP ON TS.ShipmentId = CTEP.ShippingId  
                  LEFT JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId
                  LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TS.ShipmentId
                                                            AND TC.UserId = cted.UserId
             WHERE 
                    TS.IsDeleted = 0
                   AND (TS.StatusId != 1
                        AND TS.StatusId != 11
                        AND TS.StatusId != 8);