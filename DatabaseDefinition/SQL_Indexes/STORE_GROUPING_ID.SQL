/* Add the remaining keys, constraints and indexes for the table "STORE_GROUP".               */
create unique index "STORE_GROUPING_ID" on "STORE_GROUP" (
	"USER_RID", "SG_ID", "SG_VERSION", "FILTER_RID")  


go
