/* Add foreign key constraints to table "METHOD_RULE".                                        */
alter table "METHOD_RULE"
	add constraint "METHOD_METHOD_RULE_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
