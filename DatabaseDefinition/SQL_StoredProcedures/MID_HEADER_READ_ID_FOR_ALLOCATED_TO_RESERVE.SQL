--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header IDs where all units are allocated to the reserve store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_ID_FOR_ALLOCATED_TO_RESERVE]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_ID 
	FROM HEADER 
	WHERE UNITS_RECEIVED = RSV_ALLOCATED_UNITS

END
GO
