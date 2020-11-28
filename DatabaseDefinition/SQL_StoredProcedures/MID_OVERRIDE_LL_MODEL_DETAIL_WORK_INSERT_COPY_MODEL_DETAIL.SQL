--dv =============================================
--dv Create date:	11/30/2013
--dv Description:	Inserts a Model Work Override Low Level Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO OVERRIDE_LL_MODEL_DETAIL_WORK  
		(
		OLL_RID,
		HN_RID,
		VERSION_RID,
		EXCLUDE_IND
		)
		SELECT
			omd.*
		FROM OVERRIDE_LL_MODEL_DETAIL omd
		WHERE omd.OLL_RID = @OLL_RID

	SELECT @@ROWCOUNT;
END
GO









