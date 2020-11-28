--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Gets all information from the
--dv				HEADER table for an Assortment
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_ALL_ASSORTMENT]
		@ASRT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		hdr.*
    FROM HEADER hdr
	WHERE ASRT_RID = @ASRT_RID 
    
END
GO