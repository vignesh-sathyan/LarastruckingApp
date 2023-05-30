--Declare @TotalCount int                                              
--exec [usp_CustomerOldShipmentDashoard ] 12,'','StatusName','ASC',0,100,@TotalCount out                                        
--print @TotalCount    
CREATE PROC [dbo].[usp_CustomerOldShipmentDashoard]                         
(                        
@UserId INT,                        
@SearchTerm VARCHAR(50),                        
@SortColumn VARCHAR(50),                        
@SortOrder VARCHAR(50),                        
@PageNumber INT,                        
@PageSize INT,                        
@TotalCount int out                        
)                        
AS                        
BEGIN                        
SET NOCOUNT ON;                        
                         
 DECLARE @StartRow INT                        
 DECLARE @EndRow INT                        
                         
 -- calculate the starting and ending of records                        
 SET @SortColumn = LOWER(ISNULL(@SortColumn, ''))                        
 SET @SortOrder = LOWER(ISNULL(@SortOrder, ''))                          
                        
                     
                        
 ;With cte_getStatus as                        
(                        
select SS.StatusName, SH.StatusId, SH.ShipmentId from tblShipmentStatusHistory SH                        
inner join (select MAX(CreatedOn) as MaxDate,ShipmentId from tblShipmentStatusHistory group by ShipmentId) ssh on SH.ShipmentId=ssh.ShipmentId and SH.CreatedOn = ssh.MaxDate                        
inner join tblShipmentStatus SS on SH.StatusId = SS.StatusId                        
),                       
                      
 cte_getPickDate(ShippingId,PickDateTime) as                      
(                      
select distinct(TSRS.ShippingId), TSRS.MINDate from tblShipmentRoutesStop  SRS                      
inner join (select MIN(PickDateTime) as MINDate,ShippingId from tblShipmentRoutesStop ISRS WHERE IsDeleted=0 group by ShippingId) AS TSRS on SRS.ShippingId=TSRS.ShippingId and SRS.PickDateTime = TSRS.MINDate                      
                      
),                      
 cte_getDeliveryDate(ShippingId,DeliveryDateTime) as                      
(                      
select distinct(TRSD.ShippingId), SRS.DeliveryDateTime from tblShipmentRoutesStop  SRS                      
inner join (select MAX(DeliveryDateTime) as MaxDate,ShippingId from tblShipmentRoutesStop ISRS WHERE IsDeleted=0 group by ShippingId) AS TRSD on SRS.ShippingId=TRSD.ShippingId and SRS.DeliveryDateTime = TRSD.MaxDate                      
),                      
    
	    CTE_EqipmentCommaSeperated(ShipmentId,                                 
                                        CostumerEquipment)                                
             AS (SELECT DISTINCT                                
                        (ShipmentId),                                 
                        STUFF(                                
                 (             
                     SELECT ', ' + TE.EquipmentNo                                
                    FROM tblShipmentEquipmentNdriver tblSED                                
                          INNER JOIN [dbo].[tblEquipmentDetail] TE ON TE.EDID = tblSED.EquipmentId                                
                     WHERE tblSED.ShipmentId = TS.ShipmentId FOR XML PATH('')                                
                 ), 1, 1, '') AS CostumerEquipment                         
                 FROM tblShipment TS),             
                
cte_getDriver(ShipmentId,DriverName)  AS                    
(                      
     SELECT ShipmentId,                      
        STUFF(                      
            (                      
         SELECT distinct ', ' + CONVERT(varchar(max),concat(TD.FirstName,' ',TD.LastName))                      
             FROM tblShipmentEquipmentNdriver TSED                      
          INNER JOIN tblDriver TD ON TSED.DriverId=TD.DriverID                                  
          WHERE TSED.ShipmentId=TS.ShipmentId                      
            FOR XML PATH('')                      
             ), 1, 1, '')                       
          AS  DriverName                  
           FROM tblShipment TS)                           
                          
                        
  SELECT                    
                           
  TS.ShipmentId,                                      
                                             
    TS.AirWayBill,                                                                   
    CTEP.PickDateTime,                                          
    CTED.DeliveryDateTime,
	 CTEE.CostumerEquipment,                                                          
      TS.ShipmentRefNo,                           
     CTES.StatusName AS StatusName ,                    
     CTEDR.DriverName ,                   
      TS.CreatedDate As CreatedOn               
     INTO #TEMPDATA                                                                      
    FROM [dbo].[tblShipment] TS                                                  
    INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerId =TS.CustomerID      
  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId                                                                                          
   LEFT OUTER JOIN [dbo].tblShipmentEquipmentNdriver TSEND ON TSEND.ShipmentId = TS.ShipmentId                            
   INNER JOIN cte_getStatus  CTES ON TS.ShipmentId =CTES.ShipmentId                      
   INNER JOIN cte_getPickDate CTEP ON TS.ShipmentId =CTEP.ShippingId                      
   INNER JOIN cte_getDeliveryDate CTED ON TS.ShipmentId =CTED.ShippingId                     
   INNER JOIN cte_getDriver CTEDR ON TS.ShipmentId =CTEDR.ShipmentId  
   INNER JOIN CTE_EqipmentCommaSeperated CTEE ON CTEE.ShipmentId = TS.ShipmentId  
 WHERE TS.IsDeleted=0 AND TCR.UserId = @UserId   AND TSS.StatusId != 1  AND TSS.StatusId != 2   
 AND TSS.StatusId != 3  AND TSS.StatusId != 4 AND TSS.StatusId != 5  AND TSS.StatusId != 6 AND TSS.StatusId != 9    
           
 ---------------------------------------------------------------------------------------------------------------------------------------------------          
          
 ;With cte_getStatus as                        
