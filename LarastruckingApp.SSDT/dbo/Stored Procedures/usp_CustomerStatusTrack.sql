 CREATE PROC usp_CustomerStatusTrack
 (
  @ShipmentId INT     
 )
 as
 BEGIN
 SET NOCOUNT ON;   
  select     
   TSSH.ShipmentId,      
   TSSH.StatusId,            
   TSSH.SubStatusId,    
   TSSH.Reason,    
   TSSH.CreatedOn,            
   TSS.StatusName,                      
   TSSS.SubStatusName                    
                
   From [dbo].[tblShipmentStatusHistory] TSSH    
   INNER JOIN [dbo].[tblShipmentStatus] TSS ON TSS.StatusId = TSSH.StatusId                      
   LEFT JOIN [dbo].[tblShipmentSubStatus] TSSS ON TSSS.SubStatusId = TSSH.SubStatusId       
   WHERE                                                 
   TSSH.ShipmentId =@ShipmentId
 END