alter table "METHOD_GENERAL_ALLOCATION"
	add constraint "HDR_METH_GEN_AL_FK1" foreign key (
		"GEN_ALLOC_HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
