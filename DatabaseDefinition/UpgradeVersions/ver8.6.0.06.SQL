/*RELEASE 8.6.0.06 01-11-2017 TT#1903-MD - JSmith - Versioning Test _ Should a user be able to make a store inactive on line? */
exec SP_MID_UPDATE_TEXT 060066, 'Store active flag has been changed.  Changing this flag while other windows are open or other users are logged in may affect the results.  Do you want to continue?',250,0,5,null,0

GO
