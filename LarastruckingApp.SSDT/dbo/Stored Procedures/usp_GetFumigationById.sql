CREATE PROC [dbo].[usp_GetFumigationById](@FumigationId INT)
AS
    BEGIN

	  SELECT *
        FROM tblFumigationRouts
        WHERE FumigationId = @FumigationId
        SELECT *
        FROM tblFumigation
        WHERE FumigationId = @FumigationId
      
        SELECT *
        FROM tblFumigationAccessorialPrice
        WHERE FumigationId = @FumigationId
    END;