/*RELEASE 5.4.0.34 12-17-2014 -TT#898-MD -RMatelic -Add a prompt when the user closes the MID app using the "x" in the top right corner */
exec SP_MID_SET_TEXT 112004,'Are you sure you want to sign out of the application?',250,0,6,null,0 
exec SP_MID_SET_TEXT 900899,'Show Sign Off Prompt',30,1,2,0,0 
GO

IF .dbo.UDF_ColumnAlreadyExists('USER_OPTIONS', 'SHOW_SIGNOFF_PROMPT')=0
BEGIN
    ALTER TABLE USER_OPTIONS ADD SHOW_SIGNOFF_PROMPT char(1) null
END
GO
