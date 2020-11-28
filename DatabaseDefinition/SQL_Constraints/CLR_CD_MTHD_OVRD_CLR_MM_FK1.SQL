alter table "METHOD_OVERRIDE_COLOR_MINMAX"
	add constraint "CLR_CD_MTHD_OVRD_CLR_MM_FK1" foreign key (
		"COLOR_CODE_RID")
	 references "COLOR_CODE" (
		"COLOR_CODE_RID")  

go
