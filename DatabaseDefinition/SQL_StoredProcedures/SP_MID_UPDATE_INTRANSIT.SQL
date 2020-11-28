CREATE PROCEDURE [dbo].[SP_MID_UPDATE_INTRANSIT] 
	@xml TEXT,
	@ReturnCode INT = - 100 OUTPUT,
	@debug INT = 0
AS
IF @debug = 0
	SET NOCOUNT ON

DECLARE @tree INT,
	@rtncd INT
DECLARE @Inttbl TABLE (
	Par_RID INT,
	Sty_RID INT,
	C_RID INT,
	Sz_RID INT,
	S_RID INT,
	Dy INT,
	Incr INT
	)
DECLARE @Accumtbl TABLE (
	HN_RID INT,
	Dy INT,
	S_RID INT,
	Incr INT
	)
DECLARE @Updatetbl TABLE (
	Flag CHAR,
	HN_RID INT,
	Dy INT,
	S_RID INT,
	Incr INT
	)

EXEC @rtncd = sp_xml_preparedocument @tree OUTPUT,
	@xml

IF @debug <> 0
	PRINT @rtncd

INSERT @Inttbl
SELECT *
FROM OPENXML(@tree, '/UpdateIntransit/Intransit/Str') WITH (
		Par_RID INT '../@pSty_RID',
		Sty_RID INT '../@Sty_RID',
		C_RID INT '../@C_RID',
		Sz_RID INT '../@Sz_RID',
		S_RID INT,
		Dy INT,
		Incr INT
		)

EXEC @rtncd = sp_xml_removedocument @tree

IF @debug <> 0
BEGIN
	PRINT @rtncd

	SELECT *
	FROM @Inttbl
END

INSERT @Accumtbl
SELECT Par_RID,
	Dy,
	S_RID,
	Incr
FROM @Inttbl
WHERE Par_RID IS NOT NULL

INSERT @Accumtbl
SELECT Sty_RID,
	Dy,
	S_RID,
	Incr
FROM @Inttbl
WHERE Sty_RID IS NOT NULL

INSERT @Accumtbl
SELECT C_RID,
	Dy,
	S_RID,
	Incr
FROM @Inttbl
WHERE C_RID IS NOT NULL

INSERT @Accumtbl
SELECT Sz_RID,
	Dy,
	S_RID,
	Incr
FROM @Inttbl
WHERE Sz_RID IS NOT NULL

DECLARE @tot INT,
	@ins INT,
	@upd INT,
	@del INT,
	@noaction INT /* MID Track 5694: added "noaction" count to identify deletes that required an insert */

INSERT @Updatetbl (
	Flag,
	HN_RID,
	Dy,
	S_RID,
	Incr
	)
SELECT 'I',
	HN_RID,
	Dy,
	S_RID,
	sum(Incr)
FROM @Accumtbl
GROUP BY HN_RID,
	Dy,
	S_RID
HAVING sum(Incr) <> 0
ORDER BY HN_RID,
	Dy,
	S_RID

SET @tot = @@rowcount

UPDATE @Updatetbl
SET Flag = 'U',
	Incr = Incr + UNITS
FROM @Updatetbl U
JOIN dbo.STORE_INTRANSIT I ON I.HN_RID = U.HN_RID
	AND I.TIME_ID = U.Dy
	AND I.ST_RID = U.S_RID

UPDATE @Updatetbl
SET Flag = 'D'
WHERE Incr < 1
	AND Flag = 'U' /* MID Track 5694: delete occurs when transaction causes Incr to be less than 1 and there is an entry on STORE_INTRANSIT */

UPDATE @Updatetbl
SET Flag = 'N'
WHERE Incr < 1
	AND Flag = 'I' /* MID Track 5694: noaction occurs when transaction is an insert but Incr is less than 1 */

SET @noaction = @@rowcount

UPDATE dbo.STORE_INTRANSIT
SET UNITS = Incr
FROM dbo.STORE_INTRANSIT I
JOIN @Updatetbl U ON I.HN_RID = U.HN_RID
	AND I.TIME_ID = U.Dy
	AND I.ST_RID = U.S_RID
WHERE Flag = 'U'

SET @upd = @@rowcount

INSERT dbo.STORE_INTRANSIT (
	HN_RID,
	TIME_ID,
	ST_RID,
	UNITS
	)
SELECT HN_RID,
	Dy,
	S_RID,
	Incr
FROM @Updatetbl
WHERE Flag = 'I'

SET @ins = @@rowcount

DELETE dbo.STORE_INTRANSIT
WITH (ROWLOCK) /* TT#929 - Orphaned Intransit More Info */
FROM @Updatetbl U
JOIN dbo.STORE_INTRANSIT I ON I.HN_RID = U.HN_RID
	AND I.TIME_ID = U.Dy
	AND I.ST_RID = U.S_RID
WHERE Flag = 'D'

SET @del = @@rowcount

IF @debug <> 0
BEGIN
	SELECT *
	FROM @Accumtbl

	SELECT *
	FROM @Updatetbl

	SELECT @tot,
		@ins,
		@upd,
		@del,
		@noaction
END

IF @tot = (@ins + @upd + @del + @noaction) /* MID Track 5694: account for the noaction transactions in the total */
	SET @ReturnCode = @tot - @noaction /* MID Track 5694: return number of rows actually affected by the transactions */
		/* begin TT#929 - Orphaned Intransit More Info */
		/* else set @ReturnCode=-101            */
ELSE
BEGIN
	SET @ReturnCode = - 101

	RAISERROR (
			'Total actions not in sync with sub actions:  @tot = %t, @ins = %i, @upd = %u, @del = %d, @noaction = %n',
			16,
			1,
			@tot,
			@ins,
			@upd,
			@del,
			@noaction
			)
END

/* end TT#929 - Orphaned Intransit More Info */
RETURN @ReturnCode
GO


