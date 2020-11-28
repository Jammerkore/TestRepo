--dv =============================================
--dv Create date:	10/29/2014
--dv Description:	Reads filters for the given user and filter type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_FROM_USER]
	@FILTER_TYPE int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM FILTER 
	WHERE FILTER_TYPE = @FILTER_TYPE
		AND USER_RID = @USER_RID

END
GO


