/*RELEASE 5.2.1.13 01-27-2014 TT#3600 -jsobek -Add a default email address on the global options screen...  */
IF .dbo.UDF_ColumnAlreadyExists('SYSTEM_OPTIONS', 'SMTP_FROM_ADDRESS')=0
BEGIN
	ALTER TABLE SYSTEM_OPTIONS ADD SMTP_FROM_ADDRESS varchar(250) default 'app@midretail.com' not null
END
GO