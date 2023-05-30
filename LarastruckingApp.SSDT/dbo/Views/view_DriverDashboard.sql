﻿CREATE VIEW [dbo].[view_DriverDashboard]
as
  WITH CTE_CommaSepartedVarietal(ShipmentId,QuantityNMethod)          
             AS (SELECT DISTINCT          
                        (ShipmentId),           
                        STUFF((          
                     SELECT  ',' + CONVERT(VARCHAR(MAX), concat(replace(cast(sum(tsfd.QuantityNweight) as varchar), '.00', '')  , ' ', (select PricingMethodExt  from tblPricingMethod where PricingMethodId=tsfd.PricingMethodId)))    
                     FROM tblShipmentFreightDetail tsfd          
                     Left JOIN tblPricingMethod tpm ON tpm.PricingMethodId = tsfd.PricingMethodId        
          
                     WHERE tsfd.ShipmentId = tsfdd.ShipmentId and tsfd.QuantityNweight is not null AND tsfd.IsDeleted = 0 group by tsfd.PricingMethodId,tsfd.ShipmentId    
      FOR XML PATH('')          
                 ), 1, 1, '') AS QuantityNMethod          
                 FROM tblShipmentFreightDetail tsfdd),    
         
               
             cte_getPickDate(ShippingId,PickDateTime)          
             AS (SELECT DISTINCT (TSRS.ShippingId),TSRS.MINDate          
             FROM tblShipmentRoutesStop SRS          
             INNER JOIN          
                 (          
                 SELECT MIN(PickDateTime) AS MINDate,           
                     ShippingId FROM tblShipmentRoutesStop ISRS          
                     WHERE IsDeleted = 0          
                     GROUP BY ShippingId          
                 ) AS TSRS ON SRS.ShippingId = TSRS.ShippingId          
                              AND SRS.PickDateTime = TSRS.MINDate),     
                  
             CTE_EqipmentCommaSeperated(ShipmentId,           
                                        DriverEquipment)          
             AS (SELECT DISTINCT          
                        (ShipmentId),           
                        STUFF(          
                 (          
                     SELECT ', ' + TE.EquipmentNo          
                     FROM tblShipmentEquipmentNdriver tblSED          
                          INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = tblSED.EquipmentId          
                     WHERE tblSED.ShipmentId = TS.ShipmentId FOR XML PATH('')          
                 ), 1, 1, '') AS DriverEquipment          
                 FROM tblShipment TS),         
              
             cte_getDriver(DriverId,UserId,ShipmentId)AS (SELECT DISTINCT (TSEND.DriverId), TD.UserId,ShipmentId          
                 FROM tblShipmentEquipmentNdriver TSEND          
                 INNER JOIN tblDriver TD ON TSEND.DriverId = TD.DriverID)        
             
                    SELECT TS.ShipmentId,           
                    cted.DriverId,           
                    ISNULL(TC.PreTripCheckupId, 0) PreTripCheckupId,           
                    CTEP.PickDateTime,           
                    CTEE.DriverEquipment,           
                    TS.ShipmentRefNo,           
                    CTEC.QuantityNMethod,           
                    TS.StatusId,           
                    TSS.StatusName,          
                    CASE          
                        WHEN dbo.ufnGetPreTripStatus(TS.ShipmentId, TC.UserId) IS NULL          
                        THEN 'PENDING'          
                        ELSE dbo.ufnGetPreTripStatus(TS.ShipmentId, TC.UserId)          
                    END PreTripStatus          
                    FROM [dbo].[tblShipment] TS          
                  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId          
                  INNER JOIN cte_getDriver cted ON TS.ShipmentId = cted.ShipmentId          
                  INNER JOIN CTE_CommaSepartedVarietal CTEC ON CTEC.ShipmentId = TS.ShipmentId          
                  INNER JOIN cte_getPickDate CTEP ON TS.ShipmentId = CTEP.ShippingId          
                  INNER JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId          
                  LEFT JOIN [dbo].[tblPreTripCheckUp] TC ON TC.ShipmentId = TS.ShipmentId AND TC.UserId = cted.UserId ;