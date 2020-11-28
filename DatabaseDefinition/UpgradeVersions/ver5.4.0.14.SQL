/*RELEASE 5.4.0.14 11-06-2014 -TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition */
exec SP_MID_UPDATE_TEXT 110001,'A Filter with the name you specified already exists.  Please choose a different name.',250,0,6,null,0
GO
