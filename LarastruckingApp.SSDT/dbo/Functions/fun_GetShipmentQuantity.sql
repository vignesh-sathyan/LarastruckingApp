CREATE FUNCTION fun_GetShipmentQuantity
				 (
				 @ShipmentId int
				 )
				 returns table as 
				 return(
				SELECT TOP 1 TSFD.ShipmentId, 
				(select CASE WHEN SUM(T1.QuantityNweight)>0 THEN REPLACE(CAST(SUM(T1.QuantityNweight) AS nvarchar),'.00','')+' PALLET, ' ELSE '' END from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=TSFD.ShipmentId AND T1.IsDeleted=0) AS PALLET,
				(select CASE WHEN SUM(T1.NoOfBox)>0 THEN REPLACE(CAST(SUM(T1.NoOfBox) AS nvarchar),'.00','')+' BOX, ' ELSE '' END from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=TSFD.ShipmentId AND T1.IsDeleted=0) AS BOX,
				(select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' LB, ' ELSE '' END  from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=TSFD.ShipmentId AND T1.Unit='LB' AND T1.IsDeleted=0) AS LB,
				(select CASE WHEN SUM(T1.Weight)>0 THEN REPLACE(CAST(SUM(T1.Weight) AS nvarchar),'.00','')+' KG' ELSE '' END from tblShipmentFreightDetail T1 WHERE T1.ShipmentId=TSFD.ShipmentId AND T1.Unit='KG' AND T1.IsDeleted=0) AS KG
				--, SUM(TSFD.QuantityNweight) AS PALLET,SUM(TSFD.NoOfBox) AS BOX, SUM(TSFD.Weight) AS WEIGHTS,TSFD.Unit INTO #TEMP 
				FROM tblShipmentFreightDetail TSFD
				WHERE TSFD.ShipmentId= @ShipmentId AND TSFD.IsDeleted=0--@ShipmentId
				--GROUP BY TSFD.ShipmentId,TSFD.Unit ;

				--SELECT  DISTINCT(T.ShipmentId), (select sum(PALLET) from #TEMP AS T1 WHERE T1.ShipmentId=T.ShipmentId ) AS PALLET,(select sum(BOX) from #TEMP AS T1 WHERE T1.ShipmentId=T.ShipmentId ) AS BOX,(select sum(WEIGHTS) from #TEMP AS T1 WHERE T1.ShipmentId=T.ShipmentId AND T1.Unit='KG' ) AS KG,(select sum(WEIGHTS) from #TEMP AS T1 WHERE T1.ShipmentId=T.ShipmentId AND T1.Unit='LB' ) AS LB   FROM #TEMP T;

				--DROP TABLE #TEMP;
				);