-- =============================================
-- Description:	Gets forecast audit information by styleRID, dates, methods and users
-- =============================================
CREATE PROCEDURE [dbo].[MID_GET_FORECAST_ANALYSIS] 
@RestrictToDescendantsOf_HN_RID INT = -1,
@RestrictToLowerLevelSequence INT=-1,

@UseDateRange BIT = 0,
@StartDate DATETIME,
@EndDate DATETIME,

@UseForecastDateRange BIT = 0,
@ForecastStartDate DATETIME,
@ForecastEndDate DATETIME,

@ResultLimit INT=-1,

@RestrictMethods BIT = 0,
@MethodRIDsToInclude VARCHAR(MAX),

@RestrictUsers BIT = 0,
@UserRIDsToInclude VARCHAR(MAX),

@RestrictStoreForecastVersions BIT = 0,
@StoreForecastVersionRIDsToInclude VARCHAR(MAX),

@RestrictChainForecastVersions BIT = 0,
@ChainForecastVersionRIDsToInclude VARCHAR(MAX)

AS 
BEGIN 
	 
	IF (@RestrictToDescendantsOf_HN_RID IS NULL)
		SET @RestrictToDescendantsOf_HN_RID = -1

	DECLARE @METHOD_RIDS_TO_INCLUDE TABLE 
	(  
		METHOD_RID int
		PRIMARY KEY (METHOD_RID)
	)

	IF (@RestrictMethods = 1)
	BEGIN
	    INSERT INTO @METHOD_RIDS_TO_INCLUDE SELECT DISTINCT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@MethodRIDsToInclude, ',') 
	END

	DECLARE @USER_RIDS_TO_INCLUDE TABLE 
	(  
		USER_RID int
		PRIMARY KEY (USER_RID)
	)
	IF (@RestrictUsers = 1)
	BEGIN
	    INSERT INTO @USER_RIDS_TO_INCLUDE SELECT DISTINCT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@UserRIDsToInclude, ',') 
	END

	DECLARE @STORE_FORECAST_VERSION_RIDS_TO_INCLUDE TABLE 
	(  
		FV_RID int
		PRIMARY KEY (FV_RID)
	)
	IF (@RestrictStoreForecastVersions = 1)
	BEGIN
	    INSERT INTO @STORE_FORECAST_VERSION_RIDS_TO_INCLUDE SELECT DISTINCT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@StoreForecastVersionRIDsToInclude, ',') 
	END

	DECLARE @CHAIN_FORECAST_VERSION_RIDS_TO_INCLUDE TABLE 
	(  
		FV_RID int
		PRIMARY KEY (FV_RID)
	)
	IF (@RestrictChainForecastVersions = 1)
	BEGIN
	    INSERT INTO @CHAIN_FORECAST_VERSION_RIDS_TO_INCLUDE SELECT DISTINCT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@ChainForecastVersionRIDsToInclude, ',') 
	END

	DECLARE @HN_RIDS_TO_INCLUDE TABLE 
	(  
		HN_RID int
		PRIMARY KEY (HN_RID)
	)
	IF NOT (@RestrictToDescendantsOf_HN_RID IS NULL OR @RestrictToDescendantsOf_HN_RID = -1)
	BEGIN
		INSERT INTO @HN_RIDS_TO_INCLUDE SELECT HN_RID FROM [dbo].[UDF_HIERARCHY_GET_ALL_DESCENDANT_NODES] (@RestrictToDescendantsOf_HN_RID)
	END
	

	DECLARE @ResultTable TABLE 
	(  
		[Process Date/Time] datetime,
		[Process User] varchar(256),
		[Forecast Node] varchar(150),
		[Method Name] varchar(50),
		[Method User] varchar(256),
		[Method Node] varchar(50),
		[Method Level] varchar(50),
		[Forecast Time Range] varchar(50),
		[OTS Begining] datetime,
		[OTS Ending] datetime,

		[Store Version] varchar(50),
		[Chain Version] varchar(50),
		[Active] char(1),
		[High Level] char(1),
		[Low Level] char(1),
		[HN_RID] int
	)

	 INSERT INTO @ResultTable SELECT * FROM
	 (
	

	SELECT
		AF.PROCESS_DATE_TIME as [Process Date/Time], 
		AU.USER_NAME as [Process User], 
		--bn.BN_ID as 'Forecast ID', 
		AF.HN_TEXT as [Forecast Node], 
		AF.METHOD_NAME as [Method Name], 
		AU2.USER_NAME as [Method User],
		BN1.BN_ID as [Method Node],
		PHL.PHL_ID as [Method Level],
		AF.TIME_RANGE_DISPLAY as [Forecast Time Range], 
		--AF.TIME_RANGE_BEGIN as 'OTS Begining', 
		--AF.TIME_RANGE_END as 'OTS Ending',

		 [dbo].[UDF_DATE_GET_DATETIME_FROM_JULIAN_DAY_DD_FORMAT] (AF.TIME_RANGE_BEGIN) AS [OTS Begining],
		 [dbo].[UDF_DATE_GET_DATETIME_FROM_JULIAN_DAY_DD_FORMAT] (AF.TIME_RANGE_END) AS [OTS Ending],

		FVS.DESCRIPTION as [Store Version],
		FVC.DESCRIPTION as [Chain Version],

		CASE 
		 WHEN HN.ACTIVE_IND = 0 THEN 'N'
		 ELSE 'Y'
		END as [Active],
		CASE 
		 WHEN M.HIGH_LEVEL_IND = 0 THEN 'N'
		 ELSE 'Y'
		END as [High Level],  
		CASE 
		 WHEN M.LOW_LEVELS_IND= 0 THEN 'N'
		 ELSE 'Y'
		END as [Low Level],
		HN.HN_RID

	FROM AUDIT_FORECAST AF WITH (NOLOCK)
	INNER JOIN APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_RID = AF.USER_RID
	INNER JOIN HIERARCHY_NODE HN  WITH (NOLOCK) ON HN.HN_RID = AF.HN_RID
	INNER JOIN PRODUCT_HIERARCHY_LEVELS PHL WITH (NOLOCK) ON PHL.PHL_SEQUENCE=HN.HOME_LEVEL
	INNER JOIN BASE_NODE BN WITH (NOLOCK) ON BN.HN_RID = AF.HN_RID 
	INNER JOIN OTS_PLAN M WITH (NOLOCK) ON M.METHOD_RID = AF.METHOD_RID
	INNER JOIN METHOD MM WITH (NOLOCK) ON  MM.METHOD_RID = AF.METHOD_RID
	INNER JOIN APPLICATION_USER AU2 WITH (NOLOCK) ON MM.USER_RID = AU2.USER_RID
	INNER JOIN BASE_NODE BN1 WITH (NOLOCK) ON BN1.HN_RID = M.PLAN_HN_RID
	INNER JOIN FORECAST_VERSION FVS ON AF.STORE_FV_RID = FVS.FV_RID
	INNER JOIN FORECAST_VERSION FVC ON AF.CHAIN_FV_RID = FVC.FV_RID
	
	WHERE 
	(
		(@RestrictToLowerLevelSequence IS NULL OR @RestrictToLowerLevelSequence = -1) OR (HN.HOME_LEVEL <= @RestrictToLowerLevelSequence)
	)
	AND
	(
		@UseDateRange=0 OR (AF.PROCESS_DATE_TIME >= @StartDate)
	)
	AND
	(
		@UseDateRange=0 OR (AF.PROCESS_DATE_TIME <= @EndDate)
	)
	AND
	(
		@UseForecastDateRange=0 OR ([dbo].[UDF_DATE_GET_DATETIME_FROM_JULIAN_DAY_DD_FORMAT] (AF.TIME_RANGE_BEGIN) >= @ForecastStartDate)
	)
	AND
	(
		@UseForecastDateRange=0 OR ([dbo].[UDF_DATE_GET_DATETIME_FROM_JULIAN_DAY_DD_FORMAT] (AF.TIME_RANGE_END) <= @ForecastEndDate)
	)
	AND
	(
		@RestrictMethods=0 OR (AF.METHOD_RID IN (SELECT METHOD_RID FROM @METHOD_RIDS_TO_INCLUDE))
	)
	AND
	(
		@RestrictUsers=0 OR (AF.USER_RID IN (SELECT USER_RID FROM @USER_RIDS_TO_INCLUDE))
	)
	AND
	(
		@RestrictStoreForecastVersions=0 OR (AF.STORE_FV_RID IN (SELECT FV_RID FROM @STORE_FORECAST_VERSION_RIDS_TO_INCLUDE))
	)
	AND
	(
		@RestrictChainForecastVersions=0 OR (AF.CHAIN_FV_RID IN (SELECT FV_RID FROM @CHAIN_FORECAST_VERSION_RIDS_TO_INCLUDE))
	)
	
	) AS T

	
	
	IF (@ResultLimit IS NULL OR @ResultLimit=-1)
		BEGIN
			IF (@RestrictToDescendantsOf_HN_RID IS NULL OR @RestrictToDescendantsOf_HN_RID = -1)
				SELECT * FROM @ResultTable ORDER BY [Process Date/Time]
			ELSE
				SELECT RT.* FROM @ResultTable RT INNER JOIN @HN_RIDS_TO_INCLUDE HNR ON RT.HN_RID=HNR.HN_RID ORDER BY [Process Date/Time]
		END
	ELSE
		BEGIN
			IF (@RestrictToDescendantsOf_HN_RID IS NULL OR @RestrictToDescendantsOf_HN_RID = -1)
				SELECT TOP (@ResultLimit) * FROM @ResultTable ORDER BY [Process Date/Time]
			ELSE
				SELECT TOP (@ResultLimit) RT.* FROM @ResultTable RT INNER JOIN @HN_RIDS_TO_INCLUDE HNR ON RT.HN_RID=HNR.HN_RID ORDER BY [Process Date/Time]
		END
	
END
GO
