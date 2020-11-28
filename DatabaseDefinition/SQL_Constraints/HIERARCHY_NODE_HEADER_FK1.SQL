/* Add foreign key constraints to table "HEADER".                                             */
alter table "HEADER"
	add constraint "HIERARCHY_NODE_HEADER_FK1" foreign key (
		"STYLE_HNRID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
