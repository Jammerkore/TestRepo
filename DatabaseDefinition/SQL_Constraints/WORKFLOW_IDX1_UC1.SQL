alter table "WORKFLOW" add constraint "WORKFLOW_IDX1_UC1" unique (
	"WORKFLOW_NAME",
	"WORKFLOW_TYPE_ID",
	"WORKFLOW_USER_RID")  


go
