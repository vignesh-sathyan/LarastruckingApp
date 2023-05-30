CREATE PROC [dbo].[usp_GetShipmentList_Test]        
(@SearchTerm VARCHAR(50),         
 @SortColumn VARCHAR(50),         
 @SortOrder  VARCHAR(50),         
 @PageNumber INT,         
 @PageSize   INT,    
 @StartDate   DATE=NULL,    
 @EndDate   DATE=NULL ,    
@CustomerId int=null,    
@StatusId int=null,    
@FreightTypeId int= null    
 --@TotalCount INT OUT        
)        
AS        
    BEGIN         
 SET NOCOUNT ON;          
 SET @SortColumn = LOWER(ISNULL(@SortColumn, ''));        
        SET @SortOrder = LOWER(ISNULL(@SortOrder, ''));        
        SET @SearchTerm = ISNULL(@SearchTerm, '');             
        
select * from view_GetAllShipment where
  --(    
  --    ((NullIf(@StartDate, '') IS NULL)  OR CAST(PickupDate AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS DATE) >= @StartDate )      
  --  AND ((NullIf(@EndDate, '') IS NULL)  OR CAST(PickupDate AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time' AS DATE) <=  @EndDate)      
  -- )      
   --  AND(    
   --   ((NullIf(@CustomerId, '') IS NULL)  OR CustomerId = @CustomerId)      
   --)    
   --  AND(    
   --   ((NullIf(@FreightTypeId , '') IS NULL)  OR FreightTypeId = @FreightTypeId)      
   --)    
   --  AND(    
   --   ((NullIf(@StatusId, '') IS NULL)  OR StatusId = @StatusId)      
   --)    AND
    (        
            AirWayBill LIKE '%' + @SearchTerm + '%'         
       OR CustomerPO LIKE '%' + @SearchTerm + '%'        
          OR StatusName LIKE '%' + @SearchTerm + '%'                                                                 
                       OR CustomerName LIKE '%' + @SearchTerm + '%'        
                        OR Driver LIKE '%' + @SearchTerm + '%'        
                        OR Equipment LIKE '%' + @SearchTerm + '%'        
                        OR PickupDate LIKE '%' + @SearchTerm + '%'        
                        OR DeliveryDate LIKE '%' + @SearchTerm + '%'        
                        OR PickupLocation LIKE '%' + @SearchTerm + '%'        
                        OR DeliveryLocation LIKE '%' + @SearchTerm + '%'        
                        )        
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
                      END DESC        
         OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY;             
END;