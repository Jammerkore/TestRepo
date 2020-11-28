--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Gets assortment view for header/user
--dv History:		8/13/2014 -TT#1268-MD -jsobek -5.4 Merge
--dv =============================================
CREATE FUNCTION [dbo].[UDF_ASRT_GET_USER_VIEW]
(
	@HDR_RID int,
	@USER_RID int
)
RETURNS INT
AS
BEGIN
	DECLARE @VIEW_RID INT
	SET @VIEW_RID = 
	(
		SELECT TOP 1 VIEW_RID 
					FROM ASSORTMENT_USER_VIEW_JOIN uvj WITH (NOLOCK) 
					WHERE HDR_RID = @HDR_RID and USER_RID = @USER_RID
	)

	RETURN @VIEW_RID
END
GO