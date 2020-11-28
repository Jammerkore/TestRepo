CREATE PROCEDURE [dbo].[SP_MID_DETAIL_ACCESS] 
	@inUseType INT,
	@inUseRID INT,
	@outAllowDelete INT OUTPUT
AS
BEGIN
	DECLARE @seq INT
	DECLARE @severity CHAR(10)
	DECLARE @inUseSql NVARCHAR(4000)
	DECLARE @ParmDefinition NVARCHAR(500)
	DECLARE @sqlStatement NVARCHAR(4000)

	/* These fields are the real output fields for my Data Table. */
	CREATE TABLE #TEMP (
		Severity CHAR(10) NULL, -- Object in question severity.
		Header1 VARCHAR(100) NOT NULL, -- Object in question name.
		Header2 VARCHAR(100) NULL, -- Object in question type.
		Header3 VARCHAR(100) NULL, -- Object in use name.
		Header4 VARCHAR(100) NULL, -- Object in use description.
		Header5 VARCHAR(100) NULL, -- Object in use folder.
		Header6 VARCHAR(100) NULL, -- Object in use UserID.
		Header7 VARCHAR(100) NULL, -- Object in use Reference.
		Header8 VARCHAR(100) NULL, -- Object in use Reference.
		Header9 VARCHAR(100) NULL -- Object in use Reference.
		)

	DECLARE C1 CURSOR
	FOR
	SELECT APPLICATION_LABEL_TYPE,
		SEQUENCE,
		SEVERITY,
		[dbo].[UDF_MID_ENC_DEC](5, REFERENCE_TEXT)
	FROM DETAIL_INFO
	WHERE APPLICATION_LABEL_TYPE = @inUseType
	ORDER BY SEQUENCE

	/* from   DETAIL_INFO where APPLICATION_LABEL_TYPE = ###### Order by SEQUENCE  
 ###### would be the enum for the thing you wanted, such as a 'velocity method' */
	OPEN C1

	FETCH NEXT
	FROM C1
	INTO @inUseType,
		@seq,
		@severity,
		@inUseSql

	WHILE @@FETCH_STATUS = 0
	BEGIN
		/* At this point you have the first results row from the DETAIL_INFO table 
	Pull out the SQL in @inUseSql, replace it to include the RID you care about, 
	execute it and insert results into the #temp table. */
		SET @ParmDefinition = '@inUseRID int, @severity char(10)'
		SET @sqlStatement = @inUseSql

		EXECUTE sp_executesql @sqlStatement,
			@ParmDefinition,
			@inUseRID = @inUseRID,
			@severity = @severity

		FETCH NEXT
		FROM C1
		INTO @inUseType,
			@seq,
			@severity,
			@inUseSql
	END

	CLOSE C1

	DEALLOCATE C1

	--print '@inUseType = ' + CONVERT(nvarchar, @inUseType)
	--print '@seq = ' + CONVERT(nvarchar, @seq)
	--print '@inUseRID = ' + CONVERT(nvarchar, @inUseRID)
	--print '@sqlStatement= ' + @sqlStatement
	--print '@ParmDefinition = ' + CONVERT(varchar, @ParmDefinition)
	--This query tests the population of the #TEMP table.
	SET @outAllowDelete = 1;

	IF EXISTS (
			SELECT TOP 1 *
			FROM #TEMP
			WHERE Severity = 'In Use'
			)
	BEGIN
		SET @outAllowDelete = 0
	END

	SELECT DISTINCT Header1,
		Header2,
		Header3,
		Header4,
		Header5,
		Header6,
		Header7,
		Header8,
		Header9
	FROM #TEMP
END
GO


