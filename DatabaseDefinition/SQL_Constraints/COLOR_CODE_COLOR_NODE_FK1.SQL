alter table "COLOR_NODE"
	add constraint "COLOR_CODE_COLOR_NODE_FK1" foreign key (
		"COLOR_CODE_RID")
	 references "COLOR_CODE" (
		"COLOR_CODE_RID")  

go
