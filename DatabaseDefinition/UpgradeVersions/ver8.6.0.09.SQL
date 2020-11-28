/*RELEASE 8.6.0.09 02-27-2017 TT#1938-MD - JSmith - VSW - Store Profiles- Reserve Store = BLANK   Node Properties VSW TAB Reserve Store = 0 */

UPDATE STORES
SET IMO_ID = null
WHERE ST_ID = 'Reserve'
AND IMO_ID = '0'

GO
