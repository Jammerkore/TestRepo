ALTER TABLE "PURGE_CRITERIA_HEADER_PURGE"
	add constraint "NODE_PURGE_CRITERIA_HP_FK1" foreign key (
		"HN_RID")
	references "HIERARCHY_NODE" (
		"HN_RID")
go
