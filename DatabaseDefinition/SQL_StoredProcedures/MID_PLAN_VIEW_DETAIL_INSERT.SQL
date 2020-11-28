--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Inserts a plan view detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_DETAIL_INSERT] 
	@VIEW_RID int,
    @AXIS int,
    @AXIS_SEQUENCE int,
    @PROFILE_TYPE int,
    @PROFILE_KEY int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PLAN_VIEW_DETAIL
		(
		VIEW_RID,
        AXIS,
        AXIS_SEQUENCE,
        PROFILE_TYPE,
        PROFILE_KEY
		)
	VALUES
		(
		@VIEW_RID,
        @AXIS,
        @AXIS_SEQUENCE,
        @PROFILE_TYPE,
        @PROFILE_KEY
		)

	SELECT @@ROWCOUNT;

END
GO