/* Add the remaining keys, constraints and indexes for the table "TASKLIST".                  */
create unique index "TASKLIST_NAME_USER_RID" on "TASKLIST" (
	"TASKLIST_NAME",
	"USER_RID")  


go
