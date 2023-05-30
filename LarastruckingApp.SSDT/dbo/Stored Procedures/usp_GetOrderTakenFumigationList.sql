--exec [usp_GetOrderTakenFumigationList] '','FumigationId','asc',0,100    
  
CREATE PROC [dbo].[usp_GetOrderTakenFumigationList]                      
(@SearchTerm VARCHAR(50),                       
 @SortColumn VARCHAR(50),                       
 @SortOrder  VARCHAR(50),                       
 @PageNumber INT,                       
 @PageSize   INT                       
 --@TotalCount INT OUT                      
)                      
AS                      
    BEGIN                            
  SET NOCOUNT ON;                             
        -- calculate the starting and ending of records                                                
        SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));                      
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));                      
        SET @SearchTerm = ISNULL(@SearchTerm, '');                    
       
    WITH                
  CTE_FumigationLocation(FumigationIds,PickUpLocation,FumigationSite,DeliveryLocation,ActLoadingStart,ActLoadingFinish,ActFumigationIn,
  ActFumigationRelease,ActDepartureDate,ActDeliveryArrival,ActDeliveryDeparture,AWB_CP_CN,BoxCount,PalletCount,TrailerPosition,VendorNConsignee,FumigationTypes,Temperature)                      
 AS  
    (  select FUM.FumigationId,  
STRING_AGG(CONCAT(PA.CompanyName,'||',PA.Address1,' ',PA.City,' ',PST.Name,' ',PA.Zip ),'$') as PickupLocation,  
STRING_AGG(CONCAT(FS.CompanyName,'||',FS.Address1,' ',FS.City,' ',FST.Name,' ',FS.Zip),'$') AS FumigationSite,  
STRING_AGG(CONCAT(DA.CompanyName,'||',DA.Address1,' ',DA.City,' ',DST.Name,' ',DA.Zip),'$') AS DeliveryLocation,  
  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DriverLoadingStartTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '$') AS ActLoadingStart,  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DriverLoadingFinishTime AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '$') AS ActLoadingFinish,  
  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DriverFumigationIn AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|') AS ActFumigationIn,  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DriverFumigationRelease AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '$') AS ActFumigationRelease,  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DepartureDate AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '|')  AS ActDepartureDate,  
  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DriverDeliveryArrival AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '$') AS ActDeliveryArrival,  
