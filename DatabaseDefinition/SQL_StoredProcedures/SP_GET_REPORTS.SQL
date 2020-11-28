CREATE PROCEDURE [dbo].[SP_GET_REPORTS] 
	@ShowEligibility INT,
	@ShowModifiers INT,
	@ShowSimilarStore INT,
	@ShowStoreGrades INT,
	@ShowAllocationMinMax INT,
	@ShowVelocityGrades INT,
	@ShowCapacity INT,
	@ShowDailypercentages INT,
	@ShowPurgeCriteria INT,
	@ShowForecastLevel INT,
	@ShowForecastType INT,
	@ShowStockMinMax INT,
	@ShowCharacteristics INT,
	@ShowChainSetPercent INT,
	@ShowVSW INT,
	@ShowSizeCurvesCriteria INT,
	@ShowSizeCurvesTolerance INT,
	@ShowSizeCurvesSimilarStores INT
AS
BEGIN
	SELECT REPORT_NAME
	FROM (
		SELECT CASE 
				WHEN @ShowEligibility = 1
					OR @ShowModifiers = 1
					OR @ShowSimilarStore = 1
					THEN 'Store Eligibility'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowStoreGrades = 1
					OR @ShowAllocationMinMax = 1
					THEN 'Store Grades'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowVelocityGrades = 1
					THEN 'Velocity Grades'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowCapacity = 1
					THEN 'Store Capacity'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowDailypercentages = 1
					THEN 'Daily Percentages'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowPurgeCriteria = 1
					THEN 'Purge Criteria'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowForecastLevel = 1
					THEN 'OTS ForecastLevel'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowForecastType = 1
					THEN 'OTS ForecastType'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowStockMinMax = 1
					THEN 'Stock Min/Max'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowCharacteristics = 1
					THEN 'Characteristics'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowChainSetPercent = 1
					THEN 'Chain Set Percentages'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowVSW = 1
					THEN 'VSW'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowSizeCurvesCriteria = 1
					THEN 'Size Curves Criteria'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowSizeCurvesTolerance = 1
					THEN 'Size Curves Tolerance'
				ELSE NULL
				END AS [REPORT_NAME]
		
		UNION
		
		SELECT CASE 
				WHEN @ShowSizeCurvesSimilarStores = 1
					THEN 'Size Curves Similar Stores'
				ELSE NULL
				END AS [REPORT_NAME]
		) R
	WHERE REPORT_NAME IS NOT NULL
END
GO


