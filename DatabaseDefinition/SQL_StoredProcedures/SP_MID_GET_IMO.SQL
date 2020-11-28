CREATE PROCEDURE [dbo].[SP_MID_GET_IMO] 
	@HN_List GET_IMO_HN_TYPE READONLY,
	@FlashBack DATETIME OUTPUT,
	@Return_Code INT = - 100 OUTPUT,
	@debug BIT = 0
AS
IF (@debug = 0)
	SET NOCOUNT ON
SET @FlashBack = COALESCE(@FlashBack, getdate())

IF @debug <> 0
	SELECT @FlashBack

/*===================*/
/* load Node table   */
/*===================*/
DECLARE @hn_tbl TABLE (hn INT PRIMARY KEY (hn))

INSERT @hn_tbl
SELECT *
FROM @HN_List

IF @debug <> 0
	SELECT *
	FROM @hn_tbl

/*===================*/
/* Read STORE_IMO    */
/*===================*/
DECLARE @st_inv TABLE (
	ST_RID INT,
	UNITS INT
	)

INSERT INTO @st_inv
SELECT ST_RID,
	sum(UNITS) AS UNITS
FROM dbo.STORE_IMO I
JOIN @hn_tbl H ON I.HN_RID = H.hn
GROUP BY ST_RID
OPTION (HASH GROUP)

/*=======================*/
/* Read STORE_IMO_REV    */
/*=======================*/
INSERT INTO @st_inv
SELECT ST_RID,
	sum(UNITS) AS UNITS
FROM dbo.STORE_IMO_REV I
JOIN @hn_tbl H ON I.HN_RID = H.hn
WHERE I.FLASHBACK > @FlashBack
GROUP BY ST_RID
OPTION (HASH GROUP)

SELECT ST_RID,
	sum(UNITS) AS UNITS
FROM @st_inv
GROUP BY ST_RID
OPTION (HASH GROUP)

SET @Return_Code = @@ROWCOUNT

RETURN @Return_Code
GO


