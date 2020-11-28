--dv =============================================
--dv Create date:	8/13/2014
--dv Description:	Gets the assortment view for the user and header
--dv History:		TT#1268-MD -jsobek -5.4 Merge
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_USER_VIEW_READ]
	@HDR_RID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[UDF_ASRT_GET_USER_VIEW] (@HDR_RID, @USER_RID) AS VIEW_RID

END
GO