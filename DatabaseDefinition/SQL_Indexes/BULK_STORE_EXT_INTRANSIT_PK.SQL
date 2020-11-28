/* Add the remaining keys, constraints and indexes for the table "BULK_STORE_EXTERNAL_INTRANSIT". */
create index "BULK_STORE_EXT_INTRANSIT_PK" on "BULK_STORE_EXTERNAL_INTRANSIT" (
	"HN_RID",
	"TIME_ID",
	"ST_RID")  


go
