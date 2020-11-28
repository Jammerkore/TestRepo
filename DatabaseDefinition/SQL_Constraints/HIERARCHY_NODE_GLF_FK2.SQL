alter table "GROUP_LEVEL_FUNCTION"
	add constraint "HIERARCHY_NODE_GLF_FK2" foreign key (
		"SEASON_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
