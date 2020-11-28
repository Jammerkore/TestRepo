--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Inserts a row on the method copy group level table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_GROUP_LEVEL_INSERT]
	@METHOD_RID int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_COPY_GROUP_LEVEL 
		(
		METHOD_RID, 
		SGL_RID
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID
		)

	SELECT @@ROWCOUNT
END
GO