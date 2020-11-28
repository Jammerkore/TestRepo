--dv =============================================
--dv Create date:	11/30/2013
--dv Modified date: 3/31/2014
--dv Description:	Inserts a Header Work Override Low Level Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE]
	@OLL_RID int,
	@ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CustomUserRID INT = 5;

	INSERT INTO OVERRIDE_LL_MODEL_HEADER_WORK  
		(
		OLL_RID,
		NAME,
		HN_RID,
		HIGH_LEVEL_HN_RID,
		HIGH_LEVEL_SEQ,
		HIGH_LEVEL_OFFSET,
		HIGH_LEVEL_TYPE,
		LOW_LEVEL_SEQ,
		LOW_LEVEL_OFFSET,
		LOW_LEVEL_TYPE,
		ACTIVE_ONLY_IND,
		USER_RID
		)
		SELECT
			ollmh.*,
			COALESCE(ui.USER_RID, @CustomUserRID) as USER_RID
		FROM OVERRIDE_LL_MODEL_HEADER ollmh
		LEFT OUTER JOIN USER_ITEM ui ON ollmh.OLL_RID = ui.ITEM_RID AND ui.ITEM_TYPE = @ITEM_TYPE
		WHERE ollmh.OLL_RID = @OLL_RID

	SELECT @@ROWCOUNT;
END
GO









