--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Reads all header packs
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM HEADER_PACK

END
GO
