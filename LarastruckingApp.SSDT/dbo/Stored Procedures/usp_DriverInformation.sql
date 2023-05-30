CREATE PROCEDURE [dbo].[usp_DriverInformation]      
(      
 @SpType INT  = NULL,      
 @DriverId INT = NULL      
)      
AS      
BEGIN      
      
 IF(@SpType = 1)      
 -- GET DRIVERS DOCUMENT      
 BEGIN      
  SET NOCOUNT ON;       
  SELECT       
   D.DriverId,       
   D.FirstName,      
   D.LastName,      
   D.EmailId Email,      
   D.Address1,      
   D.Address2,      
   D.City,      
   TS.Name [State],      
   TC.Name [Country],      
   D.Phone,      
   DriverDocuments =NULL      
  FROM  [dbo].[tblDriver] D      
  INNER JOIN  [dbo].[tblState] TS ON D.State = TS.ID      
  INNER JOIN [dbo].[tblCountry] TC ON D.Country = TC.ID      
  WHERE      
  D.DriverID = @DriverId AND      
 -- D.IsActive = 1 AND       
  D.IsDeleted = 0      
 END      
------------------------------------------------------------------------------------------------------------      
 ELSE IF(@SpType = 2)      
 -- GET ALL DOCUMENTS OF DRIVER      
 BEGIN      
 SET NOCOUNT ON;       
  SELECT       
   DocumentId,       
   DocumentTypeId,      
   DD.ImageName,      
   CASE      
     WHEN DT.Name = 'OTHER' THEN DocumentName      
     ELSE DT.Name      
   END DocumentName,      
   DocumentExpiryDate ExpiryDate,      
   ImageURL      
  FROM  [dbo].[tblDriver] D      
  INNER JOIN  [dbo].[tblDriverDocument] DD ON D.DriverID = DD.DriverId      
  INNER JOIN [dbo].[tblDocumentType] DT ON DD.DocumentTypeId = DT.ID      
  WHERE      
  D.DriverID = @DriverId AND      
  DD.IsActive = 1 AND       
  D.IsDeleted = 0 AND      
  DD.IsDeleted = 0      
 END      
END 
   
	
