CREATE PROCEDURE [dbo].[SP_MID_UPDATE_IMO] 
	@xml TEXT,
	@ReturnCode INT = - 100 OUTPUT,
	@debug INT = 1
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
	Incr INT
	)
DECLARE @Accumtbl TABLE (
	HN_RID INT,
	S_RID INT,
	Incr INT
	)
DECLARE @Updatetbl TABLE (
	Flag CHAR,
	HN_RID INT,
	S_RID INT,
	Incr INT
	)

EXEC @rtncd = sp_xml_preparedocument @tree OUTPUT,
	@xml

IF @debug <> 0
	PRINT @rtncd

INSERT @Inttbl
SELECT *
FROM OPENXML(@tree, '/UpdateIMO/IMO/Str') WITH (
		Par_RID INT '../@pSty_RID',
		Sty_RID INT '../@Sty_RID',
		C_RID INT '../@C_RID',
		Sz_RID INT '../@Sz_RID',
		S_RID INT,
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
	S_RID,
	Incr
FROM @Inttbl
WHERE Par_RID IS NOT NULL

INSERT @Accumtbl
SELECT Sty_RID,
	S_RID,
	Incr
FROM @Inttbl
WHERE Sty_RID IS NOT NULL

INSERT @Accumtbl
SELECT C_RID,
	S_RID,
	Incr
FROM @Inttbl
WHERE C_RID IS NOT NULL

INSERT @Accumtbl
SELECT Sz_RID,
	S_RID,
	Incr
FROM @Inttbl
WHERE Sz_RID IS NOT NULL

DECLARE @tot INT,
	@ins INT,
	@upd INT,
	@del INT,
	@noaction INT

INSERT @Updatetbl (
	Flag,
	HN_RID,
	S_RID,
	Incr
	)
SELECT 'I',
	HN_RID,
	S_RID,
	sum(Incr)
FROM @Accumtbl
GROUP BY HN_RID,
	S_RID
HAVING sum(Incr) <> 0
ORDER BY HN_RID,
	S_RID

SET @tot = @@ROWCOUNT

UPDATE @Updatetbl
SET Flag = 'U',
	Incr = Incr + UNITS
FROM @Updatetbl U
JOIN dbo.STORE_IMO I ON I.HN_RID = U.HN_RID
	AND I.ST_RID = U.S_RID

UPDATE @Updatetbl
SET Flag = 'D'
WHERE Incr < 1
	AND Flag = 'U'

UPDATE @Updatetbl
SET Flag = 'N'
WHERE Incr < 1
	AND Flag = 'I'

SET @noaction = @@ROWCOUNT

UPDATE dbo.STORE_IMO
SET UNITS = Incr
FROM dbo.STORE_IMO I
JOIN @Updatetbl U ON I.HN_RID = U.HN_RID
	AND I.ST_RID = U.S_RID
WHERE Flag = 'U'

SET @upd = @@ROWCOUNT

INSERT dbo.STORE_IMO (
	HN_RID,
	ST_RID,
	UNITS
	)
SELECT HN_RID,
	S_RID,
	Incr
FROM @Updatetbl
WHERE Flag = 'I'

SET @ins = @@ROWCOUNT

DELETE dbo.STORE_IMO
WITH (ROWLOCK)
FROM @Updatetbl U
JOIN dbo.STORE_IMO I ON I.HN_RID = U.HN_RID
	AND I.ST_RID = U.S_RID
WHERE Flag = 'D'

SET @del = @@ROWCOUNT

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

IF @tot = (@ins + @upd + @del + @noaction)
	SET @ReturnCode = @tot - @noaction
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

RETURN @ReturnCode
GO


