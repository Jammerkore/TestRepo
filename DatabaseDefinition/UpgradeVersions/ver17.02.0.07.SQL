/*RELEASE 17.02.0.07 8-10-2017 TT#1966-MD - JSmith - DC Fulfillment */
IF NOT EXISTS (SELECT * FROM APPLICATION_DB_CONVERSION_QUEUE WHERE CC_FUNCTION_NAME= 'PopulateInUse')
BEGIN
	INSERT INTO APPLICATION_DB_CONVERSION_QUEUE (CC_FUNCTION_NAME, IS_DEFERRED) VALUES ('PopulateInUse', 1)
END

exec SP_MID_UPDATE_TEXT 200093,'DC Fulfillment Method',50,0, null,null,0
exec SP_MID_UPDATE_TEXT 090055,'Monitoring was set to TRUE, but there was a problem creating the log file.  Method will continue, but no monitoring will be done.',250,0,5,null,0

GO
