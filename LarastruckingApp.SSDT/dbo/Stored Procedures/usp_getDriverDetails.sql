CREATE PROC [dbo].[usp_getDriverDetails]  
AS  
BEGIN  
  
SELECT DISTINCT  
TD.DriverID,  
TD.UserId,  
CONCAT(TD.FirstName ,' ',TD.LastName)  as DriverName,  
TD.EmailId,  
TDD.DocumentId,  
TDD.DocumentName,	
TDD.DocumentTypeId,  
TDD.DocumentExpiryDate,  
TD.IsActive ActiveDriver,
TDD.IsActive ActiveDocument,
TDD.EmailSentDate
  
FROM tblDriver TD  
INNER JOIN [dbo].[tblDriverDocument] TDD ON TDD.DriverId = TD.DriverID  
  
WHERE  


TDD.IsActive=1 AND TD.IsActive=1 AND 1 = CASE
              WHEN (DATEDIFF(day, DocumentExpiryDate, GETUTCDATE())<1) AND (DATEDIFF(day, DocumentExpiryDate, GETUTCDATE())>=-14) THEN 1  
			           
              ELSE 0
           END




--GETUTCDATE() >= dateadd(MINUTE,-7,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 0 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(MINUTE,-6,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 1 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(MINUTE,-5,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 2 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(MINUTE,-4,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 3 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(MINUTE,-3,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 4 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(MINUTE,-2,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 5 AND TDD.IsActive=1 AND TD.IsActive=1 OR
--GETUTCDATE() >= dateadd(MINUTE,-1,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 6 AND TDD.IsActive=1 AND TD.IsActive=1 


--GETUTCDATE() >= dateadd(day,-7,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 0 AND TDD.IsActive=1 AND TD.IsActive=1 OR
--GETUTCDATE() >= dateadd(day,-6,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 1 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(day,-5,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 2 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(day,-4,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 3 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(day,-3,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 4 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(day,-2,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 5 AND TDD.IsActive=1 AND TD.IsActive=1 OR   
--GETUTCDATE() >= dateadd(day,-1,DocumentExpiryDate) AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 6 AND TDD.IsActive=1 AND TD.IsActive=1 


 																									  
--GETUTCDATE() >= dateadd(day,-7,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 0 AND TDD.IsActive=1 AND TD.IsActive=1 ))OR  
--GETUTCDATE() >= dateadd(day,-6,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 1 AND TDD.IsActive=1 AND TD.IsActive=1)) OR  
--GETUTCDATE() >= dateadd(day,-5,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 2 AND TDD.IsActive=1 AND TD.IsActive=1)) OR  
--GETUTCDATE() >= dateadd(day,-4,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 3 AND TDD.IsActive=1 AND TD.IsActive=1)) OR  
--GETUTCDATE() >= dateadd(day,-3,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 4 AND TDD.IsActive=1 AND TD.IsActive=1)) OR  
--GETUTCDATE() >= dateadd(day,-2,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 5 AND TDD.IsActive=1 AND TD.IsActive=1)) OR  
--GETUTCDATE() >= dateadd(day,-1,(select min(DocumentExpiryDate) AS DocumentExpiryDate from tblDriverDocument where DriverId = 4 AND TDD.IsEmailSent = 0 AND TDD.EmailSentCount = 6 AND TDD.IsActive=1 AND TD.IsActive=1))
  
END