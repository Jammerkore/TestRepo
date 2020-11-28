/*RELEASE 5.2.1.10 01-22-2014 TT#3475 - agallagher - Add the Store Allocated Units and VSW Allocated Units breakdown to the Allocation Workspace */
exec SP_MID_UPDATE_TEXT 900810,'Store Allocated Units',30,1,2,null,0
exec SP_MID_UPDATE_TEXT 900811,'VSW Allocated Units',30,1,2,null,0
GO
