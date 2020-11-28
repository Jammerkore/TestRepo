alter table "COMPONENT_CRITERIA"
	add constraint "COLOR_CD_COMP_CRIT_FK1" foreign key (
		"COLOR_CODE_RID")
	 references "COLOR_CODE" (
		"COLOR_CODE_RID")  

go
