alter table "DETAIL_RULE_LAYER"
	add constraint "METHOD_DETAIL_RULE_LAYER_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
