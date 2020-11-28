/*RELEASE 5.4.0.05 10-20-2014 TT#1317-MD - stodd - Store Load errors when transaction file is delimited */
exec SP_MID_UPDATE_TEXT 060070, 'Store {0}. Transaction does not contain the minimum number of expected fields. This typically indicates one or more missing delimiters or that the delimiter defined in the config file ({1}) does not match the input file delimiter.',250,0,5,null,0
GO
