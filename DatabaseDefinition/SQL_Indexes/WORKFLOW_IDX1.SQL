/* Add the remaining keys, constraints and indexes for the table "WORKFLOW".                  */
create unique index "WORKFLOW_IDX1" on "WORKFLOW" (
	"WORKFLOW_NAME",
	"WORKFLOW_TYPE_ID",
	"WORKFLOW_USER_RID")  


go
