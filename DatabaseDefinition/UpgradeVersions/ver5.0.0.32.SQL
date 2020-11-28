/*COMMENT To insert comments to be ignored by the Database Update utility                                        ==> /*COMMENT comment */  */
/*COMMENT For Release update for 2.1.4 and prior where x = Major, y = Minor, and z = Revision                   ==> /*RELEASE x.y.z mm-dd-yy developer - description */ */ 
/*COMMENT For Release update for 2.1.5 and later where w = Major, x = Minor, y = Revision, and z = Modification ==> /*RELEASE w.x.y.z mm-dd-yy developer - description */ */
/*COMMENT For Version Control updates that must be run before the rest of the script                            ==> /*VERSIONING mm-dd-yy  developer - description */ */
/*COMMENT To execute a custom conversion class defined as ICustomConversion within a Release/Versioning header  ==> /*EXECUTE ICustomConversion */ */
/*COMMENT To execute the DatabaseConversionUtility for a specific Conversion                                    ==> /*RUNCONVERSION Conversion */ */

/*RELEASE 5.0.0.32 12-20-2013 - TT#3424 - JSmith - ROLLUP_ITEM has 'bogus' entries with HN_RID = 0 */
delete from ROLLUP_ITEM where HN_RID = 0
go
