alter table "HEADER_PACK_COLOR_SIZE"
	add constraint "HPC_PACK_COLOR_SIZE_FK1" foreign key (
		"HDR_PC_RID")
	 references "HEADER_PACK_COLOR" (
		"HDR_PC_RID")  

go
