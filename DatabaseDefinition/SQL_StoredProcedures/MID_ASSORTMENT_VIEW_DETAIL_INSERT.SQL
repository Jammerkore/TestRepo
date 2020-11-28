--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Inserts Assortment User
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_DETAIL_INSERT]
	@VIEW_RID int,
	@AXIS int,
	@AXIS_SEQUENCE int,
	@PROFILE_TYPE int,
	@PROFILE_KEY int,
	@SUMMARIZED_IND char(1)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_VIEW_DETAIL
		(
		VIEW_RID,
		AXIS,
		AXIS_SEQUENCE,
		PROFILE_TYPE,
		PROFILE_KEY,
		SUMMARIZED_IND
		)		
	VALUES
		(
		@VIEW_RID,
		@AXIS,
		@AXIS_SEQUENCE,
		@PROFILE_TYPE,
		@PROFILE_KEY,
		@SUMMARIZED_IND
		)		

    SELECT @@ROWCOUNT;
END

GO