--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads all Store Characteristics Group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_ASSORTMENT]
		@ASRT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID, 
		HDR_ID, 
		STYLE_HNRID
    FROM HEADER 
	WHERE ASRT_RID = @ASRT_RID 
		AND DISPLAY_TYPE = '800740'
    
END
GO