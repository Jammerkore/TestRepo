alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "CLR_CD_MTHD_SZ_BAS_ALLOC_FK1" foreign key (
		"COLOR_CODE_RID")
	 references "COLOR_CODE" (
		"COLOR_CODE_RID")  

go
