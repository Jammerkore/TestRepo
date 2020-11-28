--dv =============================================
--dv Create date:	11/30/2013
--dv Description:	Inserts a Detail Override Low Level Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_INSERT]
	@OLL_RID int,
	@HN_RID int,
	@VERSION_RID int,
	@EXCLUDE_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO OVERRIDE_LL_MODEL_DETAIL  
		(
		OLL_RID,
		HN_RID,
		VERSION_RID,
		EXCLUDE_IND
		)
	VALUES
		(
		@OLL_RID,
		@HN_RID,
		@VERSION_RID,
		@EXCLUDE_IND
		)

	SELECT @@ROWCOUNT;
END
GO