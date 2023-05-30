CREATE PROCEDURE [dbo].[usp_CustomerFumigationTrackDetails]                                    
(                                        
 @FumigationId INT,    
 @FumigationRouteId INT                                                    
)                                    
AS                                    
BEGIN                  
SET NOCOUNT ON;                                  
                                                        
   SELECT  distinct                                               
   TS.FumigationId,                                                     
   TSRS.FumigationRoutsId,                                                      
   TS.ShipmentRefNo,                                                      
   TSRS.AirWayBill,                                                      
   TSRS.CustomerPO,                                                                                                   
   TSRS.ContainerNo,                                                      
  CONCAT(A1.CompanyName ,', ', A1.Address1 + ' ' + A1.City  + ' ' + S1.Name + ' ' + CONVERT(Varchar(200),A1.Zip) + ' ' + C1.Name) PickUpLocation,              
  CONCAT(A2.CompanyName ,', ', A2.Address1 + ' ' + A2.City  + ' ' + S2.Name + ' ' + CONVERT(Varchar(200),A2.Zip) + ' ' + C2.Name) DeliveryAddress ,         
   CONCAT(A3.CompanyName ,', ', A3.Address1 + ' ' + A3.City  + ' ' + S3.Name + ' ' + CONVERT(Varchar(200),A3.Zip) + ' ' + C3.Name) FumigationAddress ,          
                                               
   TSRS.PickUpArrival as PickUpArrivalDate,                                                      
   TSRS.DeliveryArrival as DeliveryArrive ,        
    FumigationArrival FumigationDateTime,      
 DepartureDate FumigationDepartureDateTime                         
                                            
  FROM [dbo].[tblFumigation] TS                                                     
  INNER JOIN [dbo].[tblFumigationRouts] TSRS ON TSRS.FumigationId = TS.FumigationId                          
  INNER JOIN [dbo].[tblAddress] A1 ON A1.AddressId = TSRS.PickupLocation                                                     
  INNER JOIN  [dbo].[tblState] S1 ON S1.ID = A1.State                                                      
  INNER JOIN [dbo].[tblCountry] C1 ON C1.ID = A1.Country                                    
  INNER JOIN [dbo].[tblAddress] A2 ON A2.AddressId = TSRS.DeliveryLocation                                             
  INNER JOIN  [dbo].[tblState] S2 ON S2.ID = A2.State                                                      
  INNER JOIN [dbo].[tblCountry] C2 ON C2.ID = A2.Country        
  LEFT JOIN [dbo].[tblAddress] A3 ON A3.AddressId = TSRS.FumigationSite            
  LEFT JOIN [dbo].[tblState] S3 ON S3.ID = A3.State            
  LEFT JOIN [dbo].[tblCountry] C3 ON C3.ID = A3.Country                             
                                                 
  WHERE                                                       
  TS.FumigationId = @FumigationId 
  --AND  FumigationRoutsId= @FumigationRouteId                                          
                                  
                                  
END