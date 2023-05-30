
 create VIEW view_ShipmentQuantity AS

			 (
    SELECT DISTINCT
           (SHP.ShipmentId), 
           STUFF(
    (
        SELECT '|' + (CASE
                          WHEN
        (
            SELECT COUNT(TSFD.ShipmentBaseFreightDetailId)
            FROM view_tblShipmentFreightDetail TSFD
            WHERE TSFD.IsPartialShipment = 1
                  AND TSFD.ShipmentId = SHP.ShipmentId
                  AND TSFD.ShipmentRouteStopeId = SRS.ShippingRoutesId
                 -- AND TSFD.IsDeleted = 0
        ) = 0
                          THEN CONCAT(
        (
            SELECT CASE
                       WHEN SUM(T1.QuantityNweight) > 0
                       THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS NVARCHAR), '.00', '') + ' PLTS, '
                       ELSE ''
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                 -- AND T1.IsDeleted = 0
        ), ' ',
        (
            SELECT CASE
                       WHEN SUM(T1.NoOfBox) > 0
                       THEN REPLACE(CAST(SUM(T1.NoOfBox) AS NVARCHAR), '.00', '') + ' BXS, '
                       ELSE ''
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                  --AND T1.IsDeleted = 0
        ), ' ',
        (
            SELECT CASE
                       WHEN SUM(T1.Weight) > 0
                       THEN REPLACE(CAST(SUM(T1.Weight) AS NVARCHAR), '.00', '') + ' LBS, '
                       ELSE ''
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                  AND (T1.Unit = 'LBS'
                       OR T1.Unit = 'LB')
                 -- AND T1.IsDeleted = 0
        ), ' ',
        (
            SELECT CASE
                       WHEN SUM(T1.Weight) > 0
                       THEN REPLACE(CAST(SUM(T1.Weight) AS NVARCHAR), '.00', '') + ' KG '
                       ELSE ''
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                  AND T1.Unit = 'KG'
                  --AND T1.IsDeleted = 0
        ))
                          ELSE CONCAT(
        (
            SELECT CASE
                       WHEN (SUM(T1.PartialPallete)-SUM(T1.QuantityNweight)) >= 0 AND (SUM(T1.PartialPallete)-SUM(T1.QuantityNweight)) != 0
                       THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS NVARCHAR), '.00', '') + '/' + REPLACE(CAST(SUM(T1.PartialPallete) AS NVARCHAR), '.00', '') + ' PLTS, '
                       ELSE CASE
                                WHEN SUM(T1.QuantityNweight) > 0
                                THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS NVARCHAR), '.00', '') + ' PLTS, '
                                ELSE ''
                            END
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                  --AND T1.IsDeleted = 0
        ), ' ',
		 (
            SELECT CASE
                       WHEN (SUM(T1.PartialBox)- SUM(T1.NoOfBox))>= 0 AND (SUM(T1.PartialBox)- SUM(T1.NoOfBox))!= 0
                       THEN REPLACE(CAST(SUM(T1.NoOfBox) AS NVARCHAR), '.00', '') + '/' + REPLACE(CAST(SUM(T1.PartialBox) AS NVARCHAR), '.00', '') + ' BXS, '
                       ELSE CASE
                                WHEN SUM(T1.NoOfBox) > 0
                                THEN REPLACE(CAST(SUM(T1.NoOfBox) AS NVARCHAR), '.00', '') + ' BXS, '
                                ELSE ''
                            END
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                 -- AND T1.IsDeleted = 0
        ), ' ',
     
        (
            SELECT CASE
                       WHEN SUM(T1.Weight) > 0
                       THEN REPLACE(CAST(SUM(T1.Weight) AS NVARCHAR), '.00', '') + ' LBS, '
                       ELSE ''
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                  AND (T1.Unit = 'LBS'
                       OR T1.Unit = 'LB')
                 -- AND T1.IsDeleted = 0
        ), ' ',
        (
            SELECT CASE
                       WHEN SUM(T1.Weight) > 0
                       THEN REPLACE(CAST(SUM(T1.Weight) AS NVARCHAR), '.00', '') + ' KG '
                       ELSE ''
                   END
            FROM view_tblShipmentFreightDetail T1
            WHERE T1.ShipmentId = SHP.ShipmentId
                  AND T1.ShipmentRouteStopeId = SRS.ShippingRoutesId
                  AND T1.Unit = 'KG'
                  --AND T1.IsDeleted = 0
        ))
                      END)
        FROM tblShipmentRoutesStop AS SRS WITH(NOLOCK)
             LEFT JOIN view_tblShipmentFreightDetail SFD ON SRS.ShippingRoutesId = SFD.ShipmentRouteStopeId
        WHERE SRS.ShippingId = SHP.ShipmentId
              AND SHP.IsDeleted = 0 
			  --and SFD.IsDeleted = 0
        GROUP BY SRS.ShippingRoutesId, 
                 SRS.ShippingId FOR XML PATH('')
    ), 1, 1, '') AS Quantity
    FROM tblShipment SHP WITH(NOLOCK)
    WHERE SHP.IsDeleted = 0
        -- AND SHP.ShipmentId = 1301
)