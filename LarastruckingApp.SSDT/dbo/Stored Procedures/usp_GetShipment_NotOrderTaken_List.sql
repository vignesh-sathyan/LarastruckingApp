CREATE PROC [dbo].[usp_GetShipment_NotOrderTaken_List]                    
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
 SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));                    
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));                    
        SET @SearchTerm = ISNULL(@SearchTerm, '');                         
                    
select  [TotalCount]= COUNT(TotalCountRow) over(), ShipmentId,AirWayBill,CustomerPO,StatusId, StatusName,CustomerID,CustomerName,Driver,Equipment,PickupDate,DeliveryDate,PickupLocation,DeliveryLocation,Quantity,ApproveStatus ,Temperature, Commodity       
    
       from view_GetOrderNotTakenShipment where            
                     
              
    (                    
            AirWayBill LIKE '%' + @SearchTerm + '%'                     
       OR CustomerPO LIKE '%' + @SearchTerm + '%'   
	    OR OrderNo LIKE '%' + @SearchTerm + '%' 
		 OR CustomerRef LIKE '%' + @SearchTerm + '%' 
		  OR ContainerNo LIKE '%' + @SearchTerm + '%' 
		   OR PurchaseDoc LIKE '%' + @SearchTerm + '%'                   
          OR StatusName LIKE '%' + @SearchTerm + '%'                                                                             
                       OR CustomerName LIKE '%' + @SearchTerm + '%'                    
                        OR Driver LIKE '%' + @SearchTerm + '%'                    
                        OR Equipment LIKE '%' + @SearchTerm + '%'                    
                        OR PickupDate LIKE '%' + @SearchTerm + '%'                    
                        OR DeliveryDate LIKE '%' + @SearchTerm + '%'                    
                        OR PickupLocation LIKE '%' + @SearchTerm + '%'                    
                        OR DeliveryLocation LIKE '%' + @SearchTerm + '%'    
      OR Commodity LIKE '%' + @SearchTerm + '%'                               
                        )               
      group by TotalCountRow, ShipmentId,AirWayBill,CustomerPO,StatusId, StatusName,CustomerID,CustomerName,Driver,Equipment,PickupDate,DeliveryDate,PickupLocation,DeliveryLocation,Quantity ,ApproveStatus,Temperature, Commodity                    
ORDER BY CASE                    
                          WHEN(@SortColumn = 'ShipmentId'                    
                               AND @SortOrder = 'asc')                    
                          THEN ShipmentId                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'ShipmentId'                    
                               AND @SortOrder = 'desc')                    
                          THEN ShipmentId                    
                      END DESC,                    
                      CASE                    
                          WHEN(@SortColumn = 'StatusName'                    
                               AND @SortOrder = 'asc')                    
 THEN StatusName                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'StatusName'                    
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
   WHEN(@SortColumn = 'AirWayBill'                    
                               AND @SortOrder = 'asc')                    
                          THEN AirWayBill                    
                      END ASC,                    
                      CASE              
                          WHEN(@SortColumn = 'AirWayBill'                    
                               AND @SortOrder = 'desc')                    
                          THEN AirWayBill                    
                      END DESC,                    
                     CASE                    
                          WHEN(@SortColumn = 'CustomerPO'                    
             AND @SortOrder = 'asc')                    
                          THEN AirWayBill                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'CustomerPO'                    
                               AND @SortOrder = 'desc')                    
                          THEN CustomerPO                    
                      END DESC,                    
            CASE                    
                          WHEN(@SortColumn = 'Driver'                    
                               AND @SortOrder = 'asc')                    
           THEN Driver                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'Driver'                    
                               AND @SortOrder = 'desc')                    
                          THEN  Driver                    
                      END DESC,                    
                    
         CASE                    
                          WHEN(@SortColumn = 'Equipment'                    
                               AND @SortOrder = 'asc')                    
                          THEN Equipment                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'Equipment'                    
                               AND @SortOrder = 'desc')                    
                          THEN  Equipment                    
                      END DESC,                    
        CASE                    
                          WHEN(@SortColumn = 'PickupDate'                    
                               AND @SortOrder = 'asc')                    
                          THEN PickupDate                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'PickupDate'                    
                               AND @SortOrder = 'desc')                    
                          THEN  PickupDate                    
                      END DESC,                    
        CASE                    
                          WHEN(@SortColumn = 'DeliveryDate'                    
                               AND @SortOrder = 'asc')                    
                          THEN DeliveryDate                    
                      END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'DeliveryDate'                    
                               AND @SortOrder = 'desc')                    
                          THEN  DeliveryDate                    
                      END DESC,                    
            CASE                    
                          WHEN(@SortColumn = 'PickupLocation'                    
                               AND @SortOrder = 'asc')                    
                          THEN PickupLocation                    
                      END ASC,          
                      CASE                    
                          WHEN(@SortColumn = 'PickupLocation'                    
                               AND @SortOrder = 'desc')                    
                          THEN  PickupLocation                    
                  END DESC,                    
             CASE                    
                          WHEN(@SortColumn = 'DeliveryLocation'                    
                               AND @SortOrder = 'asc')                    
                          THEN DeliveryLocation                    
              END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'DeliveryLocation'                    
                               AND @SortOrder = 'desc')                    
                          THEN DeliveryLocation              
                      END DESC  ,  
        CASE                    
                          WHEN(@SortColumn = 'Commodity'                    
                               AND @SortOrder = 'asc')                    
                          THEN Commodity                    
              END ASC,                    
                      CASE                    
                          WHEN(@SortColumn = 'Commodity'                    
                               AND @SortOrder = 'desc')                    
                          THEN Commodity              
                      END DESC                                    
         OFFSET @PageNumber ROWS FETCH NEXT case when @PageSize>0 then @PageSize else 999999 end ROWS ONLY;                         
END;