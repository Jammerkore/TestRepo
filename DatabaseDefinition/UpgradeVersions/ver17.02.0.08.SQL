/*RELEASE 17.02.0.08 8-23-2017 TT#2034-MD - JSmith - In Use - Asst Type in PH Style - creates in the Mdse Hierarchy - when select In Use on the Node Type = Header and there is no reference to an Assortment */
IF NOT EXISTS (SELECT * FROM APPLICATION_DB_CONVERSION_QUEUE WHERE CC_FUNCTION_NAME= 'PopulateInUse')
BEGIN
	INSERT INTO APPLICATION_DB_CONVERSION_QUEUE (CC_FUNCTION_NAME, IS_DEFERRED) VALUES ('PopulateInUse', 1)
END

GO
