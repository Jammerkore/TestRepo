--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads a filter from the user and filter name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_FROM_USER_AND_NAME]
	@FILTER_TYPE int,
	@USER_RID int,
	@FILTER_NAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM FILTER 
	WHERE FILTER_TYPE = @FILTER_TYPE
		AND OWNER_USER_RID = @USER_RID 
		AND FILTER_NAME = @FILTER_NAME

END
GO


