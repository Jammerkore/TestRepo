/* Add the remaining keys, constraints and indexes for the table "HIER_NODE_JOIN".            */
create index "HIER_NODE_PARENT_IDX" on "HIER_NODE_JOIN" (
	"PARENT_HN_RID")  


go
