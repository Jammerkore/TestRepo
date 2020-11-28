--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts Assortment Properties
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STYLE_CLOSED_INSERT]
	@HDR_RID int,
	@SGL_RID int,
	@GRADE int,
	@CLOSED char(1)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_STYLE_CLOSED
		(
		HDR_RID, 
		SGL_RID, 
		GRADE,
		CLOSED
		)		
	VALUES
		(
		@HDR_RID,
		@SGL_RID,
		@GRADE,
		@CLOSED
		)		

    SELECT @@ROWCOUNT;
END

GO