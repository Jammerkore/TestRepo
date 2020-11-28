alter table "METHOD_ROLLUP"
	add constraint "HIER_NODE_MTHD_ROLLUP_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID") on update no action on delete no action  

go