(                        
select SS.StatusName, SH.StatusId, SH.FumigationId from tblFumigationStatusHistory SH                        
inner join (select MAX(CreatedOn) as MaxDate,FumigationId from tblFumigationStatusHistory group by FumigationId) ssh on SH.FumigationId=ssh.FumigationId and SH.CreatedOn = ssh.MaxDate                        
inner join tblShipmentStatus SS on SH.StatusId = SS.StatusId                        
),                       
                      
 cte_getPickDate(FumigationId,PickDateTime) as                      
(                      
select distinct(TSRS.FumigationId), TSRS.MINDate from tblFumigationRouts  SRS                      
inner join (select MIN(PickUpArrival) as MINDate,FumigationId from tblFumigationRouts ISRS WHERE IsDeleted=0 group by FumigationId) AS TSRS on SRS.FumigationId=TSRS.FumigationId and SRS.PickUpArrival = TSRS.MINDate                      
                      
),                      
 cte_getDeliveryDate(FumigationId,DeliveryDateTime) as                      
(                      
select distinct(TRSD.FumigationId), SRS.DeliveryArrival from tblFumigationRouts  SRS                      
inner join (select MAX(DeliveryArrival) as MaxDate,FumigationId from tblFumigationRouts ISRS WHERE IsDeleted=0 group by FumigationId) AS TRSD on SRS.FumigationId=TRSD.FumigationId and SRS.DeliveryArrival = TRSD.MaxDate                      
),                      
          
		   CTE_PickUpEquipment(FumigationIds,PickUpEquipment) AS (SELECT DISTINCT FumigationId, STUFF((                    
         SELECT ',' + ted.EquipmentNo          
                 
   FROM  tblEquipmentDetail  ted                     
   JOIN tblFumigationEquipmentNDriver FEM ON ted.EDID=FEM.EquipmentId AND FEM.FumigationId=FUM.FumigationId            
  -- JOIN tblDriver tu ON  tu.DriverID = FEM.DriverId          
        -- WHERE tu.UserId=@UserId          
     GROUP BY ted.EquipmentNo                    
            FOR XML PATH('')                    
        ), 1, 1, '') AS PickUpEquipment                    
        FROM tblFumigationEquipmentNDriver FUM) ,
		  
		            
