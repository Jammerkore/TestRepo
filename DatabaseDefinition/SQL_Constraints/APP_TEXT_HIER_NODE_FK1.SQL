/* Add foreign key constraints to table "HIERARCHY_NODE".                                     */
alter table "HIERARCHY_NODE"
	add constraint "APP_TEXT_HIER_NODE_FK1" foreign key (
		"OTS_PLANLEVEL_TYPE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
