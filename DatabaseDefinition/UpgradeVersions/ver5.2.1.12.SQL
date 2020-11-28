/*RELEASE 5.2.1.12 03-19-2014 TT#1076-MD -jsobek -Size Day to Week Summary - Positive Sales Option & Color Level Fields */
IF .dbo.UDF_ColumnAlreadyExists('SYSTEM_OPTIONS', 'USE_NET_SALES_IN_SIZE_DAY_TO_WEEK_SUMMARY')=0
BEGIN
	ALTER TABLE SYSTEM_OPTIONS ADD USE_NET_SALES_IN_SIZE_DAY_TO_WEEK_SUMMARY char(1) default '0' null
END
GO