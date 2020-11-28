--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns the header based on the ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FROM_ID]
	@HDR_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HDR_RID int

	SELECT @HDR_RID=HDR_RID 
	FROM HEADER WITH (NOLOCK)  -- TT#5278 - JSmith - Header Create Performance
	WHERE HDR_ID=@HDR_ID

	SELECT * 
	FROM UDF_HEADER_READ (@HDR_RID)

END
GO