cte_getDriver(FumigationId,DriverName)  AS                    
(                      
     SELECT FumigationId,                      
        STUFF(                      
            (                      
         SELECT distinct ', ' + CONVERT(varchar(max),concat(TD.FirstName,' ',TD.LastName))                      
             FROM tblFumigationEquipmentNDriver TSED                      
          INNER JOIN tblDriver TD ON TSED.DriverId=TD.DriverID                                  
          WHERE TSED.FumigationId=TS.FumigationId                      
            FOR XML PATH('')                      
             ), 1, 1, '')                       
          AS  DriverName                  
           FROM tblFumigation TS)              
            
            
            
     SELECT                    
    TS.FumigationId,                                      
    TFS.AirWayBill,                                                                   
    CTEP.PickDateTime,                      
    CTED.DeliveryDateTime,                                                        
     TS.ShipmentRefNo,                                                                  
     CTES.StatusName AS StatusName ,                     
     CTEDR.DriverName ,
	 ISNULL(PE.PickUpEquipment, '') AS CostumerEquipment,       
     TS.CreatedOn                   
                      
     INTO #FUMIGATIONTEMPDATA         
    FROM [dbo].[tblFumigation] TS              
 INNER JOIN [dbo].[tblFumigationRouts]  TFS ON TFS.FumigationId = TS.FumigationId                                             
    INNER JOIN [dbo].[tblCustomerRegistration] TCR ON TCR.CustomerId =TS.CustomerID     
  INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TS.StatusId                                                                                            
    INNER JOIN [dbo].tblFumigationEquipmentNDriver TSEND ON TSEND.FumigationId = TS.FumigationId                            
   INNER JOIN cte_getStatus  CTES ON TS.FumigationId =CTES.FumigationId                      
   INNER JOIN cte_getPickDate CTEP ON TS.FumigationId =CTEP.FumigationId                      
   INNER JOIN cte_getDeliveryDate CTED ON TS.FumigationId =CTED.FumigationId                     
   INNER JOIN cte_getDriver CTEDR ON TS.FumigationId =CTEDR.FumigationId  
   LEFT JOIN CTE_PickUpEquipment PE ON TS.FumigationId = PE.FumigationIds                    
   WHERE TS.IsDeleted=0 AND TCR.UserId = @UserId   AND TSS.StatusId != 1  AND TSS.StatusId != 2   
 AND TSS.StatusId != 3  AND TSS.StatusId != 4 AND TSS.StatusId != 5  AND TSS.StatusId != 6 AND TSS.StatusId != 9           
          
    select *,[TotalCount] = COUNT(*) OVER() into #tempAll from (    
                     
      Select FumigationId as Id,ShipmentRefNo,StatusName,PickDateTime,DeliveryDateTime , AirWayBill ,DriverName ,CreatedOn,CostumerEquipment, 'Fumigation' as Types from #FUMIGATIONTEMPDATA                    
      UNION ALL                     
      SELECT ShipmentId as Id,ShipmentRefNo,StatusName,PickDateTime,DeliveryDateTime , AirWayBill ,DriverName, CreatedOn,CostumerEquipment,'Shipment' as Types from #TEMPDATA)a                     
                          
  DROP TABLE #TEMPDATA                     
       DROP TABLE #FUMIGATIONTEMPDATA                   
       select * from #tempAll where            
          
                     
  ( (ISNULL(@SearchTerm, '') = '' OR StatusName LIKE '%' + @SearchTerm + '%')                        
  OR (ISNULL(@SearchTerm, '') = '' OR DriverName LIKE '%' + @SearchTerm + '%')                        
  OR (ISNULL(@SearchTerm, '') = '' OR AirWayBill LIKE '%' + @SearchTerm + '%')                        
  OR (ISNULL(@SearchTerm, '') = '' OR PickDateTime LIKE '%' + @SearchTerm + '%')                   
  OR (ISNULL(@SearchTerm, '') = '' OR DeliveryDateTime LIKE '%' + @SearchTerm + '%')                    
  OR (ISNULL(@SearchTerm, '') = '' OR ShipmentRefNo LIKE '%' + @SearchTerm + '%')   )                 
                   
                         
                        
  ORDER BY                         
  CASE WHEN (@SortColumn = 'UserId' AND @SortOrder='asc') THEN CreatedOn END ASC,     
    CASE WHEN (@SortColumn = 'UserId' AND @SortOrder='desc') THEN CreatedOn END DESC,                                           
  CASE WHEN (@SortColumn = 'StatusName' AND @SortOrder='asc') THEN StatusName END ASC,                        
     CASE WHEN (@SortColumn = 'StatusName' AND @SortOrder='desc') THEN StatusName END DESC,                        
                        
  CASE WHEN (@SortColumn = 'DriverName' AND @SortOrder='asc') THEN DriverName END ASC,                        
     CASE WHEN (@SortColumn = 'DriverName' AND @SortOrder='desc') THEN DriverName END DESC,                        
                        
  CASE WHEN (@SortColumn = 'AirWayBill' AND @SortOrder='asc') THEN AirWayBill END ASC,                        
     CASE WHEN (@SortColumn = 'AirWayBill' AND @SortOrder='desc') THEN AirWayBill END DESC,                        
                        
  CASE WHEN (@SortColumn = 'PickDateTime' AND @SortOrder='asc') THEN PickDateTime END ASC,                        
     CASE WHEN (@SortColumn = 'PickDateTime' AND @SortOrder='desc') THEN PickDateTime END DESC,                  
                    
  CASE WHEN (@SortColumn = 'DeliveryDateTime' AND @SortOrder='asc') THEN DeliveryDateTime END ASC,                        
     CASE WHEN (@SortColumn = 'DeliveryDateTime' AND @SortOrder='desc') THEN DeliveryDateTime END DESC,                  
                    
  CASE WHEN (@SortColumn = 'ShipmentRefNo' AND @SortOrder='asc') THEN ShipmentRefNo END ASC,                        
     CASE WHEN (@SortColumn = 'ShipmentRefNo' AND @SortOrder='desc') THEN ShipmentRefNo END DESC                          
                        
  OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY;          
      IF EXISTS                          
        (                          
            SELECT 1                          
            FROM #tempAll                          
        )                          
            BEGIN                          
                SELECT @TotalCount =                          
                (                          
                    SELECT TOP 1 TL.TotalCount                          
                    FROM #tempAll TL                          
                );                               
        END;                          
            ELSE                          
            BEGIN                          
                SELECT @TotalCount = 0;                          
        END;                                
 DROP TABLE #tempAll                       
                   
                    
  --------------------------------------------------------------------------------------------------                    
                        
                        
END