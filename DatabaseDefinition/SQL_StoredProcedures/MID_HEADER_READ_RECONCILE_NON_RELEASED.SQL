--dv ====================================================================================
--dv Create date:	04/09/2015
--dv Description:	Returns all non-released headers 
--dv except the default header, assortment headers, placeholders, and multi-headers
--dv                TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
--dv ====================================================================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_RECONCILE_NON_RELEASED]
@HEADER_TYPE_LIST DISPLAY_TYPE_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @defaultHeaderId INT = 1;

	SELECT HDR_ID FROM VW_GET_HEADERS h
	WHERE Released = '0' 
	    AND ReleaseApproved = '0'        -- TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
		AND Interfaced = '1'
		AND HDR_RID <> @defaultHeaderId
		AND h.DISPLAY_TYPE IN 
			(
				SELECT DISPLAY_TYPE FROM @HEADER_TYPE_LIST
			) 
END
GO
