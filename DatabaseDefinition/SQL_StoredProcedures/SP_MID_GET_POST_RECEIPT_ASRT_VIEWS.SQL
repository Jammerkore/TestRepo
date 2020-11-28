--dv =============================================
--dv Modified date:	8/14/2014
--dv Description:	Gets post recipt assortment views  
--dv History:		TT#1268 - MD - RMatelic - 5.4 Merge
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_GET_POST_RECEIPT_ASRT_VIEWS] 
	(@USER_RID_LIST NVARCHAR(250))
AS
BEGIN
	DECLARE @SEQ INT;
	DECLARE @cmd NVARCHAR(250);
	--DECLARE @USER_RID_LIST nvarchar( 250 );
	DECLARE @ParmDefinition NVARCHAR(250);

	SET @USER_RID_LIST = 'USER_RID IN (-1,131,4)';
	SET @SEQ = 0;

	CREATE TABLE #PostReceiptViews (
		VIEW_RID INT,
		USER_RID INT,
		VIEW_ID VARCHAR(250),
		GROUP_BY_ID INT
		);

	SET @cmd = N'insert into #PostReceiptViews SELECT VIEW_RID, USER_RID, VIEW_ID, GROUP_BY_ID from ASSORTMENT_VIEW WHERE ' + @USER_RID_LIST + ' AND ASSORTMENT_VIEW_TYPE = 1  order by VIEW_ID';
	SET @ParmDefinition = N'@USER_RID_LIST varchar(250)'

	EXEC sp_executesql @cmd

	DECLARE @viewRid INT,
		@profType INT,
		@profKey INT

	DECLARE C1 CURSOR
	FOR
	SELECT VIEW_RID,
		PROFILE_TYPE,
		PROFILE_KEY
	FROM ASSORTMENT_VIEW_DETAIL
	WHERE PROFILE_TYPE = 4
		AND PROFILE_KEY = 804602;

	OPEN C1;

	FETCH NEXT
	FROM C1
	INTO @viewRid,
		@profType,
		@profKey;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE
		FROM #PostReceiptViews
		WHERE VIEW_RID = @viewRid;

		FETCH NEXT
		FROM C1
		INTO @viewRid,
			@profType,
			@profKey;
	END;

	CLOSE C1;

	DEALLOCATE C1;

	SELECT *
	FROM #PostReceiptViews
	ORDER BY VIEW_ID;

	DROP TABLE #PostReceiptViews
END
GO


