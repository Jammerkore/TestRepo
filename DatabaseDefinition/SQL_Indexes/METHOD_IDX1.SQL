/* Add the remaining keys, constraints and indexes for the table "METHOD".                    */
create unique index "METHOD_IDX1" on "METHOD" (
	"METHOD_NAME",
	"METHOD_TYPE_ID",
	"USER_RID")  


go
