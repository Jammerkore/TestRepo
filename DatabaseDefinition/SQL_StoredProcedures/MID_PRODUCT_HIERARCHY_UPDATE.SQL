--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Updates the product hierarchy for the specified PH_RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_UPDATE]
	@PH_RID int,
	@PH_ID varchar(50),
	@PH_TYPE int,
	@PH_COLOR varchar(30),
	@PH_OWNER int,
	@OTS_PLANLEVEL_TYPE int,
	@HISTORY_ROLL_OPTION int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PRODUCT_HIERARCHY WITH (ROWLOCK) SET 
		PH_ID = @PH_ID,
		PH_TYPE = @PH_TYPE,
		PH_COLOR = @PH_COLOR,
        PH_OWNER = @PH_OWNER,
		OTS_PLANLEVEL_TYPE = @OTS_PLANLEVEL_TYPE,
		HISTORY_ROLL_OPTION = @HISTORY_ROLL_OPTION
	WHERE PH_RID = @PH_RID

	SELECT @@ROWCOUNT;

END
GO
