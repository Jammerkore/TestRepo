--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the user allocation headers table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_HEADERS_INSERT]
	@USER_RID int,
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_ALLOCATION_HEADERS
		(
		USER_RID, 
		HDR_RID
		)
	VALUES 
		(
		@USER_RID, 
		@HDR_RID
		)

	SELECT @@ROWCOUNT;

END
GO