STRING_AGG (CONVERT(VARCHAR(MAX),CAST (FUR.DriverDeliveryDeparture AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar )), '$') AS ActDeliveryDeparture,  
STRING_AGG (CONCAT( FUR.AirWayBill,' ',FUR.CustomerPO,' ',FUR.ContainerNo),'$') AS AWB_CP_CN ,  
SUM(FUR.BoxCount) AS BoxCount,                       
SUM(FUR.PalletCount) AS PalletCount,  
STRING_AGG (FUR.TrailerPosition ,'$') AS TrailerPosition ,  
STRING_AGG (FUR.VendorNConsignee,'$') AS VendorNConsignee,  
STRING_AGG (FT.FumigationName,'$') AS FumigationTypes,  
STRING_AGG (FUR.Temperature,'$') AS Temperature  
from tblFumigation FUM  
Left Join tblFumigationRouts FUR ON FUM.FumigationId=FUR.FumigationId  
LEFT JOIN tblFumigationTypes FT ON FT.FumigationTypeId = FUR.FumigationTypeId   
Left join tblAddress PA ON FUR.PickUpLocation=PA.AddressId        
Left join tblState PST ON PA.State =PST.ID        
Left join tblAddress FS ON FUR.FumigationSite=FS.AddressId        
Left join tblState FST ON FS.State =FST.ID        
Left join tblAddress DA ON FUR.DeliveryLocation=DA.AddressId        
Left join tblState DST ON DA.State =DST.ID        
where  FUM.StatusId!=1 AND FUM.StatusId!=11 AND FUM.StatusId!=8 AND FUM.IsDeleted=0 and FUR.IsDeleted=0  
GROUP BY FUM.FumigationId  
--ORDER BY FumigationId  
) ,  
 CTE_PickUpDriver(FumigationIds,                         
                              PickUpDriver)                      
                            
             AS (SELECT DISTINCT                        
                        (FumigationId),                         
                        STUFF(                        
                 (                        
                     SELECT '$' +                        
                     (                        
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName                        
                         FROM tblDriver D                        
                         WHERE D.DriverId = FED.DriverId                        
                     )                        
                     FROM tblFumigationEquipmentNDriver AS FED           
         left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId          
                     WHERE FED.FumigationId = FUM.FumigationId                        
      AND FR.IsDeleted=0          
                           AND FED.IsDeleted = 0                        
                           AND IsPickUp = 1              
         ORDER BY FR.RouteNo           
                     --GROUP BY FED.DriverId,                         
                     --FED.FumigationId           
      FOR XML PATH('')                        
                 ), 1, 1, '') AS PickUpDriver                        
                 FROM tblFumigation FUM where  FUM.StatusId!=1 AND FUM.StatusId!=11 AND FUM.StatusId!=8 AND FUM.IsDeleted=0 ) ,  
       
                     
             CTE_DeliveryDriver(FumigationIds,                         
                                DeliveryDriver)                        
 AS (SELECT DISTINCT                        
                        (FumigationId),                         
                        STUFF(                        
                 (                        
   SELECT '$' +                        
                     (                        
                         SELECT CONCAT(D.FirstName, ' ', D.LastName) DriverName                        
                         FROM tblDriver D                        
                         WHERE D.DriverId = FED.DriverId                        
                     )                        
                     FROM tblFumigationEquipmentNDriver AS FED              
        left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId                    
                     WHERE FED.FumigationId = FUM.FumigationId            
      AND FR.IsDeleted=0                      
                           AND FED.IsDeleted = 0                        
                           AND IsPickUp = 0  ORDER BY FR.RouteNo                       
                     --GROUP BY FED.DriverId,                         
                            --  FED.FumigationId           
         FOR XML PATH('')                        
                 ), 1, 1, '') AS DeliveryDriver                        
                 FROM tblFumigation FUM where  FUM.StatusId!=1 AND FUM.StatusId!=11 AND FUM.StatusId!=8 AND FUM.IsDeleted=0),            
                      
                   CTE_PickUpEquipment(FumigationIds,                         
                                 PickUpEquipment)                        
             AS (SELECT DISTINCT                        
                        (FumigationId),                         
                        STUFF(                        
                 (                        
                     SELECT '$' +                        
                     (                        
                         SELECT EquipmentNo                        
                         FROM tblEquipmentDetail                        
                         WHERE EDID = FED.EquipmentId                        
                     )                        
                     FROM tblFumigationEquipmentNDriver AS FED           
      left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId     
                     WHERE FED.FumigationId = FUM.FumigationId             
      AND FR.IsDeleted=0                          
                     AND FED.IsDeleted = 0                            
                     AND IsPickUp = 1                            
   -- GROUP BY FED.EquipmentId,                         
                           --   FED.FumigationId          
          FOR XML PATH('')                        
                 ), 1, 1, '') AS PickUpEquipment                        
                 FROM tblFumigation FUM where  FUM.StatusId!=1 AND FUM.StatusId!=11 AND FUM.StatusId!=8 AND FUM.IsDeleted=0),           
                          
                 
                   
             CTE_DeliveryEquipment(FumigationIds,                         
                                   DeliveryEquipment)                        
             AS (SELECT DISTINCT                        
                        (FumigationId),                         
                        STUFF(                        
                 (                       
                     SELECT '$' +                        
                     (                        
                         SELECT EquipmentNo                        
                         FROM tblEquipmentDetail                        
                         WHERE EDID = FED.EquipmentId                        
                     )                        
                     FROM tblFumigationEquipmentNDriver AS FED            
      left join tblFumigationRouts FR on FED.FumigationRoutsId = FR.FumigationRoutsId                         
                     WHERE FED.FumigationId = FUM.FumigationId          AND FR.IsDeleted=0                              
                           AND FED.IsDeleted = 0                        
                           AND IsPickUp = 0                        
                   --  GROUP BY FED.EquipmentId,                         
                          --    FED.FumigationId           
         FOR XML PATH('')                        
                 ), 1, 1, '') AS DeliveryEquipment                        
                 FROM tblFumigation FUM where  FUM.StatusId!=1 AND FUM.StatusId!=11 AND FUM.StatusId!=8 AND FUM.IsDeleted=0)        
               
  
  
  
  select  
   FUM.FumigationId,  
   ISNULL(CFM.VendorNConsignee, '') VendorNConsignee,  
   CR.CustomerName,   
   SS.StatusAbbreviation as StatusName,  
   ISNULL(CFM.FumigationTypes, '') FumigationTypes,                         
         ISNULL(CFM.TrailerPosition, '') TrailerPosition,  
  
   ISNULL(CFM.PickupLocation , '') AS PickupLocation,  
   ISNULL(CFM.FumigationSite , '') AS FumigationSite,   
   ISNULL(CFM.DeliveryLocation , '') AS DeliveryLocation,  
  
   ISNULL(CFM.ActLoadingStart , '') AS ActLoadingStart,   
   ISNULL(CFM.ActLoadingFinish , '') AS ActLoadingFinish,  
      
   ISNULL(CFM.ActFumigationIn , '') AS ActFumigationIn,    
   ISNULL(CFM.ActFumigationRelease , '') AS ActFumigationRelease,    
   ISNULL(CFM.ActDepartureDate , '') AS ActDepartureDate,   
  
   ISNULL(CFM.ActDeliveryArrival , '') AS ActDeliveryArrival,   
   ISNULL(CFM.ActDeliveryDeparture , '') AS ActDeliveryDeparture,   
    
   ISNULL(CFM.Temperature , '') AS Temperature,  
   ISNULL(CFM.AWB_CP_CN , '') AS AWB_CP_CN,   
   ISNULL(CFM.BoxCount, 0) AS BoxCount,                         
   ISNULL(CFM.PalletCount, 0) AS PalletCount,                         
   ISNULL(PD.PickUpDriver, '') AS PickUpDriver,                         
   ISNULL(PE.PickUpEquipment, '') AS PickUpEquipment,                         
   ISNULL(DD.DeliveryDriver, '') AS DeliveryDriver,                         
   ISNULL(DE.DeliveryEquipment, '') AS DeliveryEquipment,   
   (CONVERT(VARCHAR(MAX),CAST (FUM.CreatedOn AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS varchar ))) AS RequestedAt ,  
    ISNULL( (select TOP(1)   (case when FUM.StatusId=SSH.StatusId then 1 else 0 end) ApproveStatus from tblFumigationStatusHistory SSH where SSH.FumigationId=FUM.FumigationId order by FumigationStatusHistoryId desc),0) as ApproveStatus,  
   [TotalCount] = COUNT(CFM.FumigationIds) OVER()      
   from  CTE_FumigationLocation CFM  
   INNER JOIN tblFumigation FUM ON CFM.FumigationIds=FUM.FumigationId  
   INNER JOIN [dbo].[tblCustomerRegistration] CR ON FUM.CustomerId = CR.CustomerId                        
   Left JOIN [dbo].[tblShipmentStatus] SS ON FUM.StatusId = SS.StatusId     
   LEFT JOIN CTE_PickUpDriver PD ON FUM.FumigationId = PD.FumigationIds                        
         LEFT JOIN CTE_PickUpEquipment PE ON FUM.FumigationId = PE.FumigationIds                        
         LEFT JOIN CTE_DeliveryEquipment DE ON FUM.FumigationId = DE.FumigationIds                        
         LEFT JOIN CTE_DeliveryDriver DD ON FUM.FumigationId = DD.FumigationIds        
                    
             WHERE FUM.IsDeleted = 0                 
                   AND (CustomerName LIKE '%' + @SearchTerm + '%'                      
                        OR StatusName LIKE '%' + @SearchTerm + '%'                                                        
                        OR AWB_CP_CN LIKE '%' + @SearchTerm + '%'                                        
						OR PickupLocation LIKE '%' + @SearchTerm + '%'
						OR FumigationSite LIKE '%' + @SearchTerm + '%'
						OR DeliveryLocation LIKE '%' + @SearchTerm + '%'                          
                        OR PickUpDriver LIKE '%' + @SearchTerm + '%'                      
                        OR PickUpEquipment LIKE '%' + @SearchTerm + '%'                      
                        OR DeliveryDriver LIKE '%' + @SearchTerm + '%'                      
                        OR DeliveryEquipment LIKE '%' + @SearchTerm + '%'                      
                        OR CFM.VendorNConsignee LIKE '%' + @SearchTerm + '%'                      
                        OR FumigationTypes LIKE '%' + @SearchTerm + '%'                      
                        OR TrailerPosition LIKE '%' + @SearchTerm + '%')                      
             ORDER BY CASE                      
      WHEN(@SortColumn = 'FumigationId'                      
                               AND @SortOrder = 'asc')                      
                          THEN FumigationId                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'FumigationId'                      
                               AND @SortOrder = 'desc')                      
                          THEN FumigationId                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'StatusName'                      
                               AND @SortOrder = 'asc')                      
                          THEN StatusName                      
                      END ASC,                            CASE                      
                          WHEN(@SortColumn = 'StatusName'                      
                               AND @SortOrder = 'desc')                      
                          THEN StatusName                      
                      END DESC,   
					  
					      CASE                      
                          WHEN(@SortColumn = 'PickUpEquipment'                      
                               AND @SortOrder = 'asc')                      
                          THEN StatusName                      
                      END ASC,                            CASE                      
                          WHEN(@SortColumn = 'PickUpEquipment'                      
                               AND @SortOrder = 'desc')                      
                          THEN StatusName                      
                      END DESC,                    
                      CASE                      
                          WHEN(@SortColumn = 'CustomerName'                      
                               AND @SortOrder = 'asc')                      
                         THEN CustomerName                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'CustomerName'                      
                               AND @SortOrder = 'desc')                      
                          THEN CustomerName                      
                      END DESC,  
					  CASE                      
                          WHEN(@SortColumn = 'FumigationTypes'                      
                               AND @SortOrder = 'asc')                      
                          THEN FumigationTypes                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'FumigationTypes'                      
                               AND @SortOrder = 'desc')                      
                          THEN FumigationTypes                      
                      END DESC, 
					  
					   CASE                      
                          WHEN(@SortColumn = 'PickUpLocation'                      
                               AND @SortOrder = 'asc')                      
                          THEN FumigationTypes                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'PickUpLocation'                      
                               AND @SortOrder = 'desc')                      
                          THEN FumigationTypes                      
                      END DESC,   
					   CASE                      
                          WHEN(@SortColumn = 'AWB_CP_CN'                      
                               AND @SortOrder = 'asc')                      
                          THEN FumigationTypes                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'AWB_CP_CN'                      
                               AND @SortOrder = 'desc')                      
                          THEN FumigationTypes                      
                      END DESC,                  
                   
                      CASE                      
                          WHEN(@SortColumn = 'VendorNconsignee'                      
                               AND @SortOrder = 'asc')                      
                          THEN BoxCount      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'VendorNconsignee'                      
                               AND @SortOrder = 'desc')                      
                          THEN BoxCount                      
            END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'TrailerPosition'                      
                               AND @SortOrder = 'asc')                      
                          THEN PalletCount                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'TrailerPosition' 
						  AND @SortOrder = 'desc')                      
                          THEN PalletCount                      
                      END DESC,  
					        CASE                      
                          WHEN(@SortColumn = 'PickUpDriver'                      
                               AND @SortOrder = 'asc')                      
                          THEN PalletCount                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'PickUpDriver' 
						  AND @SortOrder = 'desc')                      
                          THEN PalletCount                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'FumigationSite'                      
                  AND @SortOrder = 'asc')                      
                          THEN PickUpDriver                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'FumigationSite'                      
                               AND @SortOrder = 'desc')                      
                          THEN PickUpDriver                      
                      END DESC,                      
                      CASE                    
                          WHEN(@SortColumn = 'DeliveryEquipment'                      
                               AND @SortOrder = 'asc')                      
                          THEN PickUpEquipment                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryEquipment'                      
                               AND @SortOrder = 'desc')                      
                          THEN PickUpEquipment                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryDriver'                      
                      AND @SortOrder = 'asc')                      
                          THEN DeliveryDriver                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryDriver'                      
                               AND @SortOrder = 'desc')                      
                          THEN DeliveryDriver                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryEquipment'                      
                               AND @SortOrder = 'asc')                      
                          THEN DeliveryEquipment   
           END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryEquipment'                      
                   AND @SortOrder = 'desc')                      
                          THEN DeliveryEquipment                      
                      END DESC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryLocation'                      
                               AND @SortOrder = 'asc')                      
                          THEN CFM.AWB_CP_CN                      
                      END ASC,                      
                      CASE                      
                          WHEN(@SortColumn = 'DeliveryLocation'                      
                               AND @SortOrder = 'desc')                      
                          THEN CFM.AWB_CP_CN           
                      END DESC                    
   
             OFFSET @PageNumber ROWS FETCH NEXT case when @PageSize>0 then @PageSize else 999999 end ROWS ONLY;                      
                     
 END;