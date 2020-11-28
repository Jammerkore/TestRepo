--dv =============================================
--dv Create date:	8/14/2014
--dv Description:	gets all non-multi headers with the given color within a given style
--dv History:		TT#0851-VStuart-SQL error in Rename Color–MID
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_GET_STYL_HDRS_WITH_COLR] 
	@STYLE_HNRID INT,
	@COLOR_CODE_RID INT
AS
BEGIN

	SELECT HDR_RID,
		HDR_GROUP_RID,
		HDR_ID
	FROM HEADER
	WHERE (STYLE_HNRID = @STYLE_HNRID)
		AND (
			HDR_RID NOT IN (
				SELECT HDR_RID
				FROM (
					SELECT HDR_RID
					FROM HEADER
					WHERE (
							HDR_GROUP_RID > 1
							AND HDR_RID = HDR_GROUP_RID
							)
					) AS T
				)
			)
		AND (
			HDR_RID IN (
				SELECT HDR_RID
				FROM HEADER_BULK_COLOR
				WHERE COLOR_CODE_RID = @COLOR_CODE_RID
				)
			OR HDR_RID IN (
				SELECT HDR_RID
				FROM HEADER_PACK
				WHERE HDR_PACK_RID IN (
						SELECT HDR_PACK_RID
						FROM HEADER_PACK_COLOR
						WHERE COLOR_CODE_RID = @COLOR_CODE_RID
						)
				)
			)
END
GO


