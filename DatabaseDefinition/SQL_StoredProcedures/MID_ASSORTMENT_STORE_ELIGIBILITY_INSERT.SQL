--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Inserts Assortment Store Eligibility
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_ELIGIBILITY_INSERT]
	@HDR_RID int,
	@ST_RID int,
	@ELIGIBLE char(1)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_STORE_ELIGIBILITY
		(
		HDR_RID, 
		ST_RID, 
		ELIGIBLE
		)		
	VALUES
		(
		@HDR_RID,
		@ST_RID,
		@ELIGIBLE
		)		

    SELECT @@ROWCOUNT;
END

GO